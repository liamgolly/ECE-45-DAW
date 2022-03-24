using Synth.Controls.Envelopes;
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

namespace Synth.Controls
{
    /// <summary>
    /// Interaction logic for hSynth.xaml
    /// </summary>
    public partial class hSynth : UserControl
    {
        public List<uint> yVals; //values are stored at 192kHz
        public List<double> pitchs;
        public List<Note> Notes;

        private int time;
        public int Time { get { return time; } set { time = value; } }

        public hSynth()
        {
            InitializeComponent();
            yVals = new();
            pitchs = new();
            time = 4;
            Notes = new() { };
            GenerateDrawnSimpleWave("saw");
            //GenerateSawWave();
            //Update();
        }

        public void SimpleUpdate()
        {

        }

        public void ScaryUpdate()
        {
            update_graph();
        }

        private void update_graph()
        {
            if (yVals.Count < 2) { return; }
            int length = yVals.Count; //get length of the waveform

            //display the list
            PathGeometry graph = new PathGeometry();
            double scale_factor = 300 / (double)length;
            for (int i = 0; i < length - 1; i++)
            {
                graph.AddGeometry(HomeColumn.generateLineGeometry(
                    i,
                    yVals[i],
                    yVals[(i)+1],
                    scale_factor,
                    yVals.Max(),
                    yVals.Min(),
                    100));
            }

            CurrentWaveform.Data = graph;
        }       

        public void GenerateSinWave()
        {

        }
        public void GenerateSquareWave()
        {

        }
        public void GenerateSawWave(double basePitch, List<double>? pitchArray, List<double>? vibratoArray)
        {
            
            int samples = this.Time * (int)SampleRatesKHZ.OneNineTwo;
            uint v = 0;
            double n = basePitch * uint.MaxValue / (int)SampleRatesKHZ.OneNineTwo; // MULTIPLY THIS NUMBER BY PITCH ENVELOPE!!!!!! CURRENT NOTE IS C4 = 440 / 2^(9/12) HZ
                                                                                   // TO GET LOWER SAMPLE RATES, USE i += 192000/sr WHEN INDEXING yVals
            pitchs.Clear();
            yVals.Clear();
            for (int i = 0; i < samples; ++i)
            {
                double p = 1;
                double vi = 1;
                double value;

                if (pitchArray != null && pitchArray.Count > i)
                    p = pitchArray[i];
                if (vibratoArray != null && vibratoArray.Count > i)
                    vi = vibratoArray[i];

                value = n * p * vi;

                v += (uint)(value);
                yVals.Add(v); //im so sorry you poor poor list.
                pitchs.Add(value / (double)uint.MaxValue * (double)SampleRatesKHZ.OneNineTwo);
            }
            
        }
        public void AliasNotes(Note note1, Note note2)
        {

        }
        
        public void GenerateDrawnSimpleWave(string type)
        {
            double width = 300;
            double height = 100;
            PathGeometry graph = new PathGeometry();
            switch (type)
            {
                case "saw":
                    Point a = new Point(10, 10);

                    Point b = new Point(10 + width / 4, height);
                    Point c = new Point(10 + width / 4, 10);

                    Point d = new Point(10 + 2 * width / 4, height);
                    Point e = new Point(10 + 2 * width / 4, 10);

                    Point f = new Point(10 + 3 * width / 4, height);
                    Point g = new Point(10 + 3 * width / 4, 10);

                    Point h = new Point(10 + 4 * width / 4, height);

                    LineGeometry i = new LineGeometry(a, b);
                    LineGeometry j = new LineGeometry(b, c);
                    LineGeometry k = new LineGeometry(c, d);
                    LineGeometry l = new LineGeometry(d, e);
                    LineGeometry m = new LineGeometry(e, f);
                    LineGeometry n = new LineGeometry(f, g);
                    LineGeometry o = new LineGeometry(g, h);
                   
                    graph.AddGeometry(i);
                    graph.AddGeometry(j);
                    graph.AddGeometry(k);
                    graph.AddGeometry(l);
                    graph.AddGeometry(m);
                    graph.AddGeometry(n);
                    graph.AddGeometry(o);


                    break;
                case "square":

                    break;
                case "sin":

                    break;              
            }

            CurrentWaveform.Data = graph;
        }
      
    }
    public class Note
    {
        public Note(double time1, double time2, int freq)
        {
            StartTime = time1;
            EndTime = time2;
            Freq = freq;
        }

        public double StartTime;
        public double EndTime;
        public int Freq;
    }
}
