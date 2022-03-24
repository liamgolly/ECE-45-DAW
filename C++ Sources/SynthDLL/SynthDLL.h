// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the SYNTHDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// SYNTHDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef SYNTHDLL_EXPORTS
#define SYNTHDLL_API __declspec(dllexport)
#else
#define SYNTHDLL_API __declspec(dllimport)
#endif



extern "C" SYNTHDLL_API void InitData(int sz);
extern "C" SYNTHDLL_API void DestructData();
extern "C" SYNTHDLL_API void WriteData(unsigned long* data, int sz);
extern "C" SYNTHDLL_API bool CreateWaveFile(int numSamples, int numChannels, int sampleRate);

