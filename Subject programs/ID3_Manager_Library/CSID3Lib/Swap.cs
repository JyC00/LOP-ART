// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;

namespace Id3Lib
{
	/// <summary>
	/// Performs byte swaping.
	/// </summary>
	internal class Swap
	{
		#region Methods
		public static unsafe int Int32(int val)
		{
			byte* pVal = (byte*)&val;
			byte swap = pVal[3];
			pVal[3] = pVal[0];
			pVal[0] = swap;
			swap = pVal[2];
			pVal[2] = pVal[1];
			pVal[1] = swap;
			return val;
		}

		public static unsafe uint UInt32(uint val)
		{
			byte* pVal = (byte*)&val;
			byte swap = pVal[3];
			pVal[3] = pVal[0];
			pVal[0] = swap;
			swap = pVal[2];
			pVal[2] = pVal[1];
			pVal[1] = swap;
			return val;
		}

		public static unsafe short Int16(short val)
		{
			byte* pVal = (byte*)&val;
			byte swap = pVal[1];
			pVal[1] = pVal[0];
			pVal[0] = swap;
			return val;
		}

		public static unsafe ushort UInt16(ushort val)
		{
			byte* pVal = (byte*)&val;
			byte swap = pVal[1];
			pVal[1] = pVal[0];
			pVal[0] = swap;
			return val;
		}
		#endregion
	}
}
