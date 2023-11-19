// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
namespace Id3Lib
{
	using System;
	using System.IO;
	using System.Text;
	using System.Diagnostics;

	/// <summary>
	/// Reduce the compexity the tag model to a simple interface
	/// </summary>
	public class TagHandler
	{

		#region Fields
		private FrameModel _tagModel = null;
		private TextCode _textCode = TextCode.ASCII; // Default text code
		private string _language = "eng"; // Default language
		#endregion

		#region Properties
		/// <summary>
		/// Get the title/songname/content description.
		/// </summary>
		public string Song
		{
			get
			{
				return GetTextFrame("TIT2");	
			}
			set
			{
				SetTextFrame("TIT2",value);
			}
		}
		/// <summary>
		/// Get the lead performer/soloist.
		/// </summary>
		public string Artist
		{
			get
			{
				return GetTextFrame("TPE1");	
			}
			set
			{
				SetTextFrame("TPE1",value);
			}
		}
		/// <summary>
		/// Get the production year.
		/// </summary>
		public string Year
		{
			get
			{
				return GetTextFrame("TYER");	
			}
			set
			{
				SetTextFrame("TYER",value);
			}
		}
		/// <summary>
		/// Get the album title.
		/// </summary>
		public string Album
		{
			get
			{
				return GetTextFrame("TALB");	
			}
			set
			{
				SetTextFrame("TALB",value);
			}
		}
		
		/// <summary>
		/// Get the track/artist comment.
		/// </summary>
		public string Comment
		{
			get
			{
				FrameBase frame = FindFrame("COMM");
				if(frame != null)
				{
					return ((FrameFullText)frame).Text;
				}
				return string.Empty;
				
			}
			set 
			{
				FrameBase frame = FindFrame("COMM");
				if(frame != null)
				{
					if(value != string.Empty)
					{
						((FrameFullText)frame).Text = value;
						((FrameFullText)frame).TextCode = _textCode;
						((FrameFullText)frame).Description = string.Empty;
						((FrameFullText)frame).Language = _language;
					}
					else
					{
						_tagModel.Frames.Remove(frame);
					}
				}
				else
				{
					if(value != string.Empty)
					{
						FrameFullText frameLCText = (FrameFullText)FrameFactory.Build("COMM");
						frameLCText.TextCode = this._textCode;
						frameLCText.Language = "eng";
						frameLCText.Description = string.Empty;
						frameLCText.Text = value;
						_tagModel.Frames.Add(frameLCText);
					}
				}

			}
		}
	
		/// <summary>
		/// Get the track number.
		/// </summary>
		public string Track
		{
			get
			{
				return GetTextFrame("TALB");	
			}
			set
			{
				SetTextFrame("TALB",value);
			}
		}
		/// <summary>
		/// Get the track genere.
		/// </summary>
		public string Genere
		{
			get
			{
				return GetTextFrame("TALB");	
			}
			set
			{
				SetTextFrame("TALB",value);
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Set the frame text
		/// </summary>
		/// <param name="tag">Frame type</param>
		/// <param name="message">Value set in frame</param>
        public void SetTextFrame(string tag, string message)
		{
			FrameBase frame = FindFrame(tag);
			if(frame != null)
			{
				if(message != string.Empty)
				{
					((FrameText)frame).Text = message;
				}
				else
				{
					_tagModel.Frames.Remove(frame);
				}
			}
			else
			{
				if(message != string.Empty)
				{
					FrameText frameText = (FrameText)FrameFactory.Build(tag);
					frameText.Text = message;
					frameText.TextCode = _textCode;
					_tagModel.Frames.Add(frameText);
				}
			}
		}

		/// <summary>
		/// Get the frame text
		/// </summary>
		/// <param name="tag">Frame type</param>
		/// <returns>Frame text</returns>
        public string GetTextFrame(string tag)
		{
			FrameBase frame = FindFrame(tag);
			if(frame != null)
			{
				return ((FrameText)frame).Text;
			}
			return string.Empty;
		}

		/// <summary>
		/// Find a frame in the model
		/// </summary>
		/// <param name="tag">Frame type</param>
		/// <returns>The found frame if found, otherwise null</returns>
        public FrameBase FindFrame(string tag)
		{
			foreach(FrameBase frame in _tagModel.Frames)
			{
				if(frame.Tag == tag)
				{
					return frame;
				}
			}
			return null;
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Attach to the TagModel
		/// </summary>
		/// <param name="tagModel">Tag model to handle</param>
		public TagHandler(FrameModel tagModel)
		{
			_tagModel = tagModel;
		}
        
        public TagHandler(){}
        public void TagHandlerInitialize(string _language, FrameModel _tagModel)
        {
            this._language=_language;
            this._tagModel = _tagModel;
        }
		#endregion
	}
}

