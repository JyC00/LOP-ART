// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Manages the ID3v2 tag header.
	/// </summary>
	/// <remarks>
	///  The <b>Header</b> class manages the first part of the ID3v2 tag that is the first ten bytes
	///  of the ID3v1 tag.
	/// </remarks>
	public class Header
	{
		#region Fields

		private byte _id3Version = 3;
		private byte _id3Revision = 0;
		private byte _id3Flags = 0;
		private int _id3RawSize = 0;
		private bool _padding = true;
		private int _paddingSize = 0;

		private readonly byte[] _id3 = {0x49,0x44,0x33}; //"ID3" tag

		#endregion

		#region Methods
        public Header() { }
        public void HeaderInitialize(int _id3Version, int _id3Revision, int _id3Flags, int _id3RawSize, bool _padding, int _paddingSize)
        {
            this._id3Version = (byte)_id3Version;
            this._id3Revision = (byte)_id3Revision;
            this._id3Flags = (byte)_id3Flags;
            this._id3RawSize=_id3RawSize;
            this._padding=_padding;
            this._paddingSize = _paddingSize;
        }
		/// <summary>
		/// Save header into the stream.
		/// </summary>
		/// <param name="stream">Stream to save header</param>
		public void Serialize(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			//TODO: Validate version and revision we support
			writer.Write(_id3);
			writer.Write(_id3Version);
			writer.Write(_id3Revision);
			writer.Write(_id3Flags);
			writer.Write(Swap.Int32(Sync.Safe(_id3RawSize)));
		}
		/// <summary>
		/// Load header from the stream.
		/// </summary>
		/// <param name="stream">Stream to load header</param>
		public void Deserialize(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			byte[] idTag = new byte[3];

			// Read the tag identifier
			reader.Read(idTag,0,3);
			// Compare the readed tag
			if(Memory.Compare(_id3,idTag,3) == false)
			{
				throw new TagNotFoundException("ID3v2 tag identifier was not found");
			}
			// Get the id3 version byte
			_id3Version = reader.ReadByte();  
			if( _id3Version == 0xff)
			{
				throw new InvalidTagException("Corrupt header, invalid ID3v2 version.");
			}
			// Get the id3 revision byte
			_id3Revision = reader.ReadByte(); 
			if(_id3Revision == 0xff)
			{
				throw new InvalidTagException("Corrupt header, invalid ID3v2 revision.");
			}
			// Get the id3 flag byte, only read what I understand
			_id3Flags = (byte)(0xf0 & reader.ReadByte());
			// Get the id3 size, swap and unsync the integer
			_id3RawSize = Swap.Int32(Sync.UnsafeBigEndian(reader.ReadInt32()));
			if(_id3RawSize == 0)
			{
				throw new InvalidTagException("Corrupt header, tag size can't be zero.");
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// Get the size of the header only.
		/// </summary>
		public int HeaderSize
		{
			get{return 10;} // ID3 Header size is fixed
		}

		/// <summary>
		/// Get or set ID3v2 major version number.
		/// </summary>
		public byte Version
		{
			get{return _id3Version;}
			set{_id3Version = value;}
		}
		/// <summary>
		/// Get or set the ID3v2 revision number.
		/// </summary>
		public byte Revision
		{
			get{return _id3Revision;}
			set{_id3Revision = value;}
		}
		/// <summary>
		/// Get or set the complete ID3v2 tag size.
		/// </summary>
		public int TagSize
		{
			get{return _id3RawSize;}
			set{_id3RawSize = value;}
		}
		/// <summary>
		/// Get or set if unsynchronisation is applied on all frames.
		/// </summary>
		public bool Unsync
		{
			get{return (_id3Flags & 0x80) > 0;}
			set
			{
				if(value == true)
				{
					_id3Flags |= 0x80;
				}
				else
				{
					unchecked{_id3Flags &= (byte)~(0x80);}
				}
			}
		}
		/// <summary>
		/// Get or set if the header is followed by an extended header.
		/// </summary>
		public bool ExtendedHeader
		{
			get{return (_id3Flags & 0x40) > 0;}
			set
			{
				if(value == true)
				{
					_id3Flags |= 0x40;
				}
				else
				{
					unchecked{_id3Flags &= (byte)~(0x40);}
				}
			}
		}
		/// <summary>
		/// Get or set if the tag is experimental stage.
		/// </summary>
		/// <remarks>
		/// This flag shall always be set when the tag is in an experimental stage.
		/// </remarks>
		public bool Experimental
		{
			get{return (_id3Flags & 0x20)  > 0;}
			set
			{
				if(value == true)
				{
					_id3Flags |= 0x20;
				}
				else
				{
					unchecked{_id3Flags &= (byte)~(0x20);}
				}
			}
		}
		/// <summary>
		/// Get or set if a footer is present at the end of the tag.
		/// </summary>
		/// <remarks>
		/// Can't be uesd simultaneously with the frame padding they are mutualy exlusive.
		/// </remarks>
		public bool Footer
		{
			get{return (_id3Flags & 0x10) > 0;}
			set
			{
				if(value == true)
				{
					_id3Flags |= 0x10;
					_padding = false;
				}
				else
				{
					unchecked{_id3Flags &= (byte)~(0x10);}
				}
			}
		}
		
		/// <summary>
		/// Get or set if padding is applied on the tag.
		/// </summary>
		/// <remarks>
		/// Can't be uesd simultanipusly with the frame footer they are mutualy exlusive.
		/// </remarks>
		public bool Padding
		{
			set
			{
				if(value == true)
				{
					Footer = false;
				}
				_padding = value;
			}
			get
			{
				return _padding;
			}
		}

		/// <summary>
		/// Get or set if padding size.
		/// </summary>
		public int PaddingSize
		{
			set
			{
				_paddingSize = value;
			}
			get
			{
				return _paddingSize;
			}
		}
		#endregion

	}
}
