using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Synth.Controls.Envelopes
{
    public class EnvelopeControls
    {
        #region vars
        double time;
        double val;
        //bool isCurvePoint;

        public double Time
        {
            get { return time; }
            set { time = value; }
        }
        public double Val
        {
            get { return val; }
            set { val = value; }
        }

        public RadioButton radioButton;
        Envelope parent;
        #endregion

        #region Constructors
        public EnvelopeControls(Envelope parent)
        {
            this.parent = parent;
            radioButton = new RadioButton
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Height = 25,
                Width = 25,
                GroupName = parent.name
        };
            radioButton.Click += this.RadioButton_Click;

            parent.AddToNodeContainer(radioButton);
            parent.AddXYSliderEvents(TimeSlider_ValueChanged, ValueSlider_ValueChanged);
            
        }

        public EnvelopeControls(double time, Envelope parent) : this(parent)
        {
            this.time = time;

        }
        public EnvelopeControls(double time, double val, Envelope parent) : this(time, parent)
        {
            this.val = val;
        }
        #endregion

        public Point GetPoint(double x, double y, double newx, double newy)
        {
            Point output = new Point(
                remap(time, 0, x, 0, newx) + 5 + 0.15 * newx, 
                newy - remap(val, 0, y, 0, newy) + 0.15 * newy);
            Canvas.SetLeft(radioButton, output.X - 10);
            Canvas.SetTop(radioButton, output.Y - 10);
            return output;
           
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            parent.EnableXYSlider(time, val);
        }
        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newval = e.NewValue;
            if (this.radioButton.IsChecked == true)
            {

                if ( newval > parent.y)
                {
                    ((Slider)sender).Value = parent.y;
                }
                else
                {
                        val = newval;
                }
            }
        }
        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double nextTime = parent.getNextTime(this);
            double prevTime = parent.getPrevTime(this);
            double newTime = e.NewValue;

            if (this.radioButton.IsChecked == true)
            {
                if (nextTime == -2)
                {
                    time = parent.x;
                    ((Slider)sender).Value = parent.x;
                }
                else if (prevTime == -1)
                {
                    time = 0;
                    ((Slider)sender).Value = 0;
                }
                else
                {
                    if (newTime < prevTime || newTime > nextTime)
                    {
                        ((Slider)sender).Value = e.OldValue;
                    }
                    else
                    {
                        time = newTime;
                    }
                }
            }
        }
        public void RemoveEvents()
        {
            parent.RemoveFromXYSliderEvents(TimeSlider_ValueChanged, ValueSlider_ValueChanged);
        }


        public static double remap(double s, double a1, double a2, double b1, double b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }
}
