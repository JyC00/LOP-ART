// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;

namespace Id3Lib
{
	/// <summary>
	/// Summary description for BaseFrame.
	/// </summary>
	public class FrameBase
	{
		#region Fields
		public string _description = null;
		public string _tag = null;
		public ushort _flags = 0;
		#endregion

		#region Constructor
		internal FrameBase(string tag)
		{
			tag.Trim();
            //if(tag.Length != 4)
            //    throw new InvalidFrameException("Invalid frame type: '" + tag +"', it must be 4 characters long.");

			_tag = tag;
		}
        public FrameBase() { }
        public void FrameBaseInitialize(string _description, string _tag, int _flags)
        {
            this._description=_description;
            this._tag=_tag;
            this._flags = (ushort)_flags;
        }
		#endregion

		#region  Properties
		/// <summary>
		/// Tag frame flags
		/// </summary>
		public ushort Flags
		{
			get{return _flags;}
			set{_flags = value;}
		}
		/// <summary>
		/// Description of the ID3 tag frame
		/// </summary>
		public string TagDescription
		{
			get{ return _description; }
		}
			
		/// <summary>
		/// ID3 Tag frame type
		/// </summary>
		public string Tag
		{
			get{return _tag;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// ID3 frame tag description
		/// </summary>
		/// <param name="description">The tag frame description</param>
		public void SetDescription(string description)
		{
			_description = description;
		}

		/// <summary>
		/// Load frame form binary data
		/// </summary>
		/// <param name="frame">binary tag frame representation</param>
        public virtual void Parse(byte[] frame) { }
		/// <summary>
		/// Save frame to binary data
		/// </summary>
		/// <returns>binary tag frame representation</returns>
        public virtual byte[] Make() { return new byte[0]; }
		#endregion
	}
}
