// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Manages URL Frames
	/// </summary>
	public class FrameUrl : FrameBase
	{
		#region Fields
		private TextCode _textEncoding;
		private string _contents;
		private string _url;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a URL frame
		/// </summary>
		/// <param name="tag">Type of URL frame</param>
		public FrameUrl(string tag):base(tag)
		{
			_textEncoding = TextCode.ASCII;
		}
        public FrameUrl() { }
        public void FrameUrlInitialize(string _contents,string _url,string _description, string _tag, int _flags)
        {
            this._contents = _contents;
            this._url = _url;
            this._description = _description;
            this._tag = _tag;
            this._flags = (ushort)_flags;
        }
		#endregion

		#region Properties
		/// <summary>
		/// Type of text encoding the frame is using
		/// </summary>
		public TextCode TextCode
		{
			get { return _textEncoding;}
			set { _textEncoding = value;}
		}
		
		/// <summary>
		/// Description of the frame contents
		/// </summary>
		public string Description
		{
			get{return _contents;}
			set{_contents = value;}
		}
		
		/// <summary>
		/// The URL page location
		/// </summary>
		public string URL
		{
			get{return _url;}
			set{_url = value;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Parse the binary frame
		/// </summary>
		/// <param name="frame">binary frame</param>
		public override void Parse(byte[] frame)
		{
            //TODO: Handle this invalid tag
            if (frame.Length < 1)
                return;

			int index = 0;
			_textEncoding = (TextCode)frame[index];
			index++;
			_contents = TextBuilder.ReadText(frame,ref index,_textEncoding);
			_url = TextBuilder.ReadTextEnd(frame,index,_textEncoding);
		}

		/// <summary>
		/// Create a binary frame
		/// </summary>
		/// <returns>binary frame</returns>
		public override byte[] Make()
		{
			MemoryStream buffer = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(buffer);
			writer.Write((byte)_textEncoding);
			writer.Write(TextBuilder.WriteText(_contents,_textEncoding));
			writer.Write(TextBuilder.WriteTextEnd(_url,_textEncoding));
			return buffer.ToArray();
		}
		/// <summary>
		/// Default frame description
		/// </summary>
		/// <returns>URL text</returns>
		public override string ToString()
		{
			return _url;
		}
		#endregion
	}
}
