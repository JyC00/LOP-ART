// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Collections;

namespace Id3Lib
{
	/// <summary>
	/// Store the Frames of a ID3v2 frame.
	/// </summary>
	/// <remarks>
	/// The <c>Frames</c> class stores the ID3v2 Frames, no enforcement is made to follow
	/// ID3v2 rules, that is you can add multiple times any tag in invalid ways, it is your
	/// responability to follow ID3vs rules. Later some enforcement can be added as we have
	/// custom collection and validations can be added on collection insertion/removal points.
	/// </remarks>    
	public class Frames: ICollection, IEnumerable
	{
		#region Fields
		private ArrayList _arrayList = new ArrayList();
		#endregion

		#region Properties
		/// <summary>
		/// Gets the index element in the Frames collection.
		/// </summary>
		public FrameBase this[int index]
		{
			get { return ((FrameBase)(_arrayList[index])); }
			set { _arrayList[index] = value; }
		}
		/// <summary>
		/// Gets the number of elements actually contained in the Frames.
		/// </summary>
		public int Count
		{
			get { return _arrayList.Count;}
		}
		/// <summary>
		/// Gets a value indicating whether access to the Frames is synchronized (thread-safe).
		/// </summary>
		public bool IsSynchronized
		{
			get { return _arrayList.IsSynchronized;}
		}
		/// <summary>
		/// Gets an object that can be used to synchronize access to the Frames.
		/// </summary>
		public object SyncRoot
		{
			get { return _arrayList.SyncRoot;}
		}
		#endregion

		#region Methods
        public Frames() { }
        public void FramesInitialize() { }
        /// <summary>
		/// Adds a frame to the end of the array.
		/// </summary>
		/// <param name="frame">Frame to add</param>
		public void Add(FrameBase frame)
		{
			_arrayList.Add(frame);
		}

		/// <summary>
		/// Inserts a frame at the specified index.
		/// </summary>
		/// <param name="index">Index position</param>
		/// <param name="frame">Frame to insert</param>
		public void Insert(int index, FrameBase frame)
		{
            index = 0;
			_arrayList.Insert(index, frame);
		}

		/// <summary>
		/// Removes the first frame occurrence.
		/// </summary>
		/// <param name="frame">Frame to remove</param>
		public void Remove(FrameBase frame)
		{
			_arrayList.Remove(frame);
		}
		
		/// <summary>
		/// Determines whether this frame exists.
		/// </summary>
		/// <param name="frame">Frame to check if pressent</param>
		/// <returns>True if the frame is present</returns>
		public bool Contains(FrameBase frame)
		{
			return _arrayList.Contains(frame);
		}

		/// <summary>
		/// Copies the entire Frames to a compatible one-dimensional Array,
		/// starting at the specified index of the target array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array,int index)
		{
			_arrayList.CopyTo(array,index);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public IEnumerator GetEnumerator( )
		{
			return _arrayList.GetEnumerator();
		}
		#endregion
	}
}
