using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using Synth.Controls;
using Synth.Controls.Envelopes;
using System.Text.RegularExpressions;
using System.Data;

namespace Synth.Pages
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Page
    {
        #region dllimport
        [DllImport("SynthDLL.dll")]
        static extern void InitData(int sz);

        [DllImport("SynthDLL.dll")]
        static extern void WriteData(uint[] data, int sz);

        [DllImport("SynthDLL.dll")]
        static extern bool CreateWaveFile(int samples, int channels, int samplerate);

        [DllImport("SynthDLL.dll")]
        static extern void DestructData();
        #endregion
        
        
        private bool switching_synths = false;

        public Editor()
        {
            InitializeComponent();
            UpdateSynthSelector();
        }

        private void UpdateSynthSelector()
        {
            if (Home.SynthColumns != null)
                foreach (string synth in Home.SynthColumns.Keys)
                {
                    ComboBoxItem newitem = new() { Content = synth };
                    bool contains = false;
                    foreach (ComboBoxItem item in EditorSynthSelector.Items)
                    {
                        if (item.Content.Equals(synth)) { contains = true; break; }
                    }
                    if (!contains) { EditorSynthSelector.Items.Add(newitem); }
                }
                    
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSynthSelector();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            GenerateWave(440);
            
        }

        public void GenerateWave(double basePitch)
        {
            if (EditorSynthSelector.SelectedValue.ToString().Equals("No Synth Selected")) { return; }
#pragma warning disable CS8604 // Possible null reference argument. - if this crashes you deserved it.
            hSynth SelectedSynth = (hSynth)Home.SynthColumns[EditorSynthSelector.Text].hcStackPanel.Children[1];
#pragma warning restore CS8604 // Possible null reference argument.

            switch (WaveformType.SelectedValue.ToString())
            {
                case "Sawtooth":
                    SelectedSynth.GenerateSawWave(Info.C4, CreatePitchList(SelectedSynth.Name), CreateVibratoList(SelectedSynth.Name));
                    break;
                case "Square":
                    SelectedSynth.GenerateSquareWave();
                    break;
                case "Sine":
                    SelectedSynth.GenerateSinWave();
                    break;

            }


            List<uint> baseVals = SelectedSynth.yVals;
            List<double> pitchs = SelectedSynth.pitchs;
            baseVals = ApplyFilter(baseVals, pitchs, SelectedSynth.Name);
            baseVals = ApplyTremolo(baseVals, SelectedSynth.Name);
            baseVals = ApplyVolume(baseVals, SelectedSynth.Name);

            InitData(baseVals.Count);
            WriteData(baseVals.ToArray(), baseVals.Count);
            CreateWaveFile(baseVals.Count, 1, (int)SampleRatesKHZ.OneNineTwo);
            DestructData();
        }

        public List<uint> GenerateWaveWithNote(double basePitch, hSynth synth, int time) 
        {

            

            hSynth SelectedSynth = synth;
            synth.Time = time;
            
            switch (WaveformType.SelectedValue.ToString())
            {
                case "Sawtooth":
                    SelectedSynth.GenerateSawWave(basePitch, CreatePitchList(synth.Name), CreateVibratoList(synth.Name));
                    break;
                case "Square":
                    SelectedSynth.GenerateSquareWave();
                    break;
                case "Sine":
                    SelectedSynth.GenerateSinWave();
                    break;

            }


            List<uint> baseVals = SelectedSynth.yVals;
            List<double> pitchs = SelectedSynth.pitchs;
            baseVals = ApplyFilter(baseVals, pitchs, synth.Name);
            baseVals = ApplyTremolo(baseVals, synth.Name);
            baseVals = ApplyVolume(baseVals, synth.Name);
            return baseVals;
        }

        

        private List<double>? CreatePitchList(string selected)
        {
            
            List<EnvelopeControls> points = Info.PitchControlDict[selected].envControls;
            EnvelopeControls current = points[1];
            List<double> output = new();

            double time = 0;
            double maxtime = Info.TimeDict[selected];
            double interval = 1 / (double)SampleRatesKHZ.OneNineTwo;

            bool hasEq = false;
            string equation = "";

            for (; time < maxtime; time += interval)
            {
                
                if (time > current.Time)
                {
                    current = points[points.IndexOf(current) + 1];
                    hasEq = false;
                }

                if (!hasEq)
                {
                    double m = (current.Val - points[points.IndexOf(current) - 1].Val) / ((current.Time - points[points.IndexOf(current) - 1].Time));
                    double b = current.Val - current.Time * m;
                    equation = b.ToString() + " + " + m.ToString() + " * ";
                    hasEq = true;
                }

                output.Add(Math.Pow(2, Convert.ToDouble(new DataTable().Compute(equation + time.ToString(), null)) / (double)12));
            }
            return output;
        }
        private List<uint> ApplyVolume(List<uint> vals, string selected)
        {
            
            List<EnvelopeControls> points = Info.VolumeControlDict[selected].envControls;
            EnvelopeControls current = points[1];
            List<uint> output = new();

            double time = 0;
            double maxtime = Info.TimeDict[selected];
            double interval = 1 / (double)SampleRatesKHZ.OneNineTwo;
            int i = 0;

            bool hasEq = false;
            string equation = "";

            for (; time < maxtime; time += interval)
            {

                if (time > current.Time)
                {
                    current = points[points.IndexOf(current) + 1];
                    hasEq = false;
                }

                if (!hasEq)
                {
                    double m = (current.Val - points[points.IndexOf(current) - 1].Val) / ((current.Time - points[points.IndexOf(current) - 1].Time));
                    double b = current.Val - current.Time * m;
                    equation = b.ToString() + " + " + m.ToString() + " * ";
                    hasEq = true;
                }

                double value = 1 - Convert.ToDouble(new DataTable().Compute(equation + time.ToString(), null));
                value = uint.MaxValue * value / (double)2;
                if (i < vals.Count && vals[i] > 0.5 * uint.MaxValue)
                {
                    output.Add((uint)EnvelopeControls.remap(
                        vals[i], 
                        0.5 * uint.MaxValue, 
                        uint.MaxValue, 
                        0.5 * uint.MaxValue + value,
                        uint.MaxValue));

                }
                else
                {
                    if (i < vals.Count)
                        output.Add((uint)EnvelopeControls.remap(
                            vals[i],
                            0,
                            0.5 * uint.MaxValue,
                            0,
                            0.5 * uint.MaxValue - value));
                }
                i++;
            }
            return output;
        }
        private List<uint> ApplyFilter(List<uint> vals, List<double> ps, string selected)
        {
            List<uint> output = new();

            
            if (Info.FilterControlDict[selected].state == State.None) { return vals; }

            for (int i = 0; i < ps.Count; i++)
            {
                double pitch = ps[i];
                double? multiplier = Info.FilterControlDict[selected].compute(pitch);
                if (multiplier == null) { continue; }
                double offset = Math.Min(1, Math.Pow(2, (double)multiplier));
                offset = 1 - offset;
                offset *= uint.MaxValue * 0.5;
               if (vals[i] > 0.5 * uint.MaxValue)
                {
                    output.Add((uint)EnvelopeControls.remap(
                        vals[i],
                        0.5 * uint.MaxValue,
                        uint.MaxValue,
                        0.5 * uint.MaxValue + offset,
                        uint.MaxValue));
                }
               else
                {
                    output.Add((uint)EnvelopeControls.remap(
                        vals[i],
                        0,
                        0.5 * uint.MaxValue,
                        0,
                        0.5 * uint.MaxValue - offset));
                }
            }
            uint temp = output[output.Count - 1];
            output.Add(temp);


            return output;
        }
        private List<double>? CreateVibratoList(string selected)
        {

            

            if (Info.VibratoDict[selected][0] == 0) { return null; }
            List<double> output = new();
            double interval = 1 / (double)SampleRatesKHZ.OneNineTwo;
            double time = 0;
            double maxTime = Info.TimeDict[selected];
            double todn = (Math.Pow(2, Info.VibratoDict[selected][0] / (double)12) - Math.Pow(2, -1 * Info.VibratoDict[selected][0] / (double)12)) / (double)2; // that one dumb number

            for (; time < maxTime; time += interval)
            {
                output.Add(todn / 2 * Math.Sin(time * (2 * Math.PI) * Info.VibratoDict[selected][1]) + 1);
            }
            return output;
        }
        private List<uint> ApplyTremolo(List<uint> vals, string selected)
        {
            List<uint> output = new();
            
            int j = 0;

            for (double i = 0; i < Info.TimeDict[selected]; i += 1 / (double)SampleRatesKHZ.OneNineTwo)
            {
                double mult = Math.Sin(i * (2 * Math.PI) * Info.TremoloDict[selected]) ;
                double value = uint.MaxValue * mult / 2;
                if (j < vals.Count && vals[j] > 0.5 * uint.MaxValue)
                {
                    output.Add((uint)EnvelopeControls.remap(
                        vals[j++],
                        0.5 * uint.MaxValue,
                        uint.MaxValue,
                        0.5 * uint.MaxValue + value,
                        uint.MaxValue));

                }
                else
                {
                    if (j < vals.Count)
                        output.Add((uint)EnvelopeControls.remap(
                            vals[j++],
                            0,
                            0.5 * uint.MaxValue,
                            0,
                            0.5 * uint.MaxValue - value));
                }
            }
            return output;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://manual.audacityteam.org/man/change_pitch.html",
                UseShellExecute = true
            });
        }

        private void Time_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }

        private void Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (switching_synths) { return;  }
            if (EditorSynthSelector.Text.Equals("No Synth Selected")) { return; }
