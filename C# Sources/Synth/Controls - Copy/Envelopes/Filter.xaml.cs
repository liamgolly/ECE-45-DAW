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
    public partial class Filter : Envelope
    {

        public State state = State.None;
        private bool bruh = false;
        private double low_freq, low_width, low_stable, low_spike, high_freq, high_width, high_stable, high_spike, c1,c2,c3,c4, m = 0;

        public Filter()
        {
            InitializeComponent();                     
            this.TimeSlider.ValueChanged += SliderValChanged;
            this.ValueSlider.ValueChanged += SliderValChanged;
            this.CutoffSlider.ValueChanged += SliderValChanged;
            Graph = this.GraphControl;
            DrawFilterGraph();
        }
        #region stuff
        public void SliderValChanged(object sender, RoutedEventArgs e)
        {
            DrawFilterGraph();
        }
        public void GraphLoaded(object sender, RoutedEventArgs e)
        {
            DrawFilterGraph();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            bruh = true;
            TimeSlider.Value = low_freq;
            TimeText.Text = TimeSlider.Value.ToString();
            ValueSlider.Value = low_stable;
            ValueText.Text = ValueSlider.Value.ToString();
            CutoffSlider.Value = low_width;
            CutoffText.Text = CutoffSlider.Value.ToString();
            bruh = false;
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            bruh = true;
            TimeSlider.Value = high_freq;
            TimeText.Text= TimeSlider.Value.ToString();
            ValueSlider.Value = high_stable;
            ValueText.Text= ValueSlider.Value.ToString();
            CutoffSlider.Value = high_width;
            CutoffText.Text= CutoffSlider.Value.ToString();
            bruh = false;
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
            CutoffSlider.IsEnabled = true;
            CutoffText.IsEnabled = true;

            //ValueSlider.Value = y;
            //TimeSlider.Value = x;
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
            
        }
        #endregion

        private void highPass_Click(object sender, RoutedEventArgs rea)
        {
            if (state == State.High)
            {
                state = State.Band;
                highPass.IsEnabled = false;
                lowPass.IsEnabled = false;
                Left.IsEnabled = true;
                low_stable = 0;
                low_width = 4;
                low_freq = 2000;
                high_stable = 0;
                high_width = 4;
                high_freq = 6000;
            }
            else if (state == State.Low)
            {
                state = State.Notch;
                highPass.IsEnabled = false;
                lowPass.IsEnabled = false;
                Right.IsEnabled = true;
                low_stable = 0;
                low_width = 4;
                low_freq = 2000;
                high_stable = 0;
                high_width = 4;
                high_freq = 6000;
            }
            else
            {
                state = State.High;
                ((Button)sender).Content = "Low Pass";
                Right.IsEnabled = true;
                Right.IsChecked = true;

                high_stable = 0;
                high_width = 4;
                high_freq = 5000;
                Right_Click(Right, rea);
                EnableXYSlider(0, 0);
            }
            DrawFilterGraph();
        }

        private void lowPass_Click(object sender, RoutedEventArgs rea)
        {
            if (state == State.High)
            {
                state = State.Notch;
                highPass.IsEnabled = false;
                lowPass.IsEnabled = false;
                Left.IsEnabled = true;
                low_stable = 0;
                low_width = 4;
                low_freq = 2000;
                high_stable = 0;
                high_width = 4;
                high_freq = 6000;
            }
            else if (state == State.Low)
            {
                state = State.Band;
                highPass.IsEnabled = false;
                lowPass.IsEnabled = false;
                Right.IsEnabled = true;
                low_stable = 0;
                low_width = 4;
                low_freq = 2000;
                high_stable = 0;
                high_width = 4;
                high_freq = 6000;
            }
            else
            {
                state = State.Low;
                ((Button)sender).Content = "High Pass";
                Left.IsEnabled = true;
                Left.IsChecked = true;
                low_stable = 0;
                low_width = 4;
                low_freq = 5000;
                Left_Click(Left, rea);
                EnableXYSlider(0, 0);
            }
            DrawFilterGraph();
        }

        private void removePass(object sender, RoutedEventArgs e)
        {
            switch(state)
            {
                case State.Low:
                    state = State.None;
                    Left.IsEnabled = false;
                    Left.IsChecked = false;
                    lowPass.Content = "Low Pass";
                    break;
                case State.High:
                    state = State.None;
                    Right.IsEnabled = false;
                    Right.IsChecked = false;
                    highPass.Content = "High Pass";
                    break;
                case State.Band:
                    if ((bool)Right.IsChecked) //no it cant lmao
                    {
                        Right.IsChecked = false;
                        Right.IsEnabled = false;
                        Left.IsChecked = true;
                        lowPass.Content = "Low Pass";
                        highPass.Content = "High Pass";
                        lowPass.IsEnabled = true;
                        highPass.IsEnabled = true;
                        state = State.Low;
                    }
                    else
                    {
                        Right.IsChecked = true;
                        Left.IsChecked = false;
                        Left.IsEnabled = false;
                        lowPass.Content = "Low Pass";
                        highPass.Content = "High Pass";
                        lowPass.IsEnabled = true;
                        highPass.IsEnabled = true;
                        state = State.High;
                    }
                    break;
                case State.Notch:
                    if ((bool)Right.IsChecked) //it isn't null compiler wake up
                    {

                        Left.IsChecked = false;
                        Left.IsEnabled = false;
                        lowPass.Content = "Low Pass";
                        highPass.Content = "High Pass";
                        lowPass.IsEnabled = true;
                        highPass.IsEnabled = true;
                        state = State.High;
                    }
                    else
                    {
                        Right.IsChecked = false;
                        Right.IsEnabled = false;
                        state = State.Low;
                        lowPass.Content = "Low Pass";
                        highPass.Content = "High Pass";
                        lowPass.IsEnabled = true;
                        highPass.IsEnabled = true;
                    }
                    break;

                default:
                    break;
                    
            }
            DrawFilterGraph();
        }
        private void DrawFilterGraph()
        {
            updatevars();
            List<Point> points = new();
            double oldx = 8000;
            double oldy = 8;
            double newx = Info.RenderX * 0.7 * 660 / (double)1920;
            double newy = Info.RenderY * 0.7 * 155 / (double)1080;
            for (double i = 0; i < 8000; i++)
            {
                double? c = compute(i);
                if (c != null)
                    points.Add(new Point(i, (double)c));
                
            }
            if (points.Count == 0) { return; }
            Point[] p = points.ToArray();
            points.Clear();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = EnvelopeControls.remap(p[i].X, 0, 8000, 0, newx);
                p[i].Y = newy - EnvelopeControls.remap(Math.Max(p[i].Y, -8), -8, 8, 0, newy);
            }

            PathSegmentCollection pathSegments = new PathSegmentCollection();
            for (int i = 0; i < p.Length; i++)
            {
                pathSegments.Add(new LineSegment(p[i], true));
            }

            GraphControl.Data = new PathGeometry() 
            { 
                Figures = new PathFigureCollection() 
                {
                    new PathFigure() 
                    { 
                        StartPoint = p[0], 
                        Segments = pathSegments 
                    } 
                } 
            };
            
        }
        
        public void updatevars()
        {
            if (!bruh)
            {
                if ((bool)Left.IsChecked)
                {
                    low_freq = TimeSlider.Value < high_freq ? TimeSlider.Value : low_freq;
                    TimeSlider.Value = TimeSlider.Value < high_freq ? TimeSlider.Value : low_freq;
                    low_stable = ValueSlider.Value;
                    low_width = CutoffSlider.Value;
                }
                else if ((bool)Right.IsChecked)
                {
                    high_freq = TimeSlider.Value > low_freq ? TimeSlider.Value : high_freq;
                    TimeSlider.Value = TimeSlider.Value > low_freq ? TimeSlider.Value : high_freq;
                    high_stable = ValueSlider.Value;
                    high_width = CutoffSlider.Value;
                }
            }
            
            m = (high_stable - low_stable) / (high_freq - low_freq);
            low_spike = Math.Pow(80000 / Math.Pow(10, low_width), 1 / (double)3);
            high_spike = Math.Pow(80000 / Math.Pow(10, high_width), 1 / (double)3);
            c1 = Math.Pow(2, low_width - 1) * Math.Pow(5, low_width) * (low_freq * Math.Pow(2, 1 - low_width) * Math.Pow(5, -1 * low_width) - Math.Sqrt(low_spike * Math.Pow(2, 2 - low_width) * Math.Pow(5, -1 * low_width)));
            c4 = Math.Pow(2, high_width - 1) * Math.Pow(5, high_width) * (high_freq * Math.Pow(2, 1 - high_width) * Math.Pow(5, -1 * high_width) + Math.Sqrt(high_spike * Math.Pow(2, 2 - high_width) * Math.Pow(5, -1 * high_width)));
            //don't question c2 and c3, it just works.
            c2 = -1 * Math.Pow(2, low_width - 1) * Math.Pow(5, low_width) * (-1 * Math.Sqrt(Math.Pow(2, 2 - low_width) * Math.Pow(5, -1 * low_width) * (-1 * Math.Pow(10, -1 * low_width) * Math.Pow(low_freq, 2) + (low_stable * high_freq) / (low_freq - high_freq) - (low_freq * high_stable) / (low_freq - high_freq) + low_stable + low_spike) + Math.Pow(Math.Pow(2, 1 - low_width) * Math.Pow(5, -1 * low_width) * low_freq - low_stable / (low_freq - high_freq) + high_stable / (low_freq - high_freq), 2)) - Math.Pow(2, 1 - low_width) * Math.Pow(5, -1 * low_width) * low_freq - low_stable / (low_freq - high_freq) - high_stable / (low_freq - high_freq));
            c3 = -1 * Math.Pow(2, high_width - 1) * Math.Pow(5, high_width) * (1 * Math.Sqrt(Math.Pow(2, 2 - high_width) * Math.Pow(5, -1 * high_width) * (-1 * Math.Pow(10, -1 * high_width) * Math.Pow(high_freq, 2) + (low_stable * high_freq) / (low_freq - high_freq) - (low_freq * high_stable) / (low_freq - high_freq) + high_stable + high_spike) + Math.Pow(Math.Pow(2, 1 - high_width) * Math.Pow(5, -1 * high_width) * high_freq - low_stable / (low_freq - high_freq) + high_stable / (low_freq - high_freq), 2)) - Math.Pow(2, 1 - high_width) * Math.Pow(5, -1 * high_width) * high_freq - low_stable / (low_freq - high_freq) - high_stable / (low_freq - high_freq));
            
        }

        public double? compute(double x)
        {
            if (state == State.None) { return null; }
            if (state == State.High || state == State.Low) // Im using a chain of if statements rather than a case statment so I can collapse code in visual studio.
            {
                if (state == State.High)
                    return x > c4 ? high_stable : (-1 * Math.Pow(10, -1 * high_width) * Math.Pow(x - high_freq, 2) + high_stable + high_spike);
                else
                    return x < c1 ? low_stable : (-1 * Math.Pow(10, -1 * low_width) * Math.Pow( x - low_freq, 2) + low_stable + low_spike);
                
            }
            else if (state == State.Band)
            {              
                 if (x < c2)
                {
                    return -1 * Math.Pow(10, -1 * low_width) * Math.Pow(x - low_freq, 2) + low_stable + low_spike;
                }
                else if (x < c3)
                {
                    
                    return m * x + (high_stable - m * high_freq);
                }
                else
                {
                    return -1 * Math.Pow(10, -1 * high_width) * Math.Pow(x - high_freq, 2) + high_stable + high_spike;
                }                
            }
            else
            {
                if (x < c1)
                {
                    return low_stable;
                }
                else if (x < (c2 + c3) / 2)
                {
                    return -1 * Math.Pow(10, -1 * low_width) * Math.Pow(x - low_freq, 2) + low_stable + low_spike;
                }
                else if (x < c4)
                {
                    return -1 * Math.Pow(10, -1 * high_width) * Math.Pow(x - high_freq, 2) + high_stable + high_spike;
                }
                else return high_stable;
            }
        }
    }

    public enum State
    {
        High,
        Low,
        Band,
        Notch,
        None
    }
}
