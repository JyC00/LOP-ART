// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Collections.Generic;

namespace Id3Lib
{
	/// <summary>
	/// Manages an ID3v2 tag as an object reprsentation. 
	/// </summary>
	/// <remarks>
    /// The <b>FrameModel</b> class represents a ID3v2 tag, it contains a <see cref="Header"/> that
	/// handles the tag header, an <see cref="ExtendedHeader"/> that it is optional and 
	/// stores the frames.
	/// </remarks>
	public class FrameModel
	{
		#region Fields
		private Header _tagHeader  = new Header();
		private ExtendedHeader _tagExtendedHeader  = new ExtendedHeader();
        private List<FrameBase> _tags = new List<FrameBase>();
		#endregion
        public FrameModel() { }
        public void FrameModelInitialize(Header _tagHeader,ExtendedHeader _tagExtendedHeader)
        {
            this._tagHeader=_tagHeader;
            this._tagExtendedHeader = _tagExtendedHeader;
            _tags.Add(new FrameBase());
        }

		#region Properties
		/// <summary>
		/// Get or set the header.
		/// </summary>
		public Header Header
		{
			get{ return _tagHeader;}
			set{ _tagHeader = value;}
		}

		/// <summary>
		/// Get or set extended header.
		/// </summary>
		public ExtendedHeader ExtendedHeader
		{
			get{ return _tagExtendedHeader;}
			set{ _tagExtendedHeader = value;}
		}

		/// <summary>
		/// Get or set the frames.
		/// </summary>
		public List<FrameBase> Frames
		{
			get{ return _tags; }
			set{ _tags = value; }
		}
		#endregion
	}
}
