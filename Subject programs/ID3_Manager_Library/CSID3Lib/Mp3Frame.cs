using System;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Incomplete experimental test to read mp3 Frames.
	/// </summary>

	public class Mp3Frame
	{
		#region Fields
		// bit rate
		ushort[] V1L1   = new ushort[] {0,32,64,96,128,160,192,224,256,288,320,352,384,416,448};
		ushort[] V1L2   = new ushort[] {0,32,48,56,64,80,96,112,128,160,192,224,256,320,384};
		ushort[] V1L3   = new ushort[] {0,32,40,48,56,64,80,96,112,128,160,192,224,256,320};
		ushort[] V2L1   = new ushort[] {0,32,48,56,64,80,96,112,128,144,160,176,192,224,256};
		ushort[] V2L2L3 = new ushort[] {0,8,16,24,32,40,48,56,64,80,96,112,128,144,160};
		// Frequency sampling speed
		ushort[] MPEG1  = new ushort[] {44100,48000,32000};
		ushort[] MPEG2  = new ushort[] {22050,24000,16000};
		ushort[] MPEG25 = new ushort[] {11025,12000,8000};
		// Channel Mode
		string[] MPEGChanelMode = new string[] {"Stereo","Joint Stereo","Dual channel (2 mono channels)","Single channel (Mono)"};
		// Layer
		string[] MPEGLayer = new string[] {"Reserved","Layer III","Layer II","Layer I"};
		// Emphasis
		string[] MPEGEmphasis = new string[] {"None","50/15 ms","Reserved","CCIT J.17"};
		#endregion

		#region Methods

		/// <summary>
		/// Find the first occurrance of an mp3 header.
		/// </summary>
		/// <param name="stream">The stream to perform the search.</param>
		/// <returns>First mp3 header position in the stream.</returns>
		public int Seek(Stream stream)
		{
			byte last = 0;
			byte data = 0;
		    int index = 0;
			bool bFound = false;
			while(!bFound & (stream.Position <= stream.Length))
			{
				last = data;
				data = (byte)stream.ReadByte();
				if((last == 0xff) && ((data & 0xf0) == 0xf0))
				{
					bFound = true;
					stream.Position = stream.Position-2;
				}
				index++;
			}
			return index;
		}

		/// <summary>
		/// Parse mp3 frame
		/// </summary>
		/// <param name="frame">Binary MP3 frame</param>
		public void ReadFrame(byte[] frame)
		{
			//Frame sync (all 12 bits set)
			if(!(frame[0] == 0xff && (frame[1] & 0xf0) == 0xf0))
				throw new ApplicationException("Invalid MP3 frame header");
			
			//MPEG Audio version ID
			byte mpegVersion = (byte)((frame[1] & 0x08)>>3);
			//Layer description
			byte layer = (byte)((frame[1] & 0x06)>>1);
			//Protection bit
			bool bProtection = (frame[1] & 0x01)>0;
			//Bitrate index
			byte bitRate = (byte)((frame[2] & 0xf0)>>4);
			//Sampling rate frequency index
			byte sampleFreq = (byte)((frame[2] & 0x0C)>>2);
			//Padding bit
			bool bPadding = (frame[2] & 0x02)>0;
			//Private bit.
			bool bPrivate = (frame[2] & 0x01)>0;
			//Chanel Mode
			byte chanelMode = (byte)((frame[3] & 0xC0)>>6);
			// Copyright
			bool bCopyright = (frame[3] & 0x08)>0;
			// Original
			bool bOriginal = (frame[3] & 0x04)>0;
			// Emphasis
			byte emphasis = (byte)((frame[3] & 0x03));

			switch(mpegVersion)
			{
				case 0:
				{
					Console.WriteLine("MPEG Version 2 (ISO/IEC 13818-3)");
					switch(layer)
					{
						case 1: //Layer III
						{
							Console.WriteLine(V2L1[bitRate]+ " kbps");
							break;
						}
						case 2: // Layer II and I
						case 3:
						{
							
							Console.WriteLine(V2L2L3[bitRate]+ " kbps");
							break;
						}
					}
					Console.WriteLine(MPEG2[sampleFreq] + " Hz");
					break;
				}
				case 1:
				{
					Console.WriteLine("MPEG Version 1 (ISO/IEC 11172-3)");
					switch(layer)
					{
						case 1: //Layer III
						{
							Console.WriteLine(V1L3[bitRate]+ " kbps");
							break;
						}
						case 2: //Layer II
						{
							Console.WriteLine(V1L2[bitRate]+ " kbps");
							break;
						}
						case 3: // Layer I
						{
							Console.WriteLine(V1L1[bitRate]+ " kbps");
							break;
						}
					}
					Console.WriteLine(MPEG1[sampleFreq] + " Hz");
					break;
				}
			}
			Console.WriteLine("Layer: "+MPEGLayer[layer]);
			Console.WriteLine("Chanel Mode: "+MPEGChanelMode[chanelMode]);
			if(bOriginal == true)
			{
				Console.WriteLine("Original media");
			}
			else
			{
				Console.WriteLine("Copy of original media");
			}
			if(bCopyright == true)
			{
				Console.WriteLine("Audio is copyrighted");
			}
			else
			{
				Console.WriteLine("Audio is not copyrighted");
			}
			Console.WriteLine("Emphasis: "+MPEGEmphasis[emphasis]);
		}
		#endregion
	}
}
