﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WaveletLibrary
{
    public class WaveletMatrix : Matrix
    {
		public WaveletMatrix(int noRows, int noCols) : base(noRows, noCols) {}
		public WaveletMatrix(int[] data) : base(data) {}

        int _row = -1;
        int _col = -1;

        public void SelectRow(int row)
        {
            _row = row;
            _col = -1;
        }

        public void SelectCol(int col)
        {
            _col = col;
            _row = -1;
        }

        public int GetSelecedVectorLength()
        {
            if (_row != -1)
                return NoCols;
            else if (_col != -1)
                return NoRows;
            else
                throw new RankException("No column or row has been selected.");
        }

        public int GetVectorElement(int index)
        {
            if (_row != -1)
                return in_Mat[_row*colNum+ index];
            else if (_col != -1)
                return in_Mat[index * colNum + _col];
            else
                throw new RankException("No column or row has been selected.");
        }

        public void SetVectorElement(int index, int value)
        {
            if (_row != -1)
                in_Mat[_row * colNum + index] = value;
            else if (_col != -1)
                in_Mat[index * colNum + _col] = value;
            else
                throw new RankException("No column or row has been selected.");
        }
        public WaveletMatrix()
        {
        }
        public void WaveletMatrixInitialize(int _row, int _col, int[] in_Mat)
        {
            this._row = _row;
            this._col = _col;
            this.in_Mat = in_Mat;
        }
    }
}
