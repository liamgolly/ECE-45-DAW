// SynthDLL.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "SynthDLL.h"
#include <cstdio>



unsigned long * DataArray{};
bool dataExists{ false };

struct WaveHeader {

	//main chunk
	unsigned char mainID[4];
	unsigned long mainSize;
	unsigned char mainFormat[4];

	//format chunk
	unsigned char formatID[4];
	long formatSize;
	short formatTag;
	unsigned short channels;
	unsigned long samplesPerSecond;
	unsigned long bytesPerSecond;
	unsigned short blockAlign;
	unsigned short bitsPerSample;

	//data chunk
	unsigned char dataID[4];
	long dataSize;
	//unsigned char waveformData[] - declared separately from the header struct, in DataArray above.

};


/// <summary>
/// Creates an audio file using the given parameters.
/// Heavily inspired by:
/// https://blog.demofox.org/diy-synthesizer/
///  - a blog that details the creation of a wav file given points on a graph.
///  - a decent chunk of the code related to actually creating a .wav file come from chapter 1.
/// http://www.piclist.com/techref/io/serial/midi/wave.html
///  - a comprehensive deep dive into the .wav spec, very useful for understanding how to actually work with the data array.
/// </summary>
/// <param name="fileName"> The name of the file to create. </param>
/// <param name="data"> A pointer to the data needed to create the file. </param>
/// <param name="dataSize"> the size of the data being passed in. </param>
/// <param name="numChannels"> The number of audio channels in the data. </param>
/// <param name="sampleRate"> The sample rate of the data. </param>
/// <param name="bitsPerSample"> The bits per sample of the data. </param>
/// <returns> Returns if the creation of the file is successful. </returns>
bool WriteWaveFile(
	const char* fileName, 
	void* data,
	unsigned long dataSize, 
	unsigned short numChannels, 
	unsigned long sampleRate, 
	unsigned short bitsPerSample) {

	FILE* file;
	fopen_s(&file, fileName, "w+b");
	if (!file) { return false; }

	WaveHeader header;

	//fill out the header bit by bit.
	memcpy(header.mainID, "RIFF", 4);
	header.mainSize = (dataSize + 36);
	memcpy(header.mainFormat, "WAVE", 4);

	memcpy(header.formatID, "fmt ", 4);
	header.formatSize = 16;
	header.formatTag = 1;
	header.channels = numChannels;
	header.samplesPerSecond = sampleRate;
	header.bytesPerSecond = sampleRate * numChannels * (bitsPerSample / 8);
	header.blockAlign = numChannels * (bitsPerSample / 8);
	header.bitsPerSample = bitsPerSample;

	memcpy(header.dataID, "data", 4);
	header.dataSize = dataSize;

	fwrite(&header, sizeof(header), 1, file);
	fwrite(data, dataSize, 1, file);

	fclose(file);
	return true;
}


/// <summary>
/// Initilizes the Data array to a given size.
/// </summary>
/// <param name="sz"> The size of the data array.</param>
/// <returns></returns>
void InitData(int sz) {
	dataExists = true;
	DataArray = new unsigned long[sz];
}


/// <summary>
/// Writes to the Data array, must be used before using CreateWaveFile()
/// If InitData() has not been called, this function will call it for you.
/// </summary>
/// <param name="data"> The data, as a long array, to write. </param>
/// <param name="sz"> The size of the data initilized in InitData</param>
/// <returns></returns>
void WriteData(unsigned long * data, int sz) {
	if (!dataExists) {
		InitData(sz);
	}
	for (int i = 0; i < sz; i++) {
		DataArray[i] = data[i];
	}
}


/// <summary>
/// Writes the data from WriteData() to a .wav file named 'file.wav'.
/// </summary>
/// <param name="sz"> The size of the data initilized. </param>
/// <param name="numSamples"> The number of samples. </param>
/// <param name="numChannels"> The number of channels. </param>
/// <param name="sampleRate"> Samples / Second. </param>
/// <returns> True if the file is created, false if it isn't. </returns>
bool CreateWaveFile(int numSamples, int numChannels, int sampleRate) {
	if (dataExists == false) { return false; }
	return WriteWaveFile("file.wav", DataArray, numSamples * sizeof(DataArray[0]), numChannels, sampleRate, sizeof(DataArray[0]) * 8);
}


/// <summary>
/// Deallocates DataArray, MUST BE USED TO STOP MEMORY LEAK.
/// </summary>
/// <returns></returns>
void DestructData() {
	delete[] DataArray;
	dataExists = false;
}