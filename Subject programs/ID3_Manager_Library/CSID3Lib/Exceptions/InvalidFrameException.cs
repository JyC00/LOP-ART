namespace Id3Lib
{
	using System;

	/// <summary>
	/// The exception is thrown when a frame is corrupt.
	/// </summary>
	class InvalidFrameException: Exception
	{
		public InvalidFrameException()
		{
		}
	
		public InvalidFrameException(string message): base(message)
		{
		}

		public InvalidFrameException(string message, Exception inner): base(message, inner)
		{
		}
	}

}