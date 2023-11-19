// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Id3Lib;

namespace TagEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ID3Edit : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabPage _tabPageGeneric;
		private System.Windows.Forms.Label _labelGenere;
		private System.Windows.Forms.Label _labelYear;
		private System.Windows.Forms.TabControl _tabControlLyrics;
		private System.Windows.Forms.ComboBox _comboBoxGenere;
		private System.Windows.Forms.TextBox _textBoxYear;
		private System.Windows.Forms.Label _labelAlbum;
		private System.Windows.Forms.Label _labelArtist;
		private System.Windows.Forms.TextBox _textBoxAlbum;
		private System.Windows.Forms.TextBox _textBoxArtist;
		private System.Windows.Forms.TextBox _textBoxTitle;
		private System.Windows.Forms.Label _labelTitle;
		private System.Windows.Forms.TabPage _tabPageLyrics;
		private System.Windows.Forms.TextBox _textBoxLyrics;
		private System.Windows.Forms.TabPage _tabPageMore;
		private System.Windows.Forms.Label _labelComposer;
		private System.Windows.Forms.TextBox _textBoxComposer;
		private System.Windows.Forms.TextBox _textBoxTrackNo;
		private System.Windows.Forms.Label _labelTrackNo;
		private System.Windows.Forms.PictureBox _artPictureBox;
		private System.Windows.Forms.TabPage _tabPageComments;
		private System.Windows.Forms.TextBox _textBoxComments;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _removePicture;
		private System.Windows.Forms.Button _addPicture;
		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;

		/// <summary>
		/// Tag Model reference
		/// </summary>
		private FrameModel _tagModel = null;

		public ID3Edit()
		{
			InitializeComponent();
		}


		public FrameModel TagModel
		{
			set{_tagModel = value;}
			get{return _tagModel;}
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._tabControlLyrics = new System.Windows.Forms.TabControl();
			this._tabPageGeneric = new System.Windows.Forms.TabPage();
			this._addPicture = new System.Windows.Forms.Button();
			this._removePicture = new System.Windows.Forms.Button();
			this._artPictureBox = new System.Windows.Forms.PictureBox();
			this._comboBoxGenere = new System.Windows.Forms.ComboBox();
			this._labelGenere = new System.Windows.Forms.Label();
			this._textBoxYear = new System.Windows.Forms.TextBox();
			this._labelYear = new System.Windows.Forms.Label();
			this._labelAlbum = new System.Windows.Forms.Label();
			this._labelArtist = new System.Windows.Forms.Label();
			this._textBoxAlbum = new System.Windows.Forms.TextBox();
			this._textBoxArtist = new System.Windows.Forms.TextBox();
			this._textBoxTrackNo = new System.Windows.Forms.TextBox();
			this._labelTrackNo = new System.Windows.Forms.Label();
			this._textBoxTitle = new System.Windows.Forms.TextBox();
			this._labelTitle = new System.Windows.Forms.Label();
			this._tabPageLyrics = new System.Windows.Forms.TabPage();
			this._textBoxLyrics = new System.Windows.Forms.TextBox();
			this._tabPageComments = new System.Windows.Forms.TabPage();
			this._textBoxComments = new System.Windows.Forms.TextBox();
			this._tabPageMore = new System.Windows.Forms.TabPage();
			this._labelComposer = new System.Windows.Forms.Label();
			this._textBoxComposer = new System.Windows.Forms.TextBox();
			this._errorProvider = new System.Windows.Forms.ErrorProvider();
			this._buttonOK = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._tabControlLyrics.SuspendLayout();
			this._tabPageGeneric.SuspendLayout();
			this._tabPageLyrics.SuspendLayout();
			this._tabPageComments.SuspendLayout();
			this._tabPageMore.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tabControlLyrics
			// 
			this._tabControlLyrics.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this._tabPageGeneric,
																							this._tabPageLyrics,
																							this._tabPageComments,
																							this._tabPageMore});
			this._tabControlLyrics.Location = new System.Drawing.Point(8, 8);
			this._tabControlLyrics.Name = "_tabControlLyrics";
			this._tabControlLyrics.SelectedIndex = 0;
			this._tabControlLyrics.Size = new System.Drawing.Size(552, 344);
			this._tabControlLyrics.TabIndex = 0;
			// 
			// _tabPageGeneric
			// 
			this._tabPageGeneric.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this._addPicture,
																						  this._removePicture,
																						  this._artPictureBox,
																						  this._comboBoxGenere,
																						  this._labelGenere,
																						  this._textBoxYear,
																						  this._labelYear,
																						  this._labelAlbum,
																						  this._labelArtist,
																						  this._textBoxAlbum,
																						  this._textBoxArtist,
																						  this._textBoxTrackNo,
																						  this._labelTrackNo,
																						  this._textBoxTitle,
																						  this._labelTitle});
			this._tabPageGeneric.Location = new System.Drawing.Point(4, 22);
			this._tabPageGeneric.Name = "_tabPageGeneric";
			this._tabPageGeneric.Size = new System.Drawing.Size(544, 318);
			this._tabPageGeneric.TabIndex = 0;
			this._tabPageGeneric.Text = "Generic";
			// 
			// _addPicture
			// 
			this._addPicture.Location = new System.Drawing.Point(288, 248);
			this._addPicture.Name = "_addPicture";
			this._addPicture.Size = new System.Drawing.Size(96, 23);
			this._addPicture.TabIndex = 14;
			this._addPicture.Text = "Add Picture";
			this._addPicture.Click += new System.EventHandler(this.addPicture_Click);
			// 
			// _removePicture
			// 
			this._removePicture.Location = new System.Drawing.Point(288, 280);
			this._removePicture.Name = "_removePicture";
			this._removePicture.Size = new System.Drawing.Size(96, 23);
			this._removePicture.TabIndex = 13;
			this._removePicture.Text = "Remove Picture";
			this._removePicture.Click += new System.EventHandler(this.removePicture_Click);
			// 
			// _artPictureBox
			// 
			this._artPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._artPictureBox.Location = new System.Drawing.Point(392, 168);
			this._artPictureBox.Name = "_artPictureBox";
			this._artPictureBox.Size = new System.Drawing.Size(140, 140);
			this._artPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._artPictureBox.TabIndex = 12;
			this._artPictureBox.TabStop = false;
			// 
			// _comboBoxGenere
			// 
			this._comboBoxGenere.Location = new System.Drawing.Point(72, 136);
			this._comboBoxGenere.Name = "_comboBoxGenere";
			this._comboBoxGenere.Size = new System.Drawing.Size(184, 21);
			this._comboBoxGenere.TabIndex = 11;
			// 
			// _labelGenere
			// 
			this._labelGenere.Location = new System.Drawing.Point(8, 136);
			this._labelGenere.Name = "_labelGenere";
			this._labelGenere.Size = new System.Drawing.Size(56, 16);
			this._labelGenere.TabIndex = 10;
			this._labelGenere.Text = "Genere:";
			this._labelGenere.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _textBoxYear
			// 
			this._textBoxYear.Location = new System.Drawing.Point(312, 136);
			this._textBoxYear.Name = "_textBoxYear";
			this._textBoxYear.Size = new System.Drawing.Size(48, 20);
			this._textBoxYear.TabIndex = 9;
			this._textBoxYear.Text = "";
			// 
			// _labelYear
			// 
			this._labelYear.Location = new System.Drawing.Point(272, 136);
			this._labelYear.Name = "_labelYear";
			this._labelYear.Size = new System.Drawing.Size(32, 16);
			this._labelYear.TabIndex = 8;
			this._labelYear.Text = "Year:";
			this._labelYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelAlbum
			// 
			this._labelAlbum.Location = new System.Drawing.Point(8, 104);
			this._labelAlbum.Name = "_labelAlbum";
			this._labelAlbum.Size = new System.Drawing.Size(56, 16);
			this._labelAlbum.TabIndex = 7;
			this._labelAlbum.Text = "Album:";
			this._labelAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelArtist
			// 
			this._labelArtist.Location = new System.Drawing.Point(8, 72);
			this._labelArtist.Name = "_labelArtist";
			this._labelArtist.Size = new System.Drawing.Size(56, 16);
			this._labelArtist.TabIndex = 6;
			this._labelArtist.Text = "Artist:";
			this._labelArtist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _textBoxAlbum
			// 
			this._textBoxAlbum.Location = new System.Drawing.Point(72, 104);
			this._textBoxAlbum.Name = "_textBoxAlbum";
			this._textBoxAlbum.Size = new System.Drawing.Size(448, 20);
			this._textBoxAlbum.TabIndex = 5;
			this._textBoxAlbum.Text = "";
			// 
			// _textBoxArtist
			// 
			this._textBoxArtist.Location = new System.Drawing.Point(72, 72);
			this._textBoxArtist.Name = "_textBoxArtist";
			this._textBoxArtist.Size = new System.Drawing.Size(448, 20);
			this._textBoxArtist.TabIndex = 4;
			this._textBoxArtist.Text = "";
			// 
			// _textBoxTrackNo
			// 
			this._textBoxTrackNo.Location = new System.Drawing.Point(480, 8);
			this._textBoxTrackNo.Name = "_textBoxTrackNo";
			this._textBoxTrackNo.Size = new System.Drawing.Size(40, 20);
			this._textBoxTrackNo.TabIndex = 3;
			this._textBoxTrackNo.Text = "";
			// 
			// _labelTrackNo
			// 
			this._labelTrackNo.Location = new System.Drawing.Point(424, 8);
			this._labelTrackNo.Name = "_labelTrackNo";
			this._labelTrackNo.Size = new System.Drawing.Size(48, 16);
			this._labelTrackNo.TabIndex = 2;
			this._labelTrackNo.Text = "Track #:";
			this._labelTrackNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _textBoxTitle
			// 
			this._textBoxTitle.Location = new System.Drawing.Point(72, 40);
			this._textBoxTitle.Name = "_textBoxTitle";
			this._textBoxTitle.Size = new System.Drawing.Size(448, 20);
			this._textBoxTitle.TabIndex = 1;
			this._textBoxTitle.Text = "";
			// 
			// _labelTitle
			// 
			this._labelTitle.Location = new System.Drawing.Point(8, 40);
			this._labelTitle.Name = "_labelTitle";
			this._labelTitle.Size = new System.Drawing.Size(56, 16);
			this._labelTitle.TabIndex = 0;
			this._labelTitle.Text = "Title:";
			this._labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _tabPageLyrics
			// 
			this._tabPageLyrics.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this._textBoxLyrics});
			this._tabPageLyrics.Location = new System.Drawing.Point(4, 22);
			this._tabPageLyrics.Name = "_tabPageLyrics";
			this._tabPageLyrics.Size = new System.Drawing.Size(544, 318);
			this._tabPageLyrics.TabIndex = 1;
			this._tabPageLyrics.Text = "Lyrics";
			// 
			// _textBoxLyrics
			// 
			this._textBoxLyrics.AcceptsReturn = true;
			this._textBoxLyrics.AcceptsTab = true;
			this._textBoxLyrics.Location = new System.Drawing.Point(16, 16);
			this._textBoxLyrics.Multiline = true;
			this._textBoxLyrics.Name = "_textBoxLyrics";
			this._textBoxLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textBoxLyrics.Size = new System.Drawing.Size(512, 288);
			this._textBoxLyrics.TabIndex = 0;
			this._textBoxLyrics.Text = "";
			// 
			// _tabPageComments
			// 
			this._tabPageComments.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this._textBoxComments});
			this._tabPageComments.Location = new System.Drawing.Point(4, 22);
			this._tabPageComments.Name = "_tabPageComments";
			this._tabPageComments.Size = new System.Drawing.Size(544, 318);
			this._tabPageComments.TabIndex = 3;
			this._tabPageComments.Text = "Comments";
			// 
			// _textBoxComments
			// 
			this._textBoxComments.AcceptsReturn = true;
			this._textBoxComments.AcceptsTab = true;
			this._textBoxComments.Location = new System.Drawing.Point(16, 15);
			this._textBoxComments.Multiline = true;
			this._textBoxComments.Name = "_textBoxComments";
			this._textBoxComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textBoxComments.Size = new System.Drawing.Size(512, 288);
			this._textBoxComments.TabIndex = 1;
			this._textBoxComments.Text = "";
			// 
			// _tabPageMore
			// 
			this._tabPageMore.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this._labelComposer,
																					   this._textBoxComposer});
			this._tabPageMore.Location = new System.Drawing.Point(4, 22);
			this._tabPageMore.Name = "_tabPageMore";
			this._tabPageMore.Size = new System.Drawing.Size(544, 318);
			this._tabPageMore.TabIndex = 2;
			this._tabPageMore.Text = "More";
			// 
			// _labelComposer
			// 
			this._labelComposer.Location = new System.Drawing.Point(8, 8);
			this._labelComposer.Name = "_labelComposer";
			this._labelComposer.Size = new System.Drawing.Size(66, 16);
			this._labelComposer.TabIndex = 15;
			this._labelComposer.Text = "Composer:";
			this._labelComposer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _textBoxComposer
			// 
			this._textBoxComposer.Location = new System.Drawing.Point(80, 8);
			this._textBoxComposer.Name = "_textBoxComposer";
			this._textBoxComposer.Size = new System.Drawing.Size(448, 20);
			this._textBoxComposer.TabIndex = 14;
			this._textBoxComposer.Text = "";
			// 
			// _errorProvider
			// 
			this._errorProvider.DataMember = null;
			// 
			// _buttonOK
			// 
			this._buttonOK.Location = new System.Drawing.Point(200, 360);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(72, 24);
			this._buttonOK.TabIndex = 1;
			this._buttonOK.Text = "OK";
			this._buttonOK.Click += new System.EventHandler(this.OnOkClick);
			// 
			// _buttonCancel
			// 
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(280, 360);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(72, 24);
			this._buttonCancel.TabIndex = 2;
			this._buttonCancel.Text = "&Cancel";
			this._buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// ID3Edit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(568, 389);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._buttonCancel,
																		  this._buttonOK,
																		  this._tabControlLyrics});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ID3Edit";
			this.ShowInTaskbar = false;
			this.Text = "ID3 Tag Editor";
			this.Load += new System.EventHandler(this.ID3Edit_Load);
			this._tabControlLyrics.ResumeLayout(false);
			this._tabPageGeneric.ResumeLayout(false);
			this._tabPageLyrics.ResumeLayout(false);
			this._tabPageComments.ResumeLayout(false);
			this._tabPageMore.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ID3Edit_Load(object sender, System.EventArgs e)
		{
			// If there is no model
			if(_tagModel == null)
				throw new ApplicationException("No data to edit on load");

			foreach(FrameBase frameBase in _tagModel.Frames)
			{
				switch(frameBase.Tag)
				{
					case"TIT2":
					{
						this._textBoxTitle.Text = frameBase.ToString();
						this._textBoxTitle.Tag = frameBase;
						break;
					}
					case"TPE1":
					{
						this._textBoxArtist.Text = frameBase.ToString();
						this._textBoxArtist.Tag = frameBase;
						break;
					}
					case"TALB":
					{
						this._textBoxAlbum.Text = frameBase.ToString();
						this._textBoxAlbum.Tag = frameBase;
						break;
					}
					case"TYER":
					{
						this._textBoxYear.Text = frameBase.ToString();
						this._textBoxYear.Tag = frameBase;
						break;
					}
					case"TCOM":
					{
						this._textBoxComposer.Text = frameBase.ToString();
						this._textBoxComposer.Tag = frameBase;
						break;
					}
					case"TCON":
					{
						this._comboBoxGenere.Text = frameBase.ToString();
						this._comboBoxGenere.Tag = frameBase;
						break;
					}
					case "TRCK":
					{
						this._textBoxTrackNo.Text = frameBase.ToString();
						this._textBoxTrackNo.Tag = frameBase;
						break;
					}
					case"USLT":
					{
						FrameFullText lcFrame = (FrameFullText)(frameBase);
						this._textBoxLyrics.Text = lcFrame.Text;
						//TODO: add --> lcFrame.Language;
						this._textBoxLyrics.Tag = frameBase;
						break;
					}
					case"COMM":
					{
						FrameFullText lcFrame = (FrameFullText)(frameBase);
						this._textBoxComments.Text = lcFrame.Text;
						//TODO: add --> lcFrame.Language;
						this._textBoxComments.Tag = frameBase;
						break;
					}
					case"APIC":
					{
						FramePicture picFrame = (FramePicture)frameBase;
						if(picFrame.PictureData != null)
						{
							Stream stream = new MemoryStream(picFrame.PictureData,false);
							this._artPictureBox.Image = Image.FromStream(stream);
							this._artPictureBox.Tag = frameBase;
						}
						break;
					}
				}
			}
		}

		private void UpdateTextBox(TextBox textBox,string frameType)
		{
			FrameBase frameBase;
			if(textBox.Text.Trim() != string.Empty)
			{
				if(textBox.Tag !=null)
				{
					frameBase = (FrameBase)textBox.Tag;
				}
				else
				{
					frameBase = new FrameText(frameType);
					this._tagModel.Frames.Add(frameBase);
				}
				((FrameText)frameBase).Text = textBox.Text;	
			}
			else
			{
				if(textBox.Tag != null)
				{
					this._tagModel.Frames.Remove((FrameBase)textBox.Tag);
				}
			}
		}

		private void OnOkClick(object sender, System.EventArgs e)
		{
			UpdateTextBox(this._textBoxTitle,"TIT2");
			UpdateTextBox(this._textBoxArtist,"TPE1");
			UpdateTextBox(this._textBoxAlbum,"TALB");
			UpdateTextBox(this._textBoxYear,"TYER");
			UpdateTextBox(this._textBoxComposer,"TCOM");
			UpdateTextBox(this._textBoxTrackNo,"TRCK");
			
			FrameBase frameBase;

			if(this._comboBoxGenere.Text.Trim() != string.Empty)
			{
				if(this._comboBoxGenere.Tag !=null)
				{
					frameBase = (FrameBase)this._comboBoxGenere.Tag;
				}
				else
				{
					frameBase = new FrameText("TCON");
					this._tagModel.Frames.Add(frameBase);
				}
				((FrameText)frameBase).Text = this._comboBoxGenere.Text;
				
			}
			else
			{
				if(this._comboBoxGenere.Tag != null)
				{
					this._tagModel.Frames.Remove((FrameBase)this._comboBoxGenere.Tag);
				}
			}

			if(this._textBoxLyrics.Text.Trim() != string.Empty)
			{
				if(this._textBoxLyrics.Tag !=null)
				{
					frameBase = (FrameBase)this._textBoxLyrics.Tag;
				}
				else
				{
					frameBase = new FrameFullText("USLT");
					this._tagModel.Frames.Add(frameBase);
				}
				((FrameFullText)frameBase).Text = this._textBoxLyrics.Text;
			}
			else
			{
				if(this._textBoxLyrics.Tag != null)
				{
					this._tagModel.Frames.Remove((FrameBase)this._textBoxLyrics.Tag);
				}
				this._textBoxLyrics.Tag = null;
			}
			frameBase = null;

			if(this._textBoxComments.Text.Trim() != string.Empty)
			{
				if(this._textBoxComments.Tag !=null)
				{
					frameBase = (FrameBase)this._textBoxComments.Tag;
				}
				else
				{
					frameBase = new FrameFullText("TCOM");
					this._tagModel.Frames.Add(frameBase);
				}
				((FrameFullText)frameBase).Text = this._textBoxComments.Text;
			}
			else
			{
				if(this._textBoxComments.Tag != null)
				{
					this._tagModel.Frames.Remove((FrameBase)this._textBoxComments.Tag);
				}
			}
			frameBase = null;

			if(this._artPictureBox.Image != null)
			{
				FramePicture framePic = null;
				if(this._artPictureBox.Tag != null)
				{
					framePic = (FramePicture)this._artPictureBox.Tag;
				}
				else
				{
					framePic = new FramePicture();
					this._tagModel.Frames.Add(framePic);
				}
				MemoryStream memoryStream = new MemoryStream();
				this._artPictureBox.Image.Save(memoryStream,System.Drawing.Imaging.ImageFormat.Jpeg);
				framePic.PictureData = memoryStream.ToArray();
				framePic.Mime = "image/jpeg";
			}
			else
			{
				if(this._artPictureBox.Tag != null)
				{
					this._tagModel.Frames.Remove((FrameBase)this._artPictureBox.Tag);
				}
			}
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void addPicture_Click(object sender, System.EventArgs e)
		{
			_openFileDialog.Multiselect= false;
			_openFileDialog.CheckFileExists = true;
			_openFileDialog.CheckPathExists = true;
			_openFileDialog.Title = "Select a picture";
			_openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|jpg (*.jpg)|*.jpg|jpeg (*.jpeg)|*.jpeg|gif (*.gif)|*.gif";
			if(_openFileDialog.ShowDialog() == DialogResult.OK)
			{ 
				FileStream stream = null;
				try
				{
					stream = File.Open(_openFileDialog.FileName,FileMode.Open,FileAccess.Read,FileShare.Read);
					byte[] buffer = new Byte[stream.Length];
					stream.Read(buffer,0,buffer.Length);
					if(buffer != null)
					{
						MemoryStream memoryStream = new MemoryStream(buffer,false);
						this._artPictureBox.Image = Image.FromStream(memoryStream);
					}
				}
				catch{}
				finally
				{
					if(stream != null)
					{
						stream.Close();
					}
				}
			}
			
		}

		private void removePicture_Click(object sender, System.EventArgs e)
		{
			this._artPictureBox.Image = null;
		}
	}
}
