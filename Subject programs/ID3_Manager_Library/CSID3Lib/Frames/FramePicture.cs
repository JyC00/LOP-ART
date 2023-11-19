// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Picture Frame
	/// </summary>
	public class FramePicture : FrameBase
	{
		#region Fields
		private TextCode   _textEncoding;
		private string _mime;
		private byte   _pictureType;
		private byte[] _pictureData;
		#endregion

		#region Constructors
		/// <summary>
		/// Picture Frame
		/// </summary>
		public FramePicture():base("APIC")
		{
			_textEncoding = TextCode.ASCII;
		}
        public void FramePictureInitialize(string _mime,string _description, string _tag, int _flags)
        {
            this._mime = _mime;
            this._description = _description;
            this._tag = _tag;
            this._flags = (ushort)_flags;
        }
		/// <summary>
		/// Picture Frame
		/// </summary>
		public FramePicture(string tag):base(tag)
		{
			_textEncoding = TextCode.ASCII;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Type of text encoding
		/// </summary>
		public TextCode TextEncoding
		{
			get{ return _textEncoding;}
			set{ _textEncoding = value;}
		}

		/// <summary>
		/// Picture MIME type
		/// </summary>
		public string Mime
		{
			get{ return _mime;}
			set{ _mime = value;}
		}

		/// <summary>
		/// Desctiption of the picture
		/// </summary>
		public byte PictureType
		{
			get{ return _pictureType;}
			set{ _pictureType = value;}
		}

		/// <summary>
		/// Desctiption of the picture
		/// </summary>
		public string PictureDescription
		{
			get { return _description;}
			set { _description = value;}
		}
		/// <summary>
		/// Binary data representing the picture
		/// </summary>
		public byte[] PictureData
		{
			get {return _pictureData;}
			set { _pictureData = value;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Load from binary data a picture frame
		/// </summary>
		/// <param name="frame">picture binary representation</param>
		public override void Parse(byte[] frame)
		{
			int index = 0;
			_textEncoding = (TextCode)frame[index];
			index++;
			_mime = TextBuilder.ReadASCII(frame,ref index);
			_pictureType = frame[index];
			index++;
			_description = TextBuilder.ReadText(frame,ref index,_textEncoding);
			_pictureData = new byte[frame.Length - index];
			Memory.Copy(frame,index,_pictureData,0,frame.Length - index);
		}
		/// <summary>
		///  Save picture frame to binary data
		/// </summary>
		/// <returns>picture binary representation</returns>
		public override byte[] Make()
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			writer.Write((byte)_textEncoding);
			writer.Write(TextBuilder.WriteASCII(_mime));
			writer.Write(_pictureType);
			writer.Write(TextBuilder.WriteText(_description,_textEncoding));
			writer.Write(_pictureData);
			return buffer.ToArray();
		}

		/// <summary>
		/// Get a desciprion of the picrure frame
		/// </summary>
		/// <returns>Pictrure description</returns>
		public override string ToString()
		{
			return _description;
		}
		#endregion
	}
}
