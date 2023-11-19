// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Id3Lib
{
	/// <summary>
	/// Handle the loading and saving of ID3 tags.
	/// </summary>
	/// <remarks>
    /// The <c>FrameManager</c> class manages the converion of a ID3v2 tag from binary form 
    /// to a <see cref="FrameManager"/> that can be manipulated and saved later again to
	/// a binary form.
	/// </remarks>
	public static class FrameManager
	{
		#region Methods
		/// <summary>
		/// Load the ID3v2 frames to a binary stream
		/// </summary>
		/// <param name="src">Bynary stream holding the ID3 Tag</param>
		/// <returns>Model keeping the ID3 Tag structure</returns>
		public static FrameModel Deserialize(Stream src)
		{
			FrameModel tagModel = new FrameModel();
			tagModel.Header.Deserialize(src); // load the ID3v2 header
			if(tagModel.Header.Version != 3 & tagModel.Header.Version != 4)
			{
				throw new NotImplementedException("ID3v2 Version " + tagModel.Header.Version + " is not supported.");
			}
			
			int id3TagSize = tagModel.Header.TagSize;
			Stream stream = src; 

			if(tagModel.Header.Unsync == true)
			{
				MemoryStream memory = new MemoryStream();
				id3TagSize -= Sync.Unsafe(stream,memory,id3TagSize);
				stream = memory; // This is now the stream
				if(id3TagSize<=0)
				{
					throw new InvalidTagException("Data is missing after the header.");
				}
			}
			int rawSize;
			// load the extended header
			if(tagModel.Header.ExtendedHeader == true)
			{
				tagModel.ExtendedHeader.Header = tagModel.Header;
				tagModel.ExtendedHeader.Deserialize(stream);
				rawSize = id3TagSize - tagModel.ExtendedHeader.Size;
				if(id3TagSize<=0)
				{
					throw new InvalidTagException("Data is missing after the extended header.");
				}
			}
			else
			{
				rawSize = id3TagSize;
			}
			
			// Read the frames
			if(rawSize <= 0)
				throw new InvalidTagException("No frames are present in the Tag, there must be at least one present.");
			
			BinaryReader reader = new BinaryReader(stream);
			// Load the tag frames
			ushort flags;
			int index = 0, frameSize;
			FrameHelper frameHelper = new FrameHelper(tagModel.Header);
			 // repeat while there is at least one complete frame avaliable, 10 is the minimum size of a valid frame
			while(rawSize > index + 10)
			{
				byte[] tag = new byte[4];
				reader.Read(tag,0,4);
				if(tag[0] == 0)
				{
					tagModel.Header.Padding = true;
					int padding = SeekEOT(src) + 4;
					int free = rawSize - index;

					break; // We reached the padding area
				}
				index+=4; // read 4 bytes
				//TODO: Validate key valid ranges
				frameSize = Swap.Int32(reader.ReadInt32());
				index+=4; // read 4 bytes
				// ID3v4 now has syncsafe sizes
				if(tagModel.Header.Version == 4)
				{
					Sync.Unsafe(frameSize);
				}
				// The size of the frame can't be larger than the avaliable space
				if(frameSize > rawSize - index)
				{
					throw new InvalidFrameException("A frame is corrupt can't, it can't be larger than the avaliable space remaining.");
				}
				flags = Swap.UInt16(reader.ReadUInt16());
				index+=2; // read 2 bytes
				byte[] frameData = new byte[frameSize];
				reader.Read(frameData,0,frameSize);
				index+=frameSize; // read more bytes
				tagModel.Frames.Add(frameHelper.Build(UTF8Encoding.UTF8.GetString(tag,0,4),flags,frameData));
			}
			return tagModel;
		}

		static private int SeekEOT(Stream stream)
		{
			int index = 0;
			while(stream.ReadByte() == 0)
			{
				index++;
			}
			return index;
		}

		/// <summary>
		/// Save the ID3v2 frames to a binary stream
		/// </summary>
		/// <param name="tagModel">Model keeping the ID3 Tag structure</param>
		/// <param name="stream">Stream keeping the ID3 Tag</param>
		public static void Serialize(FrameModel tagModel,Stream stream)
		{
			if(tagModel.Frames.Count <= 0)
			{
				throw new InvalidTagException("Can't serialize a ID3v2 tag with out any frames, there must be at least one present.");
			}

			MemoryStream memory = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memory);

			FrameHelper frameHelper = new FrameHelper(tagModel.Header);
			// Write the frames in binary format
			foreach(FrameBase frameBase in tagModel.Frames)
			{
				//TODO: Do validations on tag name correctness
				byte[] tag = new byte[4];
				UTF8Encoding.UTF8.GetBytes(frameBase.Tag,0,4,tag,0);
				writer.Write(tag); // Write the 4 byte text tag
				byte[] frame = frameHelper.Make(frameBase);
				int frameSize = frame.Length;
				if(tagModel.Header.Version == 4)
				{
					Sync.Safe(frameSize);
				}
				writer.Write(Swap.Int32(frameSize));
				writer.Write(Swap.UInt16(frameBase.Flags));
				writer.Write(frame);
			}
			
			int id3TagSize = (int)memory.Position;

			// Skip the header 10 bytes for now, we will come back and write the Header
			// with the correct size once have the tag size + padding
			stream.Seek(10,SeekOrigin.Begin);
			
			// TODO: Add extedned header handling
			if(tagModel.Header.Unsync == true)
			{
				id3TagSize += Sync.Safe(memory,stream,id3TagSize);
			}
			else
			{
				memory.WriteTo(stream);
			}
			// Write now the header
			long position = stream.Position;
			stream.Seek(0,SeekOrigin.Begin);
			tagModel.Header.TagSize = id3TagSize;
			tagModel.Header.Serialize(stream);
			stream.Position = position;

		}
		#endregion
	}
}
