// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Manage general encapsulated objects.
	/// </summary>
	/// <remarks>
    /// The <b>FrameBinary</b> class handles GEOB ID3v2 frame types that can hold any type of file
	/// or binary data encapsulated.
	/// </remarks>
	public class FrameBinary : FrameBase
	{
		#region Fields
		private TextCode _textEncoding;
		private string _mime;
		private string _fileName;
		private byte[] _objectData;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a FrameGEOB frame.
		/// </summary>
		/// <param name="tag">ID3v2 GEOB frame</param>
		internal FrameBinary(string tag):base(tag)
		{
			_textEncoding = TextCode.ASCII;
		}
        public FrameBinary() { }
        public void FrameBinaryInitialize(string _mime,string _fileName,string _description, string _tag, int _flags)
        {
            this._mime=_mime;
            this._fileName = _fileName;
            this._description = _description;
            this._tag = _tag;
            this._flags = (ushort)_flags;
        }
		#endregion

		#region Properties
		/// <summary>
		/// Type of text encoding
		/// </summary>
		public TextCode TextEncoding
		{
			get{ return _textEncoding;}
		}

		/// <summary>
		/// Text MIME type
		/// </summary>
		public string Mime
		{
			get{ return _mime;}
		}

		/// <summary>
		/// Frame description
		/// </summary>
		public string ObjectDescription
		{
			get { return _description;}
		}

		/// <summary>
		/// Binary representation of the object
		/// </summary>
		public byte[] ObjectData
		{
			get {return _objectData;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Parse the binary GEOB frame
		/// </summary>
		/// <param name="frame">binary frame</param>
		public override void Parse(byte[] frame)
		{
			int index = 0;
			_textEncoding = (TextCode)frame[index];
			index++;
			_mime = TextBuilder.ReadASCII(frame,ref index);
			_fileName = TextBuilder.ReadText(frame,ref index,_textEncoding);
			_description = TextBuilder.ReadText(frame,ref index,_textEncoding);
			_objectData = new byte[frame.Length - index];
			Memory.Copy(frame,index,_objectData,0,frame.Length - index);
		}

		/// <summary>
		/// Create a binary GEOB frame
		/// </summary>
		/// <returns>binary frame</returns>
		public override byte[] Make()
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			writer.Write((byte)_textEncoding);
			writer.Write(TextBuilder.WriteASCII(_mime));
			writer.Write(TextBuilder.WriteText(_fileName,_textEncoding));
			writer.Write(TextBuilder.WriteText(_description,_textEncoding));
			writer.Write(_objectData);
			return buffer.ToArray();
		}

		/// <summary>
		/// GEOB frame description 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _description;
		}
		#endregion
	}
}
