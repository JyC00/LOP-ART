// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;

namespace Id3Lib
{
	/// <summary>
	/// Helper that handles header flags.
	/// </summary>
	public class FlagHandler
	{
		#region Fields
		private byte _version = 2;
		private byte _revison = 3;
		private ushort _flags = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a FlagHandler.
		/// </summary>
		/// <param name="tagHeader">ID3 Header</param>
		public FlagHandler(Header tagHeader)
		{
			if(tagHeader.Version != 3 & tagHeader.Version != 4)
			{
				throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
			}
			_version = tagHeader.Version;
			_revison = tagHeader.Revision;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the ID3v2 Version
		/// </summary>
		public byte Version
		{
			get{ return _version;}
		}
		/// <summary>
		/// Get the ID3v2 Revision
		/// </summary>
		public byte Revision
		{
			get{ return _revison;}
		}

		/// <summary>
		/// Get or set the ID3v2 Frame flags
		/// </summary>
		public ushort Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}


		/// <summary>
		/// Get or set the tag ater flag</summary>
		/// <remarks>
		/// This flag tells the tag parser what to do with this frame if it is
		/// unknown and the tag is altered in any way.																												the frames.
		/// </remarks>
		public bool TagAlter
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x8000) > 0;
					}
					case 4:
					{
						return(_flags & 0x4000) > 0;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value  ?(ushort)(_flags | 0x8000):(ushort)(_flags & unchecked((ushort)~(0x8000)));
						break;
					}
					case 4:
					{
						_flags = value  ?(ushort)(_flags | 0x4000):(ushort)(_flags & unchecked((ushort)~(0x4000)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}
		
		/// <summary>
		/// Get or set the file alter flag
		/// </summary>
		/// <remarks>
		/// This flag tells the tag parser what to do with this frame if it is
		/// unknown and the file, excluding the tag, is altered.
		/// </remarks>
		public bool FileAlter
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x4000) > 0;;
					}
					case 4:
					{
						return (_flags & 0x2000) > 0;;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value ?(ushort)(_flags | 0x4000):(ushort)(_flags & unchecked((ushort)~(0x4000)));
						break;
					}
					case 4:
					{
						_flags = value ?(ushort)(_flags | 0x2000):(ushort)(_flags & unchecked((ushort)~(0x2000)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}

		/// <summary>
		/// Get or set the read only flag
		/// </summary>
		/// <remarks>
		/// This flag, if set, tells the software that the contents of this
		/// frame are intended to be read only.
		/// </remarks>
		public bool ReadOnly
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x2000) > 0;
					}
					case 4:
					{
						
						return (_flags & 0x1000) > 0;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value  ?(ushort)(_flags | 0x2000):(ushort)(_flags & unchecked((ushort)~(0x2000)));			
						break;
					}
					case 4:
					{
						_flags = value  ?(ushort)(_flags | 0x1000):(ushort)(_flags & unchecked((ushort)~(0x1000)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}


		/// <summary>
		/// Get or set the grouping flag
		/// </summary> 
		/// <remarks>
		/// This flag indicates whether or not this frame belongs in a group with other frames.
		/// </remarks>
		public bool Grouping
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x0020) > 0;
					}
					case 4:
					{
						return (_flags & 0x0040) > 0;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value  ?(ushort)(_flags | 0x0020):(ushort)(_flags & unchecked((ushort)~(0x0020)));
						break;
					}
					case 4:
					{
						_flags = value  ?(ushort)(_flags | 0x0040):(ushort)(_flags & unchecked((ushort)~(0x0040)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}
	
		/// <summary>
		/// Get or set the compression flag.
		/// </summary>
		/// <remarks>
		/// This flag indicates whether or not the frame is compressed.
		/// </remarks>
		public bool Compression
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x0080) > 0;
					}
					case 4:
					{
						return (_flags & 0x0008) > 0;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value ?(ushort)(_flags | 0x0080):(ushort)(_flags & unchecked((ushort)~(0x0080)));
						break;
					}
					case 4:
					{
						_flags = value ?(ushort)(_flags | 0x0008):(ushort)(_flags & unchecked((ushort)~(0x0008)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}


		/// <summary>
		/// Get or set the encryption flag.
		/// </summary>
		/// <remarks>
		/// This flag indicates whether or not the frame is encrypted.
		/// </remarks>
		public bool Encryption
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return (_flags & 0x0040) > 0;
					}
					case 4:
					{
						return (_flags & 0x0004) > 0;;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						_flags = value  ?(ushort)(_flags | 0x0040):(ushort)(_flags & unchecked((ushort)~(0x0040)));
						break;
					}
					case 4:
					{
						_flags = value  ?(ushort)(_flags | 0x0004):(ushort)(_flags & unchecked((ushort)~(0x0004)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}

		/// <summary>
		/// Get or set the unsynchronisation flag.
		/// </summary>
		/// <remarks>
		/// This flag indicates whether or not unsynchronisation was applied to this frame.
		/// </remarks>
		public bool Unsynchronisation
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return false;
					}
					case 4:
					{
						return (_flags & 0x0002) > 0;;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						break;
					}
					case 4:
					{
						_flags = value ?(ushort)(_flags | 0x0002):(ushort)(_flags & unchecked((ushort)~(0x0002)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}

		/// <summary>
		/// Get or set the data length.
		/// </summary>
		/// <remarks>
		/// This flag indicates that a data length indicator has been added to the frame.
		/// </remarks>
		public bool DataLength
		{
			get
			{
				switch(_version)
				{
					case 3:
					{
						return false;
					}
					case 4:
					{
						return (_flags & 0x0001) > 0;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
			set
			{
				switch(_version)
				{
					case 3:
					{
						break;
					}
					case 4:
					{
						_flags = value ?(ushort)(_flags | 0x0001):(ushort)(_flags & unchecked((ushort)~(0x0001)));
						break;
					}
					default:
					{
						throw new NotImplementedException("ID3v2 Version " + _version + " is not supported.");
					}
				}
			}
		}
		#endregion
	}
}

