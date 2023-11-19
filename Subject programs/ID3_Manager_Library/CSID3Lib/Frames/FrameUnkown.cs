// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Text;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Container for unknown frames.
	/// </summary>
	/// <remarks>
	/// The <b>FrameUnknown</b> class handles unknown frames so they can be restored
	/// or discarded later.
	/// </remarks>
	public class FrameUnknown : FrameBase
	{
		#region Fields
		private byte[] _data;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an unknown frame object.
		/// </summary>
		/// <param name="tag">ID3v2 type of unnown frame</param>
		internal FrameUnknown(string tag):base(tag)
		{
		}
        public FrameUnknown() { }
        public void FrameUnknownInitialize(string _data, string _description, string _tag, int _flags)
        {

            this._data = new byte[_data.Length];
            for (int i = 0; i < _data.Length; i++)
            {
                this._data[i] = (byte)_data[i];
            }
            this._description = _description;
            this._tag = _tag;
            this._flags = (ushort)_flags;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the binary frame
        /// </summary>
        /// <param name="frame">binary frame unknown</param>
        public override void Parse(byte[] frame)
		{
			_data = frame;
		}

		/// <summary>
		/// Get a binary frame
		/// </summary>
		/// <returns>binary frame unknown</returns>
		public override byte[] Make()
		{
			return _data;
		}
		/// <summary>
		/// Default Frame description
		/// </summary>
		/// <returns>Unknown ID3 tag</returns>
		public override string ToString()
		{
			return "Unknown ID3 tag";
		}
		#endregion
	}
}
