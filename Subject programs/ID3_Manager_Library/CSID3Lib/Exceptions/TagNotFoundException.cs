namespace Id3Lib
{
	using System;

	/// <summary>
	/// The exception is thrown when the tag is missing.
	/// </summary>
	class TagNotFoundException: Exception
	{
		public TagNotFoundException()
		{
		}
	
		public TagNotFoundException(string message): base(message)
		{
		}

		public TagNotFoundException(string message, Exception inner): base(message, inner)
		{
		}
	}

}