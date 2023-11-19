// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// ID3 Extended Header manager
	/// </summary>
	/// <remarks>
	/// The extended header contains information that can provide further
	/// insight in the structure of the tag, but is not vital to the correct
	/// parsing of the tag information; hence the extended header is optional.
	/// </remarks>
	public class ExtendedHeader
	{
		#region Fields
		private Header _tagHeader;
		private int _size;
		private byte[] _extendedHeader = null;
		#endregion

		#region Properties
		/// <summary>
		/// Set a reference to the ID3 header
		/// </summary>
		public Header Header
		{
			set{_tagHeader = value;}
		}
		/// <summary>
		/// Get the size of the extended header
		/// </summary>
		public int Size
		{
			get{return _size;}
		}
		#endregion

		#region Methods
        public ExtendedHeader() { }
        public void ExtendedHeaderInitialize(int _size, Header _tagHeader)
        {
            this._size=_size;
            this._tagHeader = _tagHeader;
        }
		/// <summary>
		/// Load the ID3 extended header from a stream
		/// </summary>
		/// <param name="stream">Binary stream containing a ID3 extended header</param>
		public void Deserialize(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			_size = Swap.Int32(Sync.UnsafeBigEndian(reader.ReadInt32()));
			if(_size < 6)
			{
				throw new Exception("corrupt extended header");
			}
			// TODO: implement the extended header, copy for now since it's optional
			_extendedHeader = new Byte[_size];
			stream.Read(_extendedHeader,0,_size);
		}

		/// <summary>
		/// Save the ID3 extended header from a stream
		/// </summary>
		/// <param name="stream">Binary stream containing a ID3 extended header</param>
		public void Serialize(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			// TODO: implement the extended header, for now write the original header
			writer.Write(_extendedHeader);
		}
		#endregion
	}
}
