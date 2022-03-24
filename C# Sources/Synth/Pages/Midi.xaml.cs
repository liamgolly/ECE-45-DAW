using Synth.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Synth.Pages
{
    /// <summary>
    /// Interaction logic for Midi.xaml
    /// </summary>
    public partial class Midi : Page
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
        Dictionary<string, Controls.PianoRoll> rolls = new();
        
        public Midi()
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
                    foreach(ComboBoxItem content in comboBox.Items)
                    {
                        if (content.Content.Equals(synth)) { contains = true; break; }
                    }
                    if (!contains)
                    {
                        comboBox.Items.Add(newitem);
                    }
                }
                    
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? newSelection = comboBox.SelectedValue.ToString();
            string? oldSelection = e.RemovedItems.Count > 0 ? (e.RemovedItems[0] as ComboBoxItem).Content.ToString() : null;
            if (newSelection == null) { return; }

            if (newSelection.Equals("NoSynthSelected"))
            {
                if (RollHolder == null) { return; }
                RollHolder.Content = null;
                return;
            }
                

            

            if (rolls.ContainsKey(newSelection))
            {               
                if (RollHolder != null)
                    RollHolder.Content = rolls[newSelection];
                rolls[newSelection].IsEnabled = true;
                rolls[newSelection].Visibility = Visibility.Visible;                
            }
            else
            {
                if (oldSelection != null && rolls.ContainsKey(oldSelection))
                {
                    rolls[oldSelection].IsEnabled = false;
                    rolls[oldSelection].Visibility = Visibility.Collapsed;
                    RollHolder.Content = null;
                    
                }
                Controls.PianoRoll newRoll = new() { Name = newSelection };
                rolls[newSelection] = newRoll;
                rolls[newSelection].IsEnabled = true;
                rolls[newSelection].Visibility = Visibility.Visible;
                if (RollHolder != null)
                    RollHolder.Content = rolls[newSelection];
            }

            UpdateLayout();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSynthSelector();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double tempo;
            string? name = comboBox.SelectedValue.ToString();
            if (!Double.TryParse(this.bpm.Text, out tempo)) return;
            if (name == null || name.Equals("NoSynthSelected")) return;
            
            tempo /= 60 * 4; //how long a single button lasts.
            bool[,] grid = new bool[32, 108];
            foreach(int i in rolls[name].ColToSelNote.Keys)
            {
                if (rolls[name].ColToSelNote[i] != -1)
                    grid[i,rolls[name].ColToSelNote[i]] = true;
            }
            hSynth synth = (hSynth)Home.SynthColumns[name].hcStackPanel.Children[1];

            int previous = -1;
            int length = 0;
            synth.Notes.Clear();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                length++;
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    
                    if (grid[i,j] && j == previous) // continuing note
                    {
                        break;
                    }
                    else if (grid[i,j] && i == 0) // first note special case
                    {
                        previous = j;
                        length--;
                        break;
                    }
                    else if (grid[i,j]) // ended note, new note
                    {
                        if (previous != -1)
                            synth.Notes.Add(new Note((i - length) * tempo, (i) * tempo, previous));
                        previous = j;
                        length = 0;
                        break;
                    }
                    else if (j == grid.GetLength(1) - 1) // blank space, no notes
                    {
                        if (previous != -1)
                            synth.Notes.Add(new Note((i - length) * tempo, (i) * tempo, previous));
                        previous = -1;
                        length = 0;
                        break;
                    }
                }
            }

            
            //do not try this at home.

            Editor editor = MainWindowHolder.Window.editor as Editor;
            List<List<uint>> output = new();
            double baseFreq = 27.5 * Math.Pow(2, -9 / (double)12);
            foreach (Note note in synth.Notes) // compile every waveform
            {
                output.Add(editor.GenerateWaveWithNote(baseFreq * Math.Pow(2, note.Freq / (double)12), synth, (int)Math.Ceiling(note.EndTime - note.StartTime)));
            }
            List<uint> final_output = new();
            for (int i = 0; i < synth.Notes.Count; i++) //real death time - add empty space.
            {
                if (i == 0)
                {
                    int empty_ticks = (int)Math.Ceiling(synth.Notes[i].StartTime * (int)SampleRatesKHZ.OneNineTwo);
                    for (int t = 0; t < empty_ticks; t++)
                    {
                        final_output.Add(0);
                    }
                    final_output.AddRange(output[i]);
                }
                else
                {
                    int empty_ticks = (int)Math.Ceiling((synth.Notes[i].StartTime - synth.Notes[i - 1].EndTime) * (int)SampleRatesKHZ.OneNineTwo);
                    for (int t = 0; t < empty_ticks; t++)
                    {
                        final_output.Add(0);
                    }
                    final_output.AddRange(output[i]);
                }
            }

            

            InitData(final_output.Count);
            WriteData(final_output.ToArray(), final_output.Count);
            CreateWaveFile(final_output.Count, 1, (int)SampleRatesKHZ.OneNineTwo);
            DestructData();

        }
    }
}
