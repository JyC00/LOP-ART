using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveletLibrary
{
    public class HaarLift : BaseLift
    {        
        public override void Predict(WaveletMatrix data, int N, bool direction)
        {
            int half = N >> 1;

            for (int i = 0; i < half; i++)
            {
                int predictVal = data.GetVectorElement(i);
                int j = i + half;

                if (direction == true)
                {
                    data.SetVectorElement(j, data.GetVectorElement(j) - predictVal);
                }
                else if (direction == false)
                {
                    data.SetVectorElement(j, data.GetVectorElement(j) + predictVal);
                }
                else
                {
                    throw new ArgumentException("Direction is not valid.");
                }
            }

        }

        public override void Update(WaveletMatrix data, int N, bool direction)
        {
            int half = N >> 1;

            for (int i = 0; i < half; i++)
            {
                int j = i + half;
                int updateVal = data.GetVectorElement(j) / 2;

                if (direction == true)
                {
                    data.SetVectorElement(i, data.GetVectorElement(i) + updateVal);
                }
                else if (direction == false)
                {
                    data.SetVectorElement(i, data.GetVectorElement(i) - updateVal);
                }
                else
                {
                    throw new ArgumentException("Direction is not valid.");
                }
            }
        }
        public HaarLift()
        {
        }
        public void HaarLiftInitialize()
        {
        }
    }
}
