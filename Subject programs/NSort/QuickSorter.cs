using System;
using System.Collections;
using System.IO;


namespace NSort
{
	/// <summary>
	/// http://www.codeproject.com/csharp/csquicksort.asp
	/// </summary>
	public class QuickSorter : SwapSorter
	{
		public QuickSorter()
			:base()
		{}

		public QuickSorter(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{}

		/// <summary>
		/// Sorts the array.
		/// </summary>
		public override void Sort()
		{
            Sort(list, 0, list.Count - 1);
		}
        private IList list;
        public void QuickSorterInitialize(IList list)
        {
            this.list = list;
        }
		public void Sort(IList array, int lower, int upper)
		{
			// Check for non-base case
			if (lower < upper)
			{
				// Split and sort partitions
				int split=Pivot(array, lower, upper);
				Sort(array, lower, split-1);
				Sort(array, split+1, upper);
			}
		}

		#region Internal
		internal int Pivot(IList array, int lower, int upper)
		{
			// Pivot with first element
			int left=lower+1;
			object pivot=array[lower];
			int right=upper;

			// Partition array elements
			while (left <= right)
			{
				// Find item out of place
				while ( (left <= right) && (Comparer.Compare(array[left], pivot) <= 0) )
				{
					++left;
				}

				while ( (left <= right) && (Comparer.Compare(array[right], pivot) > 0) )
				{
					--right;
				}

				// Swap values if necessary
				if (left < right)
				{
					Swapper.Swap(array, left, right);
					++left;
					--right;
				}
			}

			// Move pivot element
			Swapper.Swap(array, lower, right);
			return right;
		}
		#endregion
	}
}