#pragma warning disable CS8604 // Possible null reference argument. - if this crashes you deserved it.
            hSynth SelectedSynth = (hSynth)Home.SynthColumns[EditorSynthSelector.Text].hcStackPanel.Children[1];
#pragma warning restore CS8604 // Possible null reference argument.
            int output;
            if (!Int32.TryParse(Time.Text, out output))
            {
                return;
            }
            SelectedSynth.Time = output;
            Info.Time = SelectedSynth.Time;

            string? selected = EditorSynthSelector.SelectedValue.ToString();
            if (selected == null ) { return; }
            if (Info.PitchControlDict.ContainsKey(selected))
                Info.PitchControlDict[selected].NewTime(SelectedSynth.Time);
            if (Info.VolumeControlDict.ContainsKey(selected))
                Info.VolumeControlDict[selected].NewTime(SelectedSynth.Time);
            if (Info.FilterControlDict.ContainsKey(selected))
                Info.FilterControlDict[selected].NewTime(SelectedSynth.Time);
        }

        private void EditorSynthSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switching_synths = true;

            string? newSelection = EditorSynthSelector.SelectedValue.ToString();
            string? oldSelection = e.RemovedItems.Count > 0 ? (e.RemovedItems[0] as ComboBoxItem).Content.ToString() : null;

            HideOldVars(oldSelection);           
            ShowNewVars(newSelection);
            switching_synths = false;
        }

        private void HideOldVars(string? name)
        {
            if (name == null ||name.Equals("No Synth Selected")) { return; }

            Info.PitchControlDict[name].Visibility = Visibility.Collapsed;
            Info.PitchControlDict[name].IsEnabled = false;
            Info.VolumeControlDict[name].Visibility = Visibility.Collapsed;
            Info.VolumeControlDict[name].IsEnabled = false;
            Info.FilterControlDict[name].Visibility = Visibility.Collapsed;
            Info.FilterControlDict[name].IsEnabled = false;

            Info.VibratoDict[name][0] = VibratoSlider.Value;
            Info.VibratoDict[name][1] = VibratoSpeedSlider.Value;
            Info.TremoloDict[name] = TremoloSpeedSlider.Value;
            Info.DetuneDict[name] = DetuneSlider.Value;
            Info.TimeDict[name] = Int32.Parse(Time.Text);

            VibratoSlider.Value = 0;
            VibratoSpeedSlider.Value = 0;
            TremoloSpeedSlider.Value = 0;
            DetuneSlider.Value = 0;           
            VibratoText.Text = "0";
            VibratoSpeedText.Text = "0";
            TremoloSpeedText.Text = "0";
            DetuneText.Text = "0";
            Time.Text = "0";

        }

        private void ShowNewVars(string? name)
        {
            if (name == null || name.Equals("No Synth Selected")) return;
            if (Info.TimeDict.ContainsKey(name))
            {
                Time.Text = Info.TimeDict[name].ToString();
            }
            else
            {
                Info.TimeDict.Add(name, 4);
                if (Time != null)
                    Time.Text = "4";
            }
            

            if (Info.PitchControlDict.ContainsKey(name))
            {
                Info.PitchControlDict[name].Visibility = Visibility.Visible;
                Info.PitchControlDict[name].IsEnabled = true;
            }
            else
            {
                Pitch pitch = new();
                Info.PitchControlDict.Add(name, pitch);
                if (PitchHolder != null)
                    PitchHolder.Children.Add(pitch);
            }

            if (Info.VolumeControlDict.ContainsKey(name))
            {
                Info.VolumeControlDict[name].Visibility = Visibility.Visible;
                Info.VolumeControlDict[name].IsEnabled = true;
            }
            else
            {
                Volume volume = new();
                Info.VolumeControlDict.Add(name, volume);
                if (VolumeHolder != null)
                    VolumeHolder.Children.Add(volume);
            }

            if (Info.FilterControlDict.ContainsKey(name))
            {
                Info.FilterControlDict[name].Visibility = Visibility.Visible;
                Info.FilterControlDict[name].IsEnabled = true;
            }
            else
            {
                Filter filter = new();
                Info.FilterControlDict.Add(name, filter);
                if (FilterHolder != null)
                    FilterHolder.Children.Add(filter);
            }

            if (Info.VibratoDict.ContainsKey(name))
            {
                VibratoSlider.Value = Info.VibratoDict[name][0];
                VibratoSpeedSlider.Value = Info.VibratoDict[name][1];
                VibratoText.Text = Info.VibratoDict[name][0].ToString();
                VibratoSpeedText.Text = Info.VibratoDict[name][1].ToString();
            }
            else
            {
                Info.VibratoDict.Add(name, new double[] {0, 0});
                if (VibratoSlider != null)
                    VibratoSlider.Value = 0;
                if (VibratoSpeedSlider != null)
                    VibratoSpeedSlider.Value = 0;
                if (VibratoText != null)
                    VibratoText.Text = "0";
                if (VibratoSpeedText != null)
                    VibratoSpeedText.Text = "0";
            }
            
            if (Info.TremoloDict.ContainsKey(name))
            {
                TremoloSpeedSlider.Value = Info.TremoloDict[name];
                TremoloSpeedText.Text = Info.TremoloDict[name].ToString();
            }
            else
            {
                Info.TremoloDict.Add(name, 0);
                if (TremoloSpeedSlider != null)
                    TremoloSpeedSlider.Value = 0;
                if (TremoloSpeedText != null)
                    TremoloSpeedText.Text = "0";
            }

            if (Info.DetuneDict.ContainsKey(name))
            {
                DetuneSlider.Value = Info.DetuneDict[name];
                DetuneText.Text = Info.DetuneDict[name].ToString();
            }
            else
            {
                Info.DetuneDict.Add(name, 0);
                if (DetuneSlider != null)
                    DetuneSlider.Value = 0;
                if (DetuneText != null)
                    DetuneText.Text = "0";
            }

            
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            switching_synths = true;
            HideOldVars(EditorSynthSelector.SelectedValue.ToString());
            switching_synths = false;
        }
    }
}
