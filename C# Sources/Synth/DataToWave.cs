using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public static class DataToWave
    {
        private const string DllFilePath = @"C:\Users\Liam\source\repos\Synth\Synth\SynthDLL.dll";


        public static bool WriteDataToFile(int samples, int channels, int samplerate, uint[] data)
        {
            InitData(samples);
            WriteData(data, samples);
            bool output = CreateWaveFile(samples, channels, samplerate);
            DestructData();
            return output;
        }

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void InitData(int sz);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void DestructData();
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void WriteData(uint[] data, int sz);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static bool CreateWaveFile(int numSamples, int numChannels, int sampleRate);

    }
}
