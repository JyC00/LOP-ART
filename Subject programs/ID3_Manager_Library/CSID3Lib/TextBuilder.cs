// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
    //#region Global Fields
    ///// <summary>
    ///// Type of text used in frame
    ///// </summary>
    //public enum TextCode:byte
    //{
    //    /// <summary>
    //    /// ASCII(ISO-8859-1)
    //    /// </summary>
    //    ASCII = 0x00,
    //    /// <summary>
    //    /// Unicode with BOM
    //    /// </summary>
    //    UTF16 = 0x01,
    //    /// <summary>
    //    /// BigEndian Unicode without BOM
    //    /// </summary>
    //    UTF16BE = 0x02,
    //    /// <summary>
    //    /// Encoded Unicode
    //    /// </summary>
    //    UTF8 = 0x03
    //};
    //#endregion

	/// <summary>
	/// Manages binary to text and viceversa format conversions.
	/// </summary>
	public class StreamTextBuilder
	{
		#region Methods
        public StreamTextBuilder() { }
        public void StreamTextBuilderInitialize() { }
		public string ReadText(byte[]frame,ref int index,TextCode code)
		{
			switch(code)
			{
				case TextCode.ASCII:
				{
					return ReadASCII(frame,ref index);
				}
				case TextCode.UTF16:
				{
					return ReadUTF16(frame,ref index);
				}
				case TextCode.UTF16BE:
				{
					return ReadUTF16BE(frame,ref index);
				}
				case TextCode.UTF8:
				{
					return ReadUTF8(frame,ref index);
				}
				default:
				{
					throw new InvalidFrameException("Invalid TextCode string type.");
				}
			}
		}

		public string ReadTextEnd(byte[]frame, int index, TextCode code)
		{
			switch(code)
			{
				case TextCode.ASCII:
				{
					return ReadASCIIEnd(frame,index);
				}
				case TextCode.UTF16:
				{
					return ReadUTF16End(frame,index);
				}
				case TextCode.UTF16BE:
				{
					return ReadUTF16BEEnd(frame,index);
				}
				case TextCode.UTF8:
				{
					return ReadUTF8End(frame,index);
				}
				default:
				{
					throw new InvalidFrameException("Invalid TextCode string type.");
				}
			}
		}

		public string ReadASCII(byte[] frame,ref int index)
		{
			string text = null;
			int count = Memory.FindByte(frame,0,index);
			if(count == -1)
			{
				throw new InvalidFrameException("Invalid ASCII string size");
			}
			if(count > 0)
			{
				Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
				text = encoding.GetString(frame,index,count);
				index += count; // add the readed bytes
			}
			index++; // jump an end of line byte
			return text;
		}

		public string ReadUTF16(byte[] frame,ref int index)
		{
			string text = null;
			UnicodeEncoding encoding = null;
			bool readString = true;
			if(frame[index] == 0xfe && frame[index+1] == 0xff) // Big Endian
			{
				encoding = new UnicodeEncoding(true,false);
			}
			else
			{
				if(frame[index] == 0xff && frame[index+1] == 0xfe) // Litle Endian
				{
					encoding = new UnicodeEncoding(false,false);
				}
				else
				{
					if(frame[index] == 0x00 && frame[index+1] == 0x00)
					{
						readString = false;
					}
					else
					{
						throw new InvalidFrameException("Invalid UTF16 string.");
					}
				}
			}
			index+=2; // skip the BOM or EOL
			if(readString == true)
			{
				int count = Memory.FindShort(frame,0,index);
				if(count == -1)
				{
					throw new InvalidFrameException("Invalid UTF16 string size.");
				}
				text = encoding.GetString(frame,index,count);
				index += count; // add the readed bytes
				index += 2; // skip the EOL
			}
			return text;
		}

		public string ReadUTF16BE(byte[] frame,ref int index)
		{
			string text = null;
			UnicodeEncoding encoding = new UnicodeEncoding(true,false);
			int count = Memory.FindShort(frame,0,index);
			if(count == -1)
			{
				throw new InvalidFrameException("Invalid UTF16BE string size");
			}
			if(count > 0)
			{
				text = encoding.GetString(frame,index,count);
				index += count; // add the readed bytes
			}
			index+=2; // jump an end of line unicode char
			return text;
		}

		public string ReadUTF8(byte[] frame,ref int index)
		{
			string text = null;
			int count = Memory.FindByte(frame,0,index);
			if(count == -1)
			{
				throw new InvalidFrameException("Invalid UTF8 strng size");
			}
			if(count > 0)
			{
				text = UTF8Encoding.UTF8.GetString(frame,index,count);
				index += count; // add the readed bytes
			}
			index++; // jump an end of line byte
			return text;
		}

		public string ReadASCIIEnd(byte[] frame, int index)
		{
			Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
			return encoding.GetString(frame,index,frame.Length-index);
		}

		public string ReadUTF16End(byte[] frame, int index)
		{
			UnicodeEncoding encoding = null;
			if(frame[index] == 0xfe && frame[index+1] == 0xff) // Big Endian
			{
				encoding = new UnicodeEncoding(true,false);
			}
			else
			{
				if(frame[index] == 0xff && frame[index+1] == 0xfe) // Litle Endian
				{
					encoding = new UnicodeEncoding(false,false);
				}
				else
				{
					throw new InvalidFrameException("Invalid UTF16 string.");
				}
			}
			index+=2; // skip the BOM or EOL
			return encoding.GetString(frame,index,frame.Length-index);
		}

		public string ReadUTF16BEEnd(byte[] frame, int index)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(true,false);
			return encoding.GetString(frame,index,frame.Length-index); 
		}

		public string ReadUTF8End(byte[] frame,int index)
		{
			return UTF8Encoding.UTF8.GetString(frame,index,frame.Length-index);
		}

		// Write rutines
	
		public byte[] WriteText(string text, TextCode code)
		{
			switch(code)
			{
				case TextCode.ASCII:
				{
					return WriteASCII(text);
				}
				case TextCode.UTF16:
				{
					return WriteUTF16(text);
				}
				case TextCode.UTF16BE:
				{
					return WriteUTF16BE(text);
				}
				case TextCode.UTF8:
				{
					return WriteUTF8(text);
				}
				default:
				{
					throw new InvalidFrameException("Invalid TextCode string type.");
				}
			}
		}

		public static byte[] WriteTextEnd(string text, TextCode code)
		{
			switch(code)
			{
				case TextCode.ASCII:
				{
					return WriteASCIIEnd(text);
				}
				case TextCode.UTF16:
				{
					return WriteUTF16End(text);
				}
				case TextCode.UTF16BE:
				{
					return WriteUTF16BEEnd(text);
				}
				case TextCode.UTF8:
				{
					return WriteUTF8End(text);
				}
				default:
				{
					throw new InvalidFrameException("Invalid TextCode string type.");
				}
			}
		}

		public static byte[] WriteASCII(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "") //Write a null string
			{
				writer.Write((byte)0);
				return buffer.ToArray();
			}
			Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
			writer.Write(encoding.GetBytes(text));
			writer.Write((byte)0); //EOL
			return buffer.ToArray();
		}
        public void writeASCII(string text)
        {
            WriteASCII(text);
        }
		public static byte[] WriteUTF16(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "") //Write a null string
			{
				writer.Write((ushort)0);
				return buffer.ToArray();
			}
			writer.Write((byte)0xff); //Litle endian, we have UTF16BE for big endian
			writer.Write((byte)0xfe);
			UnicodeEncoding encoding = new UnicodeEncoding(false,false);
			writer.Write(encoding.GetBytes(text));
			writer.Write((ushort)0);
			return buffer.ToArray();
		}
        public void writeUTF16(string text)
        {
            WriteUTF16(text);
        }
		public static byte[] WriteUTF16BE(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			UnicodeEncoding encoding = new UnicodeEncoding(true,false);
			if(text == null || text == "") // Write a null string
			{
				writer.Write((ushort)0);
				return buffer.ToArray();
			}
			writer.Write(encoding.GetBytes(text));
			writer.Write((ushort)0);
			return buffer.ToArray();
		}
        public void writeUTF16BE(string text)
        {
            WriteUTF16BE(text);
        }
		public static byte[] WriteUTF8(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "") // Write a null string
			{
				writer.Write((byte)0);
				return buffer.ToArray();
			}
			writer.Write(UTF8Encoding.UTF8.GetBytes(text));
			writer.Write((byte)0);
			return buffer.ToArray();
		}
        public void writeUTF8(string text)
        {
            WriteUTF8(text);
        }
		public static byte[] WriteASCIIEnd(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "")
			{
				return buffer.ToArray();
			}
			Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
			writer.Write(encoding.GetBytes(text));
			return buffer.ToArray();
		}
        public void writeASCIIEnd(string text)
        {
            WriteASCIIEnd(text);
        }
		public static byte[] WriteUTF16End(string text)
		{
			MemoryStream buffer = new MemoryStream(text.Length+2);
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "")
			{
				return buffer.ToArray();
			}
			UnicodeEncoding encoding;
			writer.Write((byte)0xff); // Litle endian
			writer.Write((byte)0xfe);
			encoding = new UnicodeEncoding(false,false);
			writer.Write(encoding.GetBytes(text));
			return buffer.ToArray();

		}
        public void writeUTF16End(string text)
        {
            WriteUTF16End(text);
        }
		public static byte[] WriteUTF16BEEnd(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "")
			{
				return buffer.ToArray();
			}
			UnicodeEncoding encoding = new UnicodeEncoding(true,false);
			writer.Write(encoding.GetBytes(text));
			return buffer.ToArray();
		}
        public void writeUTF16BEEnd(string text)
        {
            WriteUTF16BEEnd(text);
        }
		public static byte[] WriteUTF8End(string text)
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			if(text == null || text == "")
			{
				return buffer.ToArray();
			}
			writer.Write(UTF8Encoding.UTF8.GetBytes(text));
			return buffer.ToArray();
		}
        public void writeUTF8End(string text)
        {
            WriteUTF8End(text);
        }
		#endregion
	}
}
