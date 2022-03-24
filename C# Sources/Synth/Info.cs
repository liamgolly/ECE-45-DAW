using Synth.Controls.Envelopes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public static class Info
    {
        public static int SampleRate;
        public static int Time;
        public static int Channels;
        public static double RenderX;
        public static double RenderY;
        public static double C4 = 440 / Math.Pow(2, 9 / (double)12);
        public static Dictionary<string, Pitch> PitchControlDict = new();
        public static Dictionary<string, Volume> VolumeControlDict = new();
        public static Dictionary<string, Filter> FilterControlDict = new();
        public static Dictionary<string, double[]> VibratoDict = new();
        public static Dictionary<string, double> TremoloDict = new();
        public static Dictionary<string, double> DetuneDict = new();
        public static Dictionary<string, double> TimeDict = new();

    }
    public class MainWindowHolder
    {
        internal static MainWindow window = null;

        public static void SetWindow(MainWindow wnd)
        {
            window = wnd;
        }
        public static MainWindow Window { get { return window; } }
    }

    public enum SampleRatesKHZ
    {
        Six = 6000,
        Twelve = 12000,
        TwentyFour = 24000,
        FourtyEight = 48000,
        NinetySix = 96000,
        OneNineTwo = 192000
    }
}
