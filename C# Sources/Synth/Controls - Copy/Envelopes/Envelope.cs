using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Synth.Controls.Envelopes
{
    public class Envelope : UserControl
    {
        public string? name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public List<EnvelopeControls> envControls { get; set; }
        

        //I didn't make this a generic UserControl like an idiot, but whatever.
        public Path? Graph { get; set; }

        public Envelope()
        {
            envControls = new();
            
        }

        public virtual void UpdateGraph() { }
        public virtual void RemoveFromNodeContainer() { }
        public virtual void AddToNodeContainer(RadioButton node) { }
        public virtual void RemoveFromXYSliderEvents(RoutedPropertyChangedEventHandler<double> xAction, RoutedPropertyChangedEventHandler<double> yAction) { }
        public virtual void AddXYSliderEvents(RoutedPropertyChangedEventHandler<double> xAction, RoutedPropertyChangedEventHandler<double> yAction) { }
        public virtual void EnableXYSlider(double x, double y) { }

        public void DrawGraph(double BaseWidth, double BaseHeight)
        {
            PathGeometry graph = new();

            for (int i = 0; i < envControls.Count - 1; i++)
            {
                graph.AddGeometry(new LineGeometry(
                    envControls[i].GetPoint(
                        x,
                        y,
                        Info.RenderX *  0.7 * BaseWidth / (double)1920,
                        Info.RenderY * 0.7 * BaseHeight / (double)1080),
                    envControls[i + 1].GetPoint(
                        x,
                        y,
                        Info.RenderX * 0.7 * BaseWidth / (double)1920,
                        Info.RenderY * 0.7 * BaseHeight / (double)1080)
                    ));
            }
            if (Graph != null)
                Graph.Data = graph;
        }
        public void AddNode(object sender, RoutedEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < envControls.Count; i++)
            {
                if (envControls[i].radioButton.IsChecked == true)
                {
                    index = i;
                    break;
                }
            }
            if (index != envControls.Count - 1)
            {
                double newTime = (envControls[index].Time + envControls[index + 1].Time) / 2;
                double val = envControls[index].Val;
                EnvelopeControls newControl = new(newTime, val, this);
                envControls.Insert(index + 1, newControl);
            }
            else
            {
                double newTime = (envControls[index].Time + envControls[index - 1].Time) / 2;
                double val = envControls[index].Val;
                EnvelopeControls newControl = new(newTime, val, this);
                envControls.Insert(index + 1, newControl);
            }
            UpdateGraph();
        }
        public void RemoveNode(object sender, RoutedEventArgs e)
        {
            if (envControls.Count < 3) { return; }
            for (int i = 0; i < envControls.Count; i++)
            {
                if (envControls[i].radioButton.IsChecked == true)
                {
                    RemoveFromNodeContainer();
                    envControls[i].RemoveEvents();
                    envControls.Remove(envControls[i]);
                    if (i != 0)
                    {
                        envControls[i - 1].radioButton.IsChecked = true;
                    }
                    else
                    {
                        envControls[i + 1].radioButton.IsChecked = true;
                    }
                    UpdateGraph();
                    return;
                }
            }
        }
        public double getNextTime(EnvelopeControls env)
        {
            int i = envControls.IndexOf(env);
            if (i != envControls.Count - 1)
            {
                return envControls[i + 1].Time;
            }
            return -2;
        }
        public double getPrevTime(EnvelopeControls env)
        {
            int i = envControls.IndexOf(env);
            if (i != 0)
            {
                return envControls[i - 1].Time;
            }
            return -1;
        }
        public virtual void NewTime(double newTime)
        {
            x = newTime;

            if (envControls != null)
            {
                for (int i = 0; i < envControls.Count - 1; i++)
                {
                    EnvelopeControls env = envControls[i];
                    EnvelopeControls env2 = envControls[i + 1];
                    if (env.Time > x)
                    {
                        env.Time = x;
                    }
                    if (env2.Time > x)
                    {
                        env2.Time = x;
                    }
                    if (env2.Time < env.Time)
                    {
                        env2.Time = env.Time + 0.01;
                    }

                }
                DrawGraph(460, 135);
            }

        }
    }
}
