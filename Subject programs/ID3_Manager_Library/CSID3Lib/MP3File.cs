using System;
using System.IO;

namespace Id3Lib
{
	/// <summary>
	/// Manage MP3 file ID3v2 tags.
	/// </summary>
	public class MP3File
	{
        public MP3File() { }
        public void MP3FileInitialize() { }
		/// <summary>
		/// Read the ID3v2 tag model.
		/// </summary>
		/// <param name="file">File location</param>
		public FrameModel Read(string file)
		{
            using (Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                 return FrameManager.Deserialize(stream);
            }
		}

		/// <summary>
		/// Write the tag model to the file.
		/// </summary>
		/// <param name="file">File location were tag will be saved</param>
		/// <param name="tagModel">Tag model beeing saved</param>
		public void Write(string file, FrameModel tagModel)
		{
			Stream stream = File.Open(file,FileMode.Open,FileAccess.ReadWrite,FileShare.Read);
            
			int tagSize;
			//Padding and foter can't exist at the same time, I prefer padding over footer.
			//The footer is useful when the tag won't be edited often.
			if(tagModel.Header.Padding == true) 
			{
				tagModel.Header.Footer = false;
			}
			
			if(tagModel.Header.Footer == true)
			{
				// Insert the tag size and header size twice for the hader and footer. 
				tagSize = tagModel.Header.TagSize + tagModel.Header.HeaderSize*2; 
			}
			else
			{
				// Inset the tag size and the header size.
				tagSize = tagModel.Header.TagSize + tagModel.Header.HeaderSize;
			}
			stream.Seek(tagSize,SeekOrigin.Begin);
			MemoryStream memoryStream = new MemoryStream();
			// serialize the frames only first to get the frames real size
            FrameManager.Serialize(tagModel, memoryStream);
			
			// 
			if(memoryStream.Length > tagSize)
			{

			}
		}
	}
}
