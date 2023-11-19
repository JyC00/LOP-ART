// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Id3Lib;

namespace TagEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu _mainMenu;
		private System.Windows.Forms.MenuItem _mainMenuItem;
		private System.Windows.Forms.MenuItem _scanMenuItem;
		private System.Windows.Forms.ListBox _mainListBox;
		private System.Windows.Forms.ContextMenu _listBoxContextMenu;
		private System.Windows.Forms.MenuItem _editListBoxMenuItem;
		private System.Windows.Forms.MenuItem _advancedEditListBoxMenuItem;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this._mainMenu = new System.Windows.Forms.MainMenu();
			this._mainMenuItem = new System.Windows.Forms.MenuItem();
			this._scanMenuItem = new System.Windows.Forms.MenuItem();
			this._mainListBox = new System.Windows.Forms.ListBox();
			this._listBoxContextMenu = new System.Windows.Forms.ContextMenu();
			this._editListBoxMenuItem = new System.Windows.Forms.MenuItem();
			this._advancedEditListBoxMenuItem = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// _mainMenu
			// 
			this._mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._mainMenuItem});
			// 
			// _mainMenuItem
			// 
			this._mainMenuItem.Index = 0;
			this._mainMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this._scanMenuItem});
			this._mainMenuItem.Text = "Main";
			// 
			// _scanMenuItem
			// 
			this._scanMenuItem.Index = 0;
			this._scanMenuItem.Text = "Scan Directory";
			this._scanMenuItem.Click += new System.EventHandler(this._scanMenuItem_Click);
			// 
			// _mainListBox
			// 
			this._mainListBox.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._mainListBox.ContextMenu = this._listBoxContextMenu;
			this._mainListBox.Location = new System.Drawing.Point(8, 8);
			this._mainListBox.Name = "_mainListBox";
			this._mainListBox.Size = new System.Drawing.Size(656, 381);
			this._mainListBox.TabIndex = 0;
			this._mainListBox.DoubleClick += new System.EventHandler(this._mainListBox_DoubleClick);
			// 
			// _listBoxContextMenu
			// 
			this._listBoxContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this._editListBoxMenuItem,
																								this._advancedEditListBoxMenuItem});
			// 
			// _editListBoxMenuItem
			// 
			this._editListBoxMenuItem.Index = 0;
			this._editListBoxMenuItem.Text = "Edit";
			this._editListBoxMenuItem.Click += new System.EventHandler(this._mainListBoxMenu_EditTag);
			// 
			// _advancedEditListBoxMenuItem
			// 
			this._advancedEditListBoxMenuItem.Index = 1;
			this._advancedEditListBoxMenuItem.Text = "Advanced Edit";
			this._advancedEditListBoxMenuItem.Click += new System.EventHandler(this._mainListBoxMenu_EditExtendedTag);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(672, 397);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._mainListBox});
			this.Menu = this._mainMenu;
			this.Name = "MainForm";
			this.Text = "ID3 Editor";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}


		private void _scanMenuItem_Click(object sender, System.EventArgs e)
		{
			DirBrowser dirBrowser = new DirBrowser();
			if( dirBrowser.ShowDialog() == DialogResult.OK)
			{
				DirScan browse = new DirScan();
				string[] files = browse.Browse(dirBrowser.DirectoryPath);
				_mainListBox.Items.Clear();
				_mainListBox.Items.AddRange(files);
			}
		}

		private void _mainListBoxMenu_EditTag(object sender, System.EventArgs e)
		{
			if(_mainListBox.SelectedIndex != -1)
			{
				EditTag((string)_mainListBox.Items[_mainListBox.SelectedIndex]);
			}
		}

		private void _mainListBoxMenu_EditExtendedTag(object sender, System.EventArgs e)
		{
			if(_mainListBox.SelectedIndex != -1)
			{
				string file =(string)_mainListBox.Items[_mainListBox.SelectedIndex];
				FileStream stream = File.Open(file,FileMode.Open,FileAccess.Read,FileShare.Read);
				FrameModel tagModel = null;
				ID3PowerEdit id3PowerEdit = new ID3PowerEdit();
				try
				{
                    tagModel = FrameManager.Deserialize(stream);
					id3PowerEdit.TagModel = tagModel;
				}
				catch(Exception)
				{
					try
					{
						ID3v1 id3v1 = new ID3v1();
						id3v1.Deserialize(stream);
						tagModel = id3v1.TagModel;
					}
					catch{}
				}
				finally
				{
					stream.Close();
				}
				id3PowerEdit.ShowDialog();
			}
		}	

		private void _mainListBox_DoubleClick(object sender, System.EventArgs e)
		{
			
			EditTag((string)_mainListBox.Items[_mainListBox.SelectedIndex]);
		}	
		
		void EditTag(string file)
		{
			FileStream stream = File.Open(file,FileMode.Open,FileAccess.Read,FileShare.Read);
			FrameModel tagModel = null;
			ID3Edit id3Edit = new ID3Edit();
			try
			{
                tagModel = FrameManager.Deserialize(stream);
			}
			catch(Exception)
			{
				try
				{
					ID3v1 id3v1 = new ID3v1();
					id3v1.Deserialize(stream);
					tagModel = id3v1.TagModel;
				}
				catch{}
			}
			finally
			{
				stream.Close();
			}

            id3Edit.TagModel = tagModel;

			if(id3Edit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				try
				{
					stream = File.Open(file,FileMode.Open,FileAccess.ReadWrite,FileShare.Read);
					tagModel = id3Edit.TagModel;
							
					int tagSize;
					if(tagModel.Header.Footer == true)
					{
						tagSize = tagModel.Header.TagSize + 20;
					}
					else
					{
						tagSize = tagModel.Header.TagSize + 10;
					}
					stream.Seek(tagSize,SeekOrigin.Begin);
					
					FileStream writeStream = File.Open(@"c:\Test.mp3",FileMode.Create,FileAccess.ReadWrite,FileShare.Read);
                    FrameManager.Serialize(tagModel, writeStream);

					//Copy mp3 stream
					const int size = 4096;
					byte[] bytes = new byte[4096];
					int numBytes;
					while((numBytes = stream.Read(bytes, 0, size)) > 0)
						writeStream.Write(bytes, 0, numBytes);
					writeStream.Close();
				
				}
				catch(Exception e)
				{
					MessageBox.Show("Error Writing Tag: " + e.Message);
				}
				finally
				{
					stream.Close();
				}
			}
		}											
	}
}
