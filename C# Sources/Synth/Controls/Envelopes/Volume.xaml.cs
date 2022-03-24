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

namespace Synth.Controls.Envelopes
{
    /// <summary>
    /// Interaction logic for Volume.xaml
    /// </summary>
    public partial class Volume : Envelope
    {
        public Volume()
        {
            InitializeComponent();
            x = x == 0 ? Info.Time : x;
            //range is 0 to 2 semitones by default
            y = y == 0 ? Math.Pow(2, 2 / (double)12) : y;
            envControls.AddRange(new List<EnvelopeControls>
            {
                new EnvelopeControls(0, 1, this),
                
                new EnvelopeControls(x, 1, this)
            });
            this.TimeSlider.ValueChanged += SliderValChanged;
            this.ValueSlider.ValueChanged += SliderValChanged;
            Graph = this.GraphControl;
            DrawGraph(460, 135);
        }
        public void SliderValChanged(object sender, RoutedEventArgs e)
        {
            DrawGraph(460, 135);
        }
        public void GraphLoaded(object sender, RoutedEventArgs e)
        {
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
