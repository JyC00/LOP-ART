namespace Id3Lib
{
	using System;
	/// <summary>
	/// The exception is thrown when the tag is corrupt.
	/// </summary>
	class InvalidTagException: Exception
	{
		public InvalidTagException()
		{
		}
	
		public InvalidTagException(string message): base(message)
		{
		}

		public InvalidTagException(string message, Exception inner): base(message, inner)
		{
		}
	}

}