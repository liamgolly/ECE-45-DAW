using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Synth.Controls.Envelopes
{
    /// <summary>
    /// Interaction logic for Pitch.xaml
    /// </summary>
    public partial class Pitch : Envelope
    {
        //x = time
        //y = semitone range
        
        
        

        public Pitch() : base()
        {
            
            name = "Pitch";

            InitializeComponent();
            x = x == 0 ? Info.Time : x;
            //range is 0 to 2 semitones by default
            y = y == 0 ? Math.Pow(2, 2 / (double)12) : y;
            envControls.AddRange(new List<EnvelopeControls>
            {
                new EnvelopeControls(0, 0, this),
                new EnvelopeControls(x / 4, y, this),
                new EnvelopeControls(x / 2, 1, this),
                new EnvelopeControls(3 * x / 4, y / 2, this),
                new EnvelopeControls(x, 0, this)
            });
            this.TimeSlider.ValueChanged += SliderValChanged;
            this.ValueSlider.ValueChanged += SliderValChanged;
            Graph = this.GraphControl;
            DrawGraph(460, 135);



        }
        public Pitch(double time, double semitoneRange) : this()
        {
            this.x = time;
            this.y = semitoneRange;
        }
        public void GraphLoaded(object sender, RoutedEventArgs e)
        {
            DrawGraph(460, 135);

        }
        public void SliderValChanged(object sender, RoutedEventArgs e)
        {
            DrawGraph(460, 135);
        }
        public void NewSemitone(object sender, RoutedEventArgs e)
        {
            y = ((ComboBox)sender).SelectedIndex + 1;
            this.ValueSlider.Maximum = ((ComboBox)sender).SelectedIndex + 1;
            if (envControls != null)
                DrawGraph(460, 135);
        }
        
        public void PreviewTime(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public override void UpdateGraph()
        {
            base.UpdateGraph();
            DrawGraph(460, 135);
        }
        public override void AddToNodeContainer(RadioButton node)
        {
            base.AddToNodeContainer(node);
            PathContainer.Children.Insert(0, node);
        }
        public override void AddXYSliderEvents(RoutedPropertyChangedEventHandler<double> xAction, RoutedPropertyChangedEventHandler<double> yAction)
        {
            base.AddXYSliderEvents(xAction, yAction);
            ValueSlider.ValueChanged += yAction;
            TimeSlider.ValueChanged += xAction;
        }
        public override void EnableXYSlider(double x, double y)
        {
            base.EnableXYSlider(x, y);
            ValueSlider.IsEnabled = true;
            TimeSlider.IsEnabled = true;
            ValueText.IsEnabled = true;
            TimeText.IsEnabled = true;

            ValueSlider.Value = y;
            TimeSlider.Value = x;
        }
        public override void RemoveFromNodeContainer()
        {
            base.RemoveFromNodeContainer();
            envControls.ForEach(node =>
            {
                if (node.radioButton.IsChecked != null && (bool)node.radioButton.IsChecked)
                {
                    PathContainer.Children.Remove(node.radioButton);
                }
            });
        }
        public override void RemoveFromXYSliderEvents(RoutedPropertyChangedEventHandler<double> xAction, RoutedPropertyChangedEventHandler<double> yAction)
        {
            base.RemoveFromXYSliderEvents(xAction, yAction);
            TimeSlider.ValueChanged -= xAction;
            ValueSlider.ValueChanged -= yAction;
        }
        public override void NewTime(double newTime)
        {
            base.NewTime(newTime);
            this.TimeSlider.Maximum = newTime;
        }

    }
}
