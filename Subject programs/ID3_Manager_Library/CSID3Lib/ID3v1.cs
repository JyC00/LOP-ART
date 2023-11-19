// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
namespace Id3Lib
{
	using System;
	using System.IO;
	using System.Text;
	using System.Diagnostics;

	/// <summary>
	/// Manage ID3v1 tags
	/// </summary>
	/// <remarks>
	/// The <b>ID3v1</b> class can read an ID3v1 tag form a mp3 file returning the <see cref="TagModel"/> and
	/// write an ID3v1 form the TagModel to a mp3 file, it will ignore any fields not supported in ID3v1 tag format.
	/// </remarks>
	public class ID3v1
	{
		#region Fields
		private readonly byte[] _id3 = {0x54,0x41,0x47}; //"TAG"
		private readonly string[] _genres =
		{
			"Blues","Classic Rock","Country","Dance","Disco","Funk","Grunge","Hip-Hop","Jazz","Metal",
			"New Age","Oldies","Other","Pop","R&B","Rap","Reggae","Rock","Techno","Industrial",
			"Alternative","Ska","Death Metal","Pranks","Soundtrack","Euro-Techno","Ambient","Trip-Hop",
			"Vocal","Jazz+Funk","Fusion","Trance","Classical","Instrumental","Acid","House",
			"Game","Sound Clip","Gospel","Noise","Alternative Rock","Bass","Soul","Punk","Space",
			"Meditative","Instrumental Pop","Instrumental Rock","Ethnic","Gothic",
			"Darkwave","Techno-Industrial","Electronic","Pop-Folk","Eurodance","Dream",
			"Southern Rock","Comedy","Cult","Gangsta","Top 40","Christian Rap","Pop/Funk","Jungle",
			"Native American","Cabaret","New Wave","Psychadelic","Rave","Showtunes","Trailer","Lo-Fi",
			"Tribal","Acid Punk","Acid Jazz","Polka","Retro","Musical","Rock & Roll","Hard Rock","Folk",
			"Folk/Rock","National Folk","Swing","Fast-Fusion","Bebob","Latin","Revival","Celtic","Bluegrass",
			"Avantgarde","Gothic Rock","Progressive Rock","Psychedelic Rock","Symphonic Rock","Slow Rock",
			"Big Band","Chorus","Easy Listening","Acoustic","Humour","Speech","Chanson","Opera","Chamber Music",
			"Sonata","Symphony","Booty Bass","Primus","Porn Groove","Satire","Slow Jam","Club",
			"Tango","Samba","Folklore","Ballad","Power Ballad","Rhytmic Soul","Freestyle","Duet",
			"Punk Rock","Drum Solo","Acapella","Euro-House","Dance Hall","Goa","Drum & Bass","Club-House",
			"Hardcore","Terror","Indie","BritPop","Negerpunk","Polsk Punk","Beat","Christian Gangsta Rap",
			"Heavy Metal","Black Metal","Crossover","Contemporary Christian",
			"Christian Rock","Merengue","Salsa","Trash Metal","Anime","JPop","SynthPop"
		};
		private string _Song;
		private string _Artist;
		private string _Album;
		private string _Year;
		private string _Comment;
		private byte _Track;
		private byte _Genere;
		#endregion

		#region Properties
		/// <summary>
		/// Get the title/songname/content description.
		/// </summary>
		public string Song
		{
			get { return _Song; }
		}
		/// <summary>
		/// Get the lead performer/soloist.
		/// </summary>
		public string Artist
		{
			get { return _Artist; }
		}
		/// <summary>
		/// Get the production year.
		/// </summary>
		public string Year
		{
			get { return _Year; }
		}
		/// <summary>
		/// Get the album title.
		/// </summary>
		public string Album
		{
			get { return _Album; }
		}
		
		/// <summary>
		/// Get the track/artist comment.
		/// </summary>
		public string Comment
		{
			get { return _Comment; }
		}
	
		/// <summary>
		/// Get the track number.
		/// </summary>
		public byte Track
		{
			get { return _Track; }
		}
		/// <summary>
		/// Get the track genere.
		/// </summary>
		public byte Genere
		{
			get { return _Genere; }
		}

