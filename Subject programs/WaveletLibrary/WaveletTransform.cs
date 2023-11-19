using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveletLibrary
{
    public class WaveletTransform
    {
        private HaarLift _lifter;
        private int _levels;

        public WaveletTransform(HaarLift waveletLifter, int levels)
        {
            _lifter = waveletLifter;
            _levels = levels;
        }

        public WaveletMatrix DoForward(WaveletMatrix data)
        {
            var data2 = Enlarge(data, _levels); //add padding

            for (var level = 1; level <= _levels; level++)
            {
                Console.WriteLine(string.Format("Level = {0}", level)); 
                TransformRows(data2, level, true);
                TransformCols(data2, level, true);
            }

            //Shrink(data2, data); // remove padding
            return data2;
        }

        public void Shrink(WaveletMatrix larger, WaveletMatrix smaller)
        {
            Console.WriteLine(string.Format("Removing padding..."));
            for (int j = 0; j < smaller.NoCols; j++)
                for (int i = 0; i < smaller.NoRows; i++)
                    smaller[i, j] = larger[i, j];
        }

        public WaveletMatrix Enlarge(WaveletMatrix data, int levels)
        {
            var extraRows = 0;
            var extraCols = 0;
            while (((data.NoRows + extraRows) >> levels) << levels != (data.NoRows + extraRows))
                extraRows++;
            while (((data.NoCols + extraCols) >> levels) << levels != (data.NoCols + extraCols))
                extraCols++;

            Console.WriteLine(string.Format("Padding for {0} level(s), PadRows = {1}, PadCols = {2}", levels, extraRows, extraCols));

            var result = new WaveletMatrix(data.NoRows + extraRows, data.NoCols + extraCols);
            for (int j = 0; j < data.NoCols; j++)
                for (int i = 0; i < data.NoRows; i++)
                    result[i, j] = data[i, j];

            return result;
        }

        public void TransformCols(WaveletMatrix data, int level, bool direction)
        {
            int n = data.NoCols / (int)Math.Pow(2, level - 1);
            for (int i = 0; i < n; i++)
            {
                //Console.WriteLine(string.Format("Level = {0}, Col = {1}", level, i));
                data.SelectCol(i);
                if (direction == true)
                    _lifter.ForwardTrans(data, level);
                else if (direction == false)
                    _lifter.InverseTrans(data, level);
                else
                    throw new ArgumentException("Direction is not valid.");
            }
        }

        public void TransformRows(WaveletMatrix data, int level, bool direction)
        {
            int n = data.NoRows / (int)Math.Pow(2, level - 1);
            for (int i = 0; i < n; i++)
            {
                //Console.WriteLine(String.Format("Level = {0}, Row = {1}", level, i));
                data.SelectRow(i);
                if (direction == true)
                    _lifter.ForwardTrans(data, level);
                else if (direction == false)
                    _lifter.InverseTrans(data, level);
                else
                    throw new ArgumentException("Direction is not valid.");
            }
        }

        public void DoInverse(WaveletMatrix data)
        {
            for (var level = 0; level < _levels; level++)
            {
                TransformRows(data, level, false);
                TransformCols(data, level, false);
            }
        }
        public WaveletTransform()
        {
        }
        public void WaveletTransformInitialize(int _levels, HaarLift _lifter)
        {
            this._levels = _levels;
            this._lifter = _lifter;            
        }
    }
}