		/// <summary>
		/// Get or set the ID3v2 TagModel.
		/// </summary>
		public FrameModel TagModel
		{
			get
			{
				FrameModel tagModel = new FrameModel();
				FrameText frameText = (FrameText)FrameFactory.Build("TIT2");
				frameText.TextCode = TextCode.ASCII;
				frameText.Text = _Song;
				tagModel.Frames.Add(frameText);

				frameText = (FrameText)FrameFactory.Build("TPE1");
				frameText.TextCode = TextCode.ASCII;
				frameText.Text = _Artist;
				tagModel.Frames.Add(frameText);

				frameText = (FrameText)FrameFactory.Build("TALB");
				frameText.TextCode = TextCode.ASCII;
				frameText.Text = _Album;
				tagModel.Frames.Add(frameText);

				frameText = (FrameText)FrameFactory.Build("TYER");
				frameText.TextCode = TextCode.ASCII;
				frameText.Text = _Year;
				tagModel.Frames.Add(frameText);

				frameText = (FrameText)FrameFactory.Build("TRCK");
				frameText.TextCode = TextCode.ASCII;
				frameText.Text = _Track.ToString();
				tagModel.Frames.Add(frameText);

				FrameFullText frameLCText = (FrameFullText)FrameFactory.Build("COMM");
				frameLCText.TextCode = TextCode.ASCII;
				frameLCText.Language = "eng";
				frameLCText.Description = "";
				frameLCText.Text = _Comment;
				tagModel.Frames.Add(frameLCText);

				//TODO: Fix this code!!!!!!!!
				tagModel.Header.TagSize = 0; //TODO: Invalid size
				tagModel.Header.Version = 2;
				tagModel.Header.Revision = 3;
				tagModel.Header.Unsync = false;
				tagModel.Header.Experimental = false;
				tagModel.Header.Footer = false;
				tagModel.Header.ExtendedHeader = false;

				return tagModel;
			}
			set
			{
				Reset();
				foreach(FrameBase frame in value.Frames)
				{
					try
					{
						switch(frame.Tag)
						{
							case"TIT2":
							{
								_Song = frame.ToString();
								break;
							}
							case"TPE1":
							{
								_Artist = frame.ToString();
								break;
							}
							case"TALB":
							{
								_Album = frame.ToString();
								break;
							}
							case"TYER":
							{
								_Year = frame.ToString();
								break;
							}
							case"TRCK":
							{
								try
								{
									_Track = byte.Parse(frame.ToString());
								}
								catch
								{
									_Track = 0;
								}
								break;
							}
							case"COMM":
							{
								_Comment = frame.ToString();
								break;
							}
							case"TCON":
							{
								byte nGenere;
								string sGenere = frame.ToString();
								if(sGenere != null)
								{
									sGenere.Trim();
								}
								else
								{
									_Genere = 255;
									break;
								}
								if(sGenere != string.Empty)
								{
									try
									{
										nGenere = byte.Parse(sGenere);
									}
									catch
									{
										
										byte index = 0;
										bool bFound = false;
										foreach(string name in _genres)
										{
											name.Trim();
											if(name == sGenere)
											{
												_Genere = index;
												bFound = true;
												break;
											}
											index++;
										}
										if(bFound == false)
										{
											_Genere = 12;
										}
									}
								}
								else
								{
									_Genere = 255;
								}
								break;
							}
						}
					}
					catch(Exception e)
					{
						Console.WriteLine("Error: {0}",e.Message);
					}
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// ID3v1 tag manager
		/// </summary>
		public ID3v1()
		{
			//Reset();
		}
        public void ID3v1Initialize(string _Song,string _Artist,string _Album,string _Year,string _Comment,int _Track,int _Genere)
        {
            this._Song=_Song;
            this._Artist=_Artist;
            this._Album=_Album;
            this._Year=_Year;
            this._Comment=_Comment;
            this._Track = (byte)_Track;
            this._Genere = (byte)_Genere;
        }
		#endregion

		#region Methods
        public void Reset()
		{
			_Song = "";
			_Artist = "";
			_Album = "";
			_Year = "";
			_Comment = "";
			_Track = 0;
			_Genere = 255;
		}

		/// <summary>
		/// Load tag from sream
		/// </summary>
		/// <param name="src">Binary stream to load</param>
		public void Deserialize(Stream src)
		{
			BinaryReader reader = new BinaryReader(src);

			// check for ID3v1 tag
			Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
			reader.BaseStream.Seek(-128, SeekOrigin.End);

			byte[] idTag = new byte[3];

			// Read the tag identifier
			reader.Read(idTag,0,3);
			// Compare the readed tag
			if(Memory.Compare(_id3,idTag,3) != true)
			{
				throw new TagNotFoundException("ID3v1 tag was not found");
			}
			
			byte[] tag = new byte[30]; // Allocate ID3 tag

			reader.Read(tag,0,30);
			// I had to use Memory.FindByte fuction because the encoding function
			// retrives the 0x00 values and dosen't stop on the first zero. so makes strings with
			// many trailing zeros at the end when converted to XML these are added in binary text.
			int index = Memory.FindByte(tag,0x00,0);

			if(index == -1)
			{
				_Song = encoding.GetString(tag);
			}
			else
			{
				_Song = encoding.GetString(tag,0,index);
			}
			reader.Read(tag,0,30);
			index = Memory.FindByte(tag,0x00,0);
			if(index == -1)
			{
				_Artist = encoding.GetString(tag);
			}
			else
			{
				_Artist = encoding.GetString(tag,0,index);
			}
			reader.Read(tag,0,30);
			index = Memory.FindByte(tag,0x00,0);
			if(index == -1)
			{
				_Album = encoding.GetString(tag);
			}
			else
			{
				_Album = encoding.GetString(tag,0,index);
			}
			reader.Read(tag,0,4);
			if(tag[0] != 0 && tag[1] != 0 && tag[2] != 0 && tag[3] != 0)
			{
				_Year = encoding.GetString(tag,0,4);
			}
			else
			{
				_Year="";
			}
			reader.Read(tag,0,30);
			if (tag[28] == 0) //Track number was stored at position 29 later hack of the original standard.
			{
				_Track = tag[29];
				index = Memory.FindByte(tag,0x00,0);
				if(index != -1)
				{
					_Comment = encoding.GetString(tag, 0, index);
				}
			}
			else
			{
				_Track = 0;
				index = Memory.FindByte(tag,0x00,0);
				if(index == -1)
				{
					_Comment = encoding.GetString(tag);
				}
				else
				{
					_Comment = encoding.GetString(tag, 0, index);
				}
			}
			_Genere = reader.ReadByte();
		}
		/// <summary>
		/// Save tag from sream
		/// </summary>
		/// <param name="src">Binary stream to save</param>
		public void Serialize(Stream src)
		{
			BinaryReader reader = new BinaryReader(src);

			// check for ID3v1 tag
			Encoding encoding = Encoding.GetEncoding(1252); // Should be ASCII
			reader.BaseStream.Seek(-128, SeekOrigin.End);

			byte[] idTag = new byte[3];

			// Read the tag identifier
			reader.Read(idTag,0,3);
			BinaryWriter writer = new BinaryWriter(src);
			long streamPosition = 0;
			// Is there a ID3v1 tag already?
			if(Memory.Compare(_id3,idTag,3) == true)
			{
				//Found a ID3 tag so we will over write the old tag
				writer.BaseStream.Seek(-125, SeekOrigin.End);
			}
			else
			{
				//Create a new Tag
				streamPosition = writer.BaseStream.Position;
				writer.BaseStream.Seek(0,SeekOrigin.End);
				writer.Write(_id3,0,3); // Write the ID3 TAG ID
			}
			try
			{
				byte[] tag = new byte[30];
				
				_Song.Trim();
				if(_Song.Length > 30)
				{
					_Song = _Song.Substring(0,30);
				}
				encoding.GetBytes(_Song,0,_Song.Length,tag,0);
				writer.Write(tag,0,30);
				Memory.Clear(tag,0,30);
				
				_Artist.Trim();
				if(_Artist.Length > 30)
				{
					_Artist = _Artist.Substring(0,30);
				}
				encoding.GetBytes(_Artist,0,_Artist.Length,tag,0);
				writer.Write(tag,0,30);
				Memory.Clear(tag,0,30);
				
				_Album.Trim();
				if(_Album.Length > 30)
				{
					_Album = _Album.Substring(0,30);
				}
				encoding.GetBytes(_Album,0,_Album.Length,tag,0);
				writer.Write(tag,0,30);
				Memory.Clear(tag,0,30);

				_Year.Trim();
				if(_Year == "")
				{
					Memory.Clear(tag,0,30);
				}
				else
				{
					UInt16 year = UInt16.Parse(_Year);
					if(year > 9999)
					{
						year = 0;
					}
					string sYear = year.ToString();
					encoding.GetBytes(sYear,0,sYear.Length,tag,0);
				}
				writer.Write(tag,0,4);
				Memory.Clear(tag,0,30);
				
				if(_Comment.Length > 28)
				{
					_Comment = _Comment.Substring(0,28);
				}
				encoding.GetBytes(_Comment,0,_Comment.Length,tag,0);
			
				writer.Write(tag,0,28);
				Memory.Clear(tag,0,30);
				writer.Write(new byte[]{0},0,1);
				writer.Write(new byte[]{_Track},0,1);
				writer.Write(new byte[]{_Genere},0,1);
			}
			catch
			{
				// There was an error while creating the tag
				if(streamPosition != 0)
				{
					// Restore the file to the original state, I hope.
					writer.BaseStream.SetLength(streamPosition);
					writer.Close();
				}
			}
		}
		#endregion


	}
}

