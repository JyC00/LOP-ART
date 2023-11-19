//------------------------------------------------------------------------
//
// Author      : Anas Abidi
// Date        : 18 Dec 2004
// Version     : 2.0
// Description : Matrix Operations Library
//
//------------------------------------------------------------------------	
using System;

namespace WaveletLibrary
{
    #region "Exception in the Library"
    class MatrixLibraryExceptions : ApplicationException
    { public MatrixLibraryExceptions(string message) : base(message) { } }

    // The Exceptions in this Class
    class MatrixNullException : ApplicationException
    {
        public MatrixNullException() :
            base("To do this operation, matrix can not be null") { }
    }
    class MatrixDimensionException : ApplicationException
    {
        public MatrixDimensionException() :
            base("Dimension of the two matrices not suitable for this operation !") { }
    }
    class MatrixNotSquare : ApplicationException
    {
        public MatrixNotSquare() :
            base("To do this operation, matrix must be a square matrix !") { }
    }
    class MatrixDeterminentZero : ApplicationException
    {
        public MatrixDeterminentZero() :
            base("Determinent of matrix equals zero, inverse can't be found !") { }
    }
    class VectorDimensionException : ApplicationException
    {
        public VectorDimensionException() :
            base("Dimension of matrix must be [3 , 1] to do this operation !") { }
    }
    class MatrixSingularException : ApplicationException
    {
        public MatrixSingularException() :
            base("Matrix is singular this operation cannot continue !") { }
    }
    #endregion

    /// <summary>
    /// Matrix Library .Net v2.0 By Anas Abidi, 2004.
    /// 
    /// The Matrix Library contains Class Matrix which provides many 
    /// static methods for making various matrix operations on objects
    /// derived from the class or on arrays defined as int. The 
    /// '+','-','*' operators are overloaded to work with the objects
    /// derived from the matrix class.
    /// </summary>
    public class Matrix
    {
        protected int[] in_Mat = new int[rowNum * colNum];
        protected const int rowNum = 10;
        protected const int colNum = 10;

        #region "Class Constructor and Indexer"
        /// <summary>
        /// Matrix object constructor, constructs an empty
        /// matrix with dimensions: rows = noRows and cols = noCols.
        /// </summary>
        /// <param name="noRows"> no. of rows in this matrix </param>
        /// <param name="noCols"> no. of columns in this matrix</param>
        public Matrix(int noRows, int noCols)
        {
            this.in_Mat = new int[noRows * noCols];
        }

        /// <summary>
        /// Matrix object constructor, constructs a matrix from an
        /// already defined array object.
        /// </summary>
        /// <param name="Mat">the array the matrix will contain</param>
        public Matrix(int[] Mat)
        {
            this.in_Mat = (int[])Mat.Clone();
        }

        /// <summary>
        /// Set or get an element from the matrix
        /// </summary>
        public int this[int Row, int Col]
        {
            get { return this.in_Mat[Row * colNum + Col]; }
            set { this.in_Mat[Row * colNum + Col] = value; }
        }
        #endregion

        #region "Public Properties"
        /// <summary>
        /// Set or get the no. of rows in the matrix.
        /// Warning: Setting this property will delete all of
        /// the elements of the matrix and set them to zero.
        /// </summary>
        public int NoRows
        {
            get { return rowNum; }
        }

        /// <summary>
        /// Set or get the no. of columns in the matrix.
        /// Warning: Setting this property will delete all of
        /// the elements of the matrix and set them to zero.
        /// </summary>
        public int NoCols
        {
            get { return colNum; }
        }

        /// <summary>
        /// This property returns the matrix as an array.
        /// </summary>
        public int[] toArray
        {
            get { return this.in_Mat; }
        }
        #endregion

        #region "Find Rows and Columns in a Matrix"
        public int Find_R_C(int[] Mat)
        {
            return rowNum - 1;
        }

        private void Find_R_C(int[] Mat, out int Row, out int Col)
        {
            Row = rowNum - 1;
            Col = colNum - 1;
        }
        #endregion

        #region "Change 1D array ([n]) to/from 2D array [n,1]"

        /// <summary>
        /// Returns the 2D form of a 1D array. i.e. array with
        /// dimension[n] is returned as an array with dimension [n,1]. 
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat"> 
        /// the array to convert, with dimesion [n] 
        /// </param>
        /// <returns> the same array but with [n,1] dimension </returns>
        public int[] OneD_2_TwoD(int[] Mat)
        {
            int Rows;
            //Find The dimensions !!
            try { Rows = Find_R_C(Mat); }
            catch { throw new MatrixNullException(); }

            int[] Sol = new int[rowNum * 1];

            for (int i = 0; i < rowNum * 1; i++)
            {
                Sol[i] = Mat[i];
            }

            return Sol;
        }

        /// <summary>
        /// Returns the 1D form of a 2D array. i.e. array with
        /// dimension[n,1] is returned as an array with dimension [n]. 
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">
        /// the array to convert, with dimesions [n,1] 
        /// </param>
        /// <returns>the same array but with [n] dimension</returns>
        public int[] TwoD_2_OneD(int[] Mat)
        {
            int Rows;
            int Cols;
            //Find The dimensions !!
            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new MatrixNullException(); }

            if (Cols != 0) throw new MatrixDimensionException();

            int[] Sol = new int[Rows + 1];

            for (int i = 0; i <= Rows; i++)
            {
                Sol[i] = Mat[i];
            }
            return Sol;
        }
        #endregion

        #region "Identity Matrix"
        /// <summary>
        /// Returns an Identity matrix with dimensions [n,n] in the from of an array.
        /// </summary>
        /// <param name="n">the no. of rows or no. cols in the matrix</param>
        /// <returns>An identity Matrix with dimensions [n,n] in the form of an array</returns>
        public int[] Identity(int n)
        {
            int[] temp = new int[n * n];
            for (int i = 0; i < n; i++) temp[i * n + i] = 1;
            return temp;
        }
        #endregion

        #region "Add Matrices"
        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in the summation</param>
        /// <param name="Mat2">Second array in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as an array</returns>
        public int[] Add(int[] Mat1, int[] Mat2)
        {
            int[] sol;
            int i, j;
            int Rows1, Cols1;
            int Rows2, Cols2;

            //Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new MatrixNullException();
            }

            if (Rows1 != Rows2 || Cols1 != Cols2) throw new MatrixDimensionException();

            sol = new int[rowNum * colNum];

            for (i = 0; i < rowNum * colNum; i++)
            {
                sol[i] = Mat1[i] + Mat2[i];
            }
            return sol;
        }

        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First matrix in the summation</param>
        /// <param name="Mat2">Second matrix in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as a Matrix object</returns>
        public Matrix Add(Matrix Mat1, Matrix Mat2)
        { return new Matrix(Add(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix object in the summation</param>
        /// <param name="Mat2">Second Matrix object in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as a Matrix object</returns>
        public static Matrix operator +(Matrix Mat1, Matrix Mat2)
        { return new Matrix(Mat1.Add(Mat1.in_Mat, Mat2.in_Mat)); }
        #endregion

        #region "Subtract Matrices"

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in the subtraction</param>
        /// <param name="Mat2">Second array in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as an array</returns>
        public int[] Subtract(int[] Mat1, int[] Mat2)
        {
            int[] sol;
            int i, j;
            int Rows1, Cols1;
            int Rows2, Cols2;

            ////Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new MatrixNullException();
            }

            if (Rows1 != Rows2 || Cols1 != Cols2) throw new MatrixDimensionException();

            sol = new int[rowNum * colNum];

            for (i = 0; i < rowNum * colNum; i++)
            {
                sol[i] = Mat1[i] - Mat2[i];
            }

            return sol;
        }

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First matrix in the subtraction</param>
        /// <param name="Mat2">Second matrix in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as a Matrix object</returns>
        public Matrix Subtract(Matrix Mat1, Matrix Mat2)
        { return new Matrix(Subtract(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix object in the subtraction</param>
        /// <param name="Mat2">Second Matrix object in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as a Matrix object</returns>
        public static Matrix operator -(Matrix Mat1, Matrix Mat2)
        { return new Matrix(Mat1.Subtract(Mat1.in_Mat, Mat2.in_Mat)); }
        #endregion

        #region "Multiply Matrices"

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">First array in multiplication</param>
        /// <param name="Mat2">Second array in multiplication</param>
        /// <returns>Mat1 multiplied by Mat2 as an array</returns>
        public int[] Multiply(int[] Mat1, int[] Mat2)
        {
            int[] sol;
            int Rows1, Cols1;
            int Rows2, Cols2;
            int MulAdd = 0;

            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (Cols1 != Rows2) throw new MatrixDimensionException();

            sol = new int[rowNum * colNum];

            for (int i = 0; i < rowNum; i++)
                for (int j = 0; j < colNum; j++)
                {
                    for (int l = 0; l < colNum; l++)
                    {
                        MulAdd = MulAdd + Mat1[i * colNum + l] * Mat2[l * colNum + j];
                    }
                    sol[i * colNum + j] = MulAdd;
                    MulAdd = 0;
                }
            return sol;
        }

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions OR the cross-product of two vectors.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">
        /// First matrix or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <param name="Mat2">
        /// Second matrix or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <returns>Mat1 multiplied by Mat2 as a Matrix object</returns>
        public Matrix Multiply(Matrix Mat1, Matrix Mat2)
        {
            if ((Mat1.NoRows == 3) && (Mat2.NoRows == 3) &&
                (Mat1.NoCols == 1) && (Mat1.NoCols == 1))
            { return new Matrix(CrossProduct(Mat1.in_Mat, Mat2.in_Mat)); }
            else
            { return new Matrix(Multiply(Mat1.in_Mat, Mat2.in_Mat)); }
        }

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions OR the cross-product of two vectors.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">
        /// First matrix or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <param name="Mat2">
        /// Second matrix or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <returns>Mat1 multiplied by Mat2 as a Matrix object</returns>
        public static Matrix operator *(Matrix Mat1, Matrix Mat2)
        {
            if ((Mat1.NoRows == 3) && (Mat2.NoRows == 3) &&
                (Mat1.NoCols == 1) && (Mat1.NoCols == 1))
            {
                return new Matrix(Mat1.CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
            }
            else
            {
                return new Matrix(Mat1.Multiply(Mat1.in_Mat, Mat2.in_Mat));
            }
        }
        #endregion

        #region "Determinant of a Matrix"
        /// <summary>
        /// Returns the determinant of a matrix with [n,n] dimension.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Array with [n,n] dimension whose determinant is to be found
        /// </param>
        /// <returns>Determinant of the array</returns>
        public int Det(int[] Mat)
        {
            int S, k, k1, i, j;
            int[] DArray;
            int save, ArrayK, tmpDet;
            int Rows, Cols;

            try
            {
                DArray = (int[])Mat.Clone();
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new MatrixNullException(); }

            if (Rows != Cols) throw new MatrixNotSquare();

            S = Rows;
            tmpDet = 1;

            for (k = 0; k <= S; k++)
            {
                if (DArray[k * colNum + k] == 0)
                {
                    j = k;
                    while ((j < S) && (DArray[k * colNum + j] == 0)) j = j + 1;
                    if (DArray[k * colNum + j] == 0) return 0;
                    else
                    {
                        for (i = k; i <= S; i++)
                        {
                            save = DArray[i * colNum + j];
                            DArray[i * colNum + j] = DArray[i * colNum + k];
                            DArray[i * colNum + k] = save;
                        }
                    }
                    tmpDet = -tmpDet;
                }
                ArrayK = DArray[k * colNum + k];
                tmpDet = tmpDet * ArrayK;
                if (k < S)
                {
                    k1 = k + 1;
                    for (i = k1; i <= S; i++)
                    {
                        for (j = k1; j <= S; j++)
                            DArray[i * colNum + j] = DArray[i * colNum + j] - DArray[i * colNum + k] * (DArray[k * colNum + j] / ArrayK);
                    }
                }
            }
            return tmpDet;
        }

        /// <summary>
        /// Returns the determinant of a matrix with [n,n] dimension.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Matrix object with [n,n] dimension whose determinant is to be found
        /// </param>
        /// <returns>Determinant of the Matrix object</returns>
        public int Det(Matrix Mat)
        { return Det(Mat.in_Mat); }
        #endregion

        #region "Inverse of a Matrix"
        /// <summary>
        /// Returns the inverse of a matrix with [n,n] dimension 
        /// and whose determinant is not zero.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Array with [n,n] dimension whose inverse is to be found
        /// </param>
        /// <returns>Inverse of the array as an array</returns>
        public int[] Inverse(int[] Mat)
        {
            int[] AI, Mat1;
            int AIN, AF;
            int Rows, Cols;
            int LL, LLM, L1, L2, LC, LCA, LCB;

            try
            {
                Find_R_C(Mat, out Rows, out Cols);
                Mat1 = (int[])Mat.Clone();
            }
            catch { throw new MatrixNullException(); }

            if (Rows != Cols) throw new MatrixNotSquare();
            if (Det(Mat) == 0) throw new MatrixDeterminentZero();

            LL = Rows;
            LLM = Cols;
            AI = new int[(LL + 1) * (LL + 1)];

            for (L2 = 0; L2 <= LL; L2++)
            {
                for (L1 = 0; L1 <= LL; L1++) AI[L1 * colNum + L2] = 0;
                AI[L2 * colNum + L2] = 1;
            }

            for (LC = 0; LC <= LL; LC++)
            {
                if (Math.Abs(Mat1[LC * colNum + LC]) < 0.0000000001)
                {
                    for (LCA = LC + 1; LCA <= LL; LCA++)
                    {
                        if (LCA == LC) continue;
                        if (Math.Abs(Mat1[LC * colNum + LCA]) > 0.0000000001)
                        {
                            for (LCB = 0; LCB <= LL; LCB++)
                            {
                                Mat1[LCB * colNum + LC] = Mat1[LCB * colNum + LC] + Mat1[LCB * colNum + LCA];
                                AI[LCB * colNum + LC] = AI[LCB * colNum + LC] + AI[LCB * colNum + LCA];
                            }
                            break;
                        }
                    }
                }
                AIN = 1 / Mat1[LC * colNum + LC];
                for (LCA = 0; LCA <= LL; LCA++)
                {
                    Mat1[LCA * colNum + LC] = AIN * Mat1[LCA * colNum + LC];
                    AI[LCA * colNum + LC] = AIN * AI[LCA * colNum + LC];
                }

                for (LCA = 0; LCA <= LL; LCA++)
                {
                    if (LCA == LC) continue;
                    AF = Mat1[LC * colNum + LCA];
                    for (LCB = 0; LCB <= LL; LCB++)
                    {
                        Mat1[LCB * colNum + LCA] = Mat1[LCB * colNum + LCA] - AF * Mat1[LCB * colNum + LC];
                        AI[LCB * colNum + LCA] = AI[LCB * colNum + LCA] - AF * AI[LCB * colNum + LC];
                    }
                }
            }
            return AI;
        }

        /// <summary>
        /// Returns the inverse of a matrix with [n,n] dimension 
        /// and whose determinant is not zero.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Matrix object with [n,n] dimension whose inverse is to be found
        /// </param>
        /// <returns>Inverse of the matrix as a Matrix object</returns>
        public Matrix Inverse(Matrix Mat)
        { return new Matrix(Inverse(Mat.in_Mat)); }
        #endregion

        #region "Transpose of a Matrix"
        /// <summary>
        /// Returns the transpose of a matrix.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">Array whose transpose is to be found</param>
        /// <returns>Transpose of the array as an array</returns>
        public int[] Transpose(int[] Mat)
        {
            int[] Tr_Mat;
            int i, j, Rows, Cols;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new MatrixNullException(); }

            Tr_Mat = new int[(Cols + 1) * (Rows + 1)];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++) Tr_Mat[j * colNum + i] = Mat[i * colNum + j];

            return Tr_Mat;
        }

        /// <summary>
        /// Returns the transpose of a matrix.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">Matrix object whose transpose is to be found</param>
        /// <returns>Transpose of the Matrix object as a Matrix object</returns>
        public Matrix Transpose(Matrix Mat)
        { return new Matrix(Transpose(Mat.in_Mat)); }
        #endregion

        #region "Singula Value Decomposition of a Matrix"
        /// <summary>
        /// Evaluates the Singular Value Decomposition of a matrix, 
        /// returns the  matrices S, U and V. Such that a given
        /// Matrix = U x S x V'.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'Singular Value Decomposition'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat_">Array whose SVD is to be computed</param>
        /// <param name="S_">An array where the S matrix is returned</param>
        /// <param name="U_">An array where the U matrix is returned</param>
        /// <param name="V_">An array where the V matrix is returned</param>
        public void SVD(int[] Mat_, out int[] S_, out int[] U_, out int[] V_)
        {
            int Rows, Cols;
            int m, MP, n, NP;
            int[] w;
            int[] A, v;

            try { Find_R_C(Mat_, out Rows, out Cols); }
            catch { throw new MatrixNullException(); }

            m = Rows + 1;
            n = Cols + 1;

            if (m < n)
            {
                m = n;
                MP = NP = n;
            }
            else if (m > n)
            {
                n = m;
                NP = MP = m;
            }
            else
            {
                MP = m;
                NP = n;
            }

            A = new int[(m + 1) * (n + 1)];

            for (int row = 1; row <= Rows + 1; row++)
                for (int col = 1; col <= Cols + 1; col++)
                { A[row * colNum + col] = Mat_[(row - 1) * colNum + (col - 1)]; }

            const int NMAX = 100;
            v = new int[(NP + 1) * (NP + 1)];
            w = new int[NP + 1];

            int k, l, nm;
            int flag, i, its, j, jj;

            int[] U_temp, S_temp, V_temp;
            int anorm, c, f, g, h, s, scale, x, y, z;
            int[] rv1 = new int[NMAX];

            l = 0;
            nm = 0;
            g = 0;
            scale = 0;
            anorm = 0;

            for (i = 1; i <= n; i++)
            {
                l = i + 1;
                rv1[i] = scale * g;
                g = s = scale = 0;
                if (i <= m)
                {
                    for (k = i; k <= m; k++) scale += Math.Abs(A[k * colNum + i]);
                    if (scale != 0)
                    {
                        for (k = i; k <= m; k++)
                        {
                            A[k * colNum + i] /= scale;
                            s += A[k * colNum + i] * A[k * colNum + i];
                        }
                        f = A[i * colNum + i];
                        g = -Sign((int)Math.Sqrt(s), f);
                        h = f * g - s;
                        A[i * colNum + i] = f - g;
                        if (i != n)
                        {
                            for (j = l; j <= n; j++)
                            {
                                for (s = 0, k = i; k <= m; k++) s += A[k * colNum + i] * A[k * colNum + j];
                                f = s / h;
                                for (k = i; k <= m; k++) A[k * colNum + j] += f * A[k * colNum + i];
                            }
                        }
                        for (k = i; k <= m; k++) A[k * colNum + i] *= scale;
                    }
                }
                w[i] = scale * g;
                g = s = scale = 0;
                if (i <= m && i != n)
                {
                    for (k = l; k <= n; k++) scale += Math.Abs(A[i * colNum + k]);
                    if (scale != 0)
                    {
                        for (k = l; k <= n; k++)
                        {
                            A[i * colNum + k] /= scale;
                            s += A[i * colNum + k] * A[i * colNum + k];
                        }
                        f = A[i * colNum + l];
                        g = -Sign((int)Math.Sqrt(s), f);
                        h = f * g - s;
                        A[i * colNum + l] = f - g;
                        for (k = l; k <= n; k++) rv1[k] = A[i * colNum + k] / h;
                        if (i != m)
                        {
                            for (j = l; j <= m; j++)
                            {
                                for (s = 0, k = l; k <= n; k++) s += A[j * colNum + k] * A[i * colNum + k];
                                for (k = l; k <= n; k++) A[j * colNum + k] += s * rv1[k];
                            }
                        }
                        for (k = l; k <= n; k++) A[i * colNum + k] *= scale;
                    }
                }
                anorm = Math.Max(anorm, (Math.Abs(w[i]) + Math.Abs(rv1[i])));
            }
            for (i = n; i >= 1; i--)
            {
                if (i < n)
                {
                    if (g != 0)
                    {
                        for (j = l; j <= n; j++)
                            v[j * colNum + i] = (A[i * colNum + j] / A[i * colNum + l]) / g;
                        for (j = l; j <= n; j++)
                        {
                            for (s = 0, k = l; k <= n; k++) s += A[i * colNum + k] * v[k * colNum + j];
                            for (k = l; k <= n; k++) v[k * colNum + j] += s * v[k * colNum + i];
                        }
                    }
                    for (j = l; j <= n; j++) v[i * colNum + j] = v[j * colNum + i] = 0;
                }
                v[i * colNum + i] = 1;
                g = rv1[i];
                l = i;
            }
            for (i = n; i >= 1; i--)
            {
                l = i + 1;
                g = w[i];
                if (i < n)
                    for (j = l; j <= n; j++) A[i * colNum + j] = 0;
                if (g != 0)
                {
                    g = 1 / g;
                    if (i != n)
                    {
                        for (j = l; j <= n; j++)
                        {
                            for (s = 0, k = l; k <= m; k++) s += A[k * colNum + i] * A[k * colNum + j];
                            f = (s / A[i * colNum + i]) * g;
                            for (k = i; k <= m; k++) A[k * colNum + j] += f * A[k * colNum + i];
                        }
                    }
                    for (j = i; j <= m; j++) A[j * colNum + i] *= g;
                }
                else
                {
                    for (j = i; j <= m; j++) A[j * colNum + i] = 0;
                }
                ++A[i * colNum + i];
            }
            for (k = n; k >= 1; k--)
            {
                for (its = 1; its <= 30; its++)
                {
                    flag = 1;
                    for (l = k; l >= 1; l--)
                    {
                        nm = l - 1;
                        if (Math.Abs(rv1[l]) + anorm == anorm)
                        {
                            flag = 0;
                            break;
                        }
                        if (Math.Abs(w[nm]) + anorm == anorm) break;
                    }
                    if (flag != 0)
                    {
                        c = 0;
                        s = 1;
                        for (i = l; i <= k; i++)
                        {
                            f = s * rv1[i];
                            if (Math.Abs(f) + anorm != anorm)
                            {
                                g = w[i];
                                h = (int)PYTHAG(f, g);
                                w[i] = h;
                                h = 1 / h;
                                c = g * h;
                                s = (-f * h);
                                for (j = 1; j <= m; j++)
                                {
                                    y = A[j * colNum + nm];
                                    z = A[j * colNum + i];
                                    A[j * colNum + nm] = y * c + z * s;
                                    A[j * colNum + i] = z * c - y * s;
                                }
                            }
                        }
                    }
                    z = w[k];
                    if (l == k)
                    {
                        if (z < 0.0)
                        {
                            w[k] = -z;
                            for (j = 1; j <= n; j++) v[j * colNum + k] = (-v[j * colNum + k]);
                        }
                        break;
                    }
                    if (its == 30) Console.WriteLine("No convergence in 30 SVDCMP iterations");
                    x = w[l];
                    nm = k - 1;
                    y = w[nm];
                    g = rv1[nm];
                    h = rv1[k];
                    f = (int)(((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y));
                    g = (PYTHAG(f, 1));
                    f = (int)(((x - z) * (x + z) + h * ((y / (f + Sign(g, f))) - h)) / x);
                    c = s = 1;
                    for (j = l; j <= nm; j++)
                    {
                        i = j + 1;
                        g = rv1[i];
                        y = w[i];
                        h = s * g;
                        g = c * g;
                        z = (int)PYTHAG(f, h);
                        rv1[j] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = g * c - x * s;
                        h = y * s;
                        y = y * c;
                        for (jj = 1; jj <= n; jj++)
                        {
                            x = v[jj * colNum + j];
                            z = v[jj * colNum + i];
                            v[jj * colNum + j] = x * c + z * s;
                            v[jj * colNum + i] = z * c - x * s;
                        }
                        z = (int)PYTHAG(f, h);
                        w[j] = z;
                        if (z != 0)
                        {
                            z = (int)(1.0 / z);
                            c = f * z;
                            s = h * z;
                        }
                        f = (c * g) + (s * y);
                        x = (c * y) - (s * g);
                        for (jj = 1; jj <= m; jj++)
                        {
                            y = A[jj * colNum + j];
                            z = A[jj * colNum + i];
                            A[jj * colNum + j] = y * c + z * s;
                            A[jj * colNum + i] = z * c - y * s;
                        }
                    }
                    rv1[l] = 0;
                    rv1[k] = f;
                    w[k] = x;
                }
            }

            S_temp = new int[NP * NP];
            V_temp = new int[NP * NP];
            U_temp = new int[MP * NP];

            for (i = 1; i <= NP; i++) S_temp[(i - 1) * colNum + (i - 1)] = w[i];

            S_ = S_temp;

            for (i = 1; i <= NP; i++)
                for (j = 1; j <= NP; j++) V_temp[(i - 1) * colNum + (j - 1)] = v[i * colNum + j];

            V_ = V_temp;

            for (i = 1; i <= MP; i++)
                for (j = 1; j <= NP; j++) U_temp[(i - 1) * colNum + (j - 1)] = A[i * colNum + j];

            U_ = U_temp;
        }

        public int SQR(int a)
        {
            return a * a;
        }

        public int Sign(int a, int b)
        {
            if (b >= 0.0) { return Math.Abs(a); }
            else { return -Math.Abs(a); }
        }

        public int PYTHAG(int a, int b)
        {
            int absa, absb;

            absa = Math.Abs(a);
            absb = Math.Abs(b);
            if (absa > absb) return (int)(absa * Math.Sqrt(1.0 + SQR(absb / absa)));
            else return (int)(absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + SQR(absa / absb)));
        }

        /// <summary>
        /// Evaluates the Singular Value Decomposition of a matrix, 
        /// returns the  matrices S, U and V. Such that a given
        /// Matrix = U x S x V'.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'Singular Value Decomposition'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">Matrix object whose SVD is to be computed</param>
        /// <param name="S">A Matrix object where the S matrix is returned</param>
        /// <param name="U">A Matrix object where the U matrix is returned</param>
        /// <param name="V">A Matrix object where the V matrix is returned</param>
        public void SVD(Matrix Mat, out Matrix S, out Matrix U, out Matrix V)
        {
            int[] s, u, v;
            SVD(Mat.in_Mat, out s, out u, out v);
            S = new Matrix(s);
            U = new Matrix(u);
            V = new Matrix(v);
        }
        #endregion

        #region "LU Decomposition of a matrix"
        /// <summary>
        /// Returns the LU Decomposition of a matrix. 
        /// the output is: lower triangular matrix L, upper
        /// triangular matrix U, and permutation matrix P so that
        ///	P*X = L*U.
        /// In case of an error the error is raised as an exception.
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat">Array which will be LU Decomposed</param>
        /// <param name="L">An array where the lower traingular matrix is returned</param>
        /// <param name="U">An array where the upper traingular matrix is returned</param>
        /// <param name="P">An array where the permutation matrix is returned</param>
        public void LU(int[] Mat, out int[] L, out int[] U, out int[] P)
        {
            int[] A;
            int i, j, k, Rows, Cols;

            try
            {
                A = (int[])Mat.Clone();
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new MatrixNullException(); }

            if (Rows != Cols) throw new MatrixNotSquare();

            int IMAX = 0, N = Rows;
            int AAMAX, Dum, TINY = (int)1E-20;
            int Sum;
            int[] INDX = new int[N + 1];
            int[] VV = new int[N * 10];
            int D = 1;

            for (i = 0; i <= N; i++)
            {
                AAMAX = 0;
                for (j = 0; j <= N; j++)
                    if (Math.Abs(A[i * colNum + j]) > AAMAX) AAMAX = Math.Abs(A[i * colNum + j]);
                if (AAMAX == 0.0) throw new MatrixSingularException();
                VV[i] = (int)(1.0 / AAMAX);
            }
            for (j = 0; j <= N; j++)
            {
                if (j > 0)
                {
                    for (i = 0; i <= (j - 1); i++)
                    {
                        Sum = A[i * colNum + j];
                        if (i > 0)
                        {
                            for (k = 0; k <= (i - 1); k++)
                                Sum = Sum - A[i * colNum + k] * A[k * colNum + j];
                            A[i * colNum + j] = Sum;
                        }
                    }
                }
                AAMAX = 0;
                for (i = j; i <= N; i++)
                {
                    Sum = A[i * colNum + j];
                    if (j > 0)
                    {
                        for (k = 0; k <= (j - 1); k++)
                            Sum = Sum - A[i * colNum + k] * A[k * colNum + j];
                        A[i * colNum + j] = Sum;
                    }
                    Dum = VV[i] * Math.Abs(Sum);
                    if (Dum >= AAMAX)
                    {
                        IMAX = i;
                        AAMAX = Dum;
                    }
                }
                if (j != IMAX)
                {
                    for (k = 0; k <= N; k++)
                    {
                        Dum = A[IMAX * colNum + k];
                        A[IMAX * colNum + k] = A[j * colNum + k];
                        A[j * colNum + k] = Dum;
                    }
                    D = -D;
                    VV[IMAX] = VV[j];
                }
                INDX[j] = IMAX;
                if (j != N)
                {
                    if (A[j * colNum + j] == 0.0) A[j * colNum + j] = (int)TINY;
                    Dum = (int)(1.0 / A[j * colNum + j]);
                    for (i = j + 1; i <= N; i++)
                        A[i * colNum + j] = A[i * colNum + j] * (int)Dum;

                }
            }

            if (A[N * colNum + N] == 0.0) A[N * colNum + N] = (int)TINY;

            int count = 0;
            int[] l = new int[(N + 1) * (N + 1)];
            int[] u = new int[(N + 1) * (N + 1)];

            for (i = 0; i <= N; i++)
            {
                for (j = 0; j <= count; j++)
                {
                    if (i != 0) l[i * colNum + j] = A[i * colNum + j];
                    if (i == j) l[i * colNum + j] = 1;
                    u[(N - i) * colNum + (N - j)] = A[(N - i) * colNum + (N - j)];
                }
                count++;
            }

            L = l;
            U = u;

            P = Identity(N + 1);
            for (i = 0; i <= N; i++)
            {
                SwapRows(P, i, INDX[i]);
            }
        }

        public void SwapRows(int[] Mat, int Row, int toRow)
        {
            int N = rowNum;
            int[] dumArray = new int[1 * (N + 1)];
            for (int i = 0; i <= N; i++)
            {
                dumArray[0 * colNum + i] = Mat[Row * colNum + i];
                Mat[Row * colNum + i] = Mat[toRow * colNum + i];
                Mat[toRow * colNum + i] = dumArray[0 * colNum + i];
            }
        }

        /// <summary>
        /// Returns the LU Decomposition of a matrix. 
        /// the output is: lower triangular matrix L, upper
        /// triangular matrix U, and permutation matrix P so that
        ///	P*X = L*U.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat">Matrix object which will be LU Decomposed</param>
        /// <param name="L">A Matrix object where the lower traingular matrix is returned</param>
        /// <param name="U">A Matrix object where the upper traingular matrix is returned</param>
        /// <param name="P">A Matrix object where the permutation matrix is returned</param>
        public void LU(Matrix Mat, out Matrix L, out Matrix U, out Matrix P)
        {
            int[] l, u, p;
            LU(Mat.in_Mat, out l, out u, out p);
            L = new Matrix(l);
            U = new Matrix(u);
            P = new Matrix(p);
        }
        #endregion

        #region "Solve system of linear equations"
        /// <summary>
        /// Solves a set of n linear equations A.X = B, and returns
        /// X, where A is [n,n] and B is [n,1]. 
        /// In the same manner if you need to compute: inverse(A).B, it is
        /// better to use this method instead, as it is much faster.  
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="MatA">The array 'A' on the left side of the equations A.X = B</param>
        /// <param name="MatB">The array 'B' on the right side of the equations A.X = B</param>
        /// <returns>Array 'X' in the system of equations A.X = B</returns>
        public int[] SolveLinear(int[] MatA, int[] MatB)
        {
            int[] A;
            int[] B;
            int SUM;
            int i, ii, j, k, ll, Rows, Cols;

            #region "LU Decompose"
            try
            {
                A = (int[])MatA.Clone();
                B = (int[])MatB.Clone();
                Find_R_C(A, out Rows, out Cols);
            }
            catch { throw new MatrixNullException(); }

            if (Rows != Cols) throw new MatrixNotSquare();
            if ((rowNum != Rows) || (colNum != 0))
                throw new MatrixDimensionException();

            int IMAX = 0, N = Rows;
            int AAMAX, Sum, Dum, TINY = (int)1E-20;

            int[] INDX = new int[N + 1];
            int[] VV = new int[N * 10];
            int D = 1;

            for (i = 0; i <= N; i++)
            {
                AAMAX = 0;
                for (j = 0; j <= N; j++)
                    if (Math.Abs(A[i * colNum + j]) > AAMAX) AAMAX = Math.Abs(A[i * colNum + j]);
                if (AAMAX == 0.0) throw new MatrixSingularException();
                VV[i] = (int)(1.0 / AAMAX);
            }
            for (j = 0; j <= N; j++)
            {
                if (j > 0)
                {
                    for (i = 0; i <= (j - 1); i++)
                    {
                        Sum = A[i * colNum + j];
                        if (i > 0)
                        {
                            for (k = 0; k <= (i - 1); k++)
                                Sum = Sum - A[i * colNum + k] * A[k * colNum + j];
                            A[i * colNum + j] = Sum;
                        }
                    }
                }
                AAMAX = 0;
                for (i = j; i <= N; i++)
                {
                    Sum = A[i * colNum + j];
                    if (j > 0)
                    {
                        for (k = 0; k <= (j - 1); k++)
                            Sum = Sum - A[i * colNum + k] * A[k * colNum + j];
                        A[i * colNum + j] = Sum;
                    }
                    Dum = VV[i] * Math.Abs(Sum);
                    if (Dum >= AAMAX)
                    {
                        IMAX = i;
                        AAMAX = Dum;
                    }
                }
                if (j != IMAX)
                {
                    for (k = 0; k <= N; k++)
                    {
                        Dum = A[IMAX * colNum + k];
                        A[IMAX * colNum + k] = A[j * colNum + k];
                        A[j * colNum + k] = Dum;
                    }
                    D = -D;
                    VV[IMAX] = VV[j];
                }
                INDX[j] = IMAX;
                if (j != N)
                {
                    if (A[j * colNum + j] == 0.0) A[j * colNum + j] = TINY;
                    Dum = 1 / A[j * colNum + j];
                    for (i = j + 1; i <= N; i++)
                        A[i * colNum + j] = A[i * colNum + j] * Dum;

                }
            }
            if (A[N * colNum + N] == 0.0) A[N * colNum + N] = TINY;
            #endregion

            ii = -1;
            for (i = 0; i <= N; i++)
            {
                ll = INDX[i];
                SUM = B[ll * colNum + 0];
                B[ll * colNum + 0] = B[i * colNum + 0];
                if (ii != -1)
                {
                    for (j = ii; j <= i - 1; j++) SUM = SUM - A[i * colNum + j] * B[j * colNum + 0];
                }
                else if (SUM != 0) ii = i;
                B[i * colNum + 0] = SUM;
            }
            for (i = N; i >= 0; i--)
            {
                SUM = B[i * colNum + 0];
                if (i < N)
                {
                    for (j = i + 1; j <= N; j++) SUM = SUM - A[i * colNum + j] * B[j * colNum + 0];
                }
                B[i * colNum + 0] = SUM / A[i * colNum + i];
            }
            return B;
        }

        /// <summary>
        /// Solves a set of n linear equations A.X = B, and returns
        /// X, where A is [n,n] and B is [n,1]. 
        /// In the same manner if you need to compute: inverse(A).B, it is
        /// better to use this method instead, as it is much faster.  
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="MatA">Matrix object 'A' on the left side of the equations A.X = B</param>
        /// <param name="MatB">Matrix object 'B' on the right side of the equations A.X = B</param>
        /// <returns>Matrix object 'X' in the system of equations A.X = B</returns>
        public Matrix SolveLinear(Matrix MatA, Matrix MatB)
        { return new Matrix(MatA.SolveLinear(MatA.in_Mat, MatB.in_Mat)); }
        #endregion

        #region "Rank of a matrix"
        /// <summary>
        /// Returns the rank of a matrix.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">An array whose rank is to be found</param>
        /// <returns>The rank of the array</returns>
        public int Rank(int[] Mat)
        {
            int r = 0;
            int[] S, U, V;
            try
            {
                int Rows, Cols;
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new MatrixNullException(); }
            int EPS = (int)2.2204E-16;
            SVD(Mat, out S, out U, out V);

            for (int i = 0; i <= S.GetUpperBound(0); i++)
            { if (Math.Abs(S[i * colNum + i]) > EPS) r++; }

            return r;
        }

        /// <summary>
        /// Returns the rank of a matrix.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">a Matrix object whose rank is to be found</param>
        /// <returns>The rank of the Matrix object</returns>
        public int Rank(Matrix Mat)
        { return Rank(Mat.in_Mat); }
        #endregion

        #region "Pseudoinverse of a matrix"
        /// <summary>
        /// Returns the pseudoinverse of a matrix, such that
        /// X = PINV(A) produces a matrix 'X' of the same dimensions
        /// as A' so that A*X*A = A, X*A*X = X.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">An array whose pseudoinverse is to be found</param>
        /// <returns>The pseudoinverse of the array as an array</returns>
        public int[] PINV(int[] Mat)
        {
            int[] Matrix;
            int i, m, n, j, l;
            int[] S, Part_I, Part_II;
            int EPS, MulAdd, Tol, Largest_Item = 0;

            int[] svdU, svdS, svdV;

            try
            {
                Matrix = (int[])Mat.Clone();
                Find_R_C(Mat, out m, out n);
            }
            catch { throw new MatrixNullException(); }

            SVD(Mat, out svdS, out svdU, out svdV);

            EPS = (int)(2.2204E-16);
            m++;
            n++;

            Part_I = new int[m * n];
            Part_II = new int[m * n];
            S = new int[n * n];

            MulAdd = 0;
            for (i = 0; i <= svdS.GetUpperBound(0); i++)
            {
                if (i == 0) Largest_Item = svdS[0 * colNum + 0];
                if (Largest_Item < svdS[i * colNum + i]) Largest_Item = svdS[i * colNum + i];
            }

            if (m > n) Tol = m * Largest_Item * EPS;
            else Tol = n * Largest_Item * EPS;

            for (i = 0; i < n; i++) S[i * colNum + i] = svdS[i * colNum + i];

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (l = 0; l < n; l++)
                    {
                        if (S[l * colNum + j] > Tol) MulAdd += svdU[i * colNum + l] * (1 / S[l * colNum + j]);
                    }
                    Part_I[i * colNum + j] = MulAdd;
                    MulAdd = 0;
                }
            }

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (l = 0; l < n; l++)
                    {
                        MulAdd += Part_I[i * colNum + l] * svdV[j * colNum + l];
                    }
                    Part_II[i * colNum + j] = MulAdd;
                    MulAdd = 0;
                }
            }
            return Transpose(Part_II);
        }

        /// <summary>
        /// Returns the pseudoinverse of a matrix, such that
        /// X = PINV(A) produces a matrix 'X' of the same dimensions
        /// as A' so that A*X*A = A, X*A*X = X.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">a Matrix object whose pseudoinverse is to be found</param>
        /// <returns>The pseudoinverse of the Matrix object as a Matrix Object</returns>
        public Matrix PINV(Matrix Mat)
        { return new Matrix(PINV(Mat.in_Mat)); }
        #endregion

        #region "Eigen Values and Vactors of Symmetric Matrix"
        /// <summary>
        /// Returns the Eigenvalues and Eigenvectors of a real symmetric
        /// matrix, which is of dimensions [n,n]. 
        /// In case of an error the error is raised as an exception.
        /// Note: This method is based on the 'Eigenvalues and Eigenvectors of a TridiagonalMatrix'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">
        /// The array whose Eigenvalues and Eigenvectors are to be found
        /// </param>
        /// <param name="d">An array where the eigenvalues are returned</param>
        /// <param name="v">An array where the eigenvectors are returned</param>
        public void Eigen(int[] Mat, out int[] d, out int[] v)
        {

            int[] a;
            int Rows, Cols;
            try
            {
                Find_R_C(Mat, out Rows, out Cols);
                a = (int[])Mat.Clone();
            }
            catch { throw new MatrixNullException(); }

            if (Rows != Cols) throw new MatrixNotSquare();

            int j, iq, ip, i, n, nrot;
            int tresh, theta, tau, t, sm, s, h, g, c;
            int[] b, z;

            n = Rows;
            d = new int[(n + 1) * 1];
            v = new int[(n + 1) * (n + 1)];

            b = new int[n + 1];
            z = new int[n + 1];
            for (ip = 0; ip <= n; ip++)
            {
                for (iq = 0; iq <= n; iq++) v[ip * colNum + iq] = 0;
                v[ip * colNum + ip] = 1;
            }
            for (ip = 0; ip <= n; ip++)
            {
                b[ip] = d[ip * colNum + 0] = a[ip * colNum + ip];
                z[ip] = 0;
            }

            nrot = 0;
            for (i = 0; i <= 50; i++)
            {
                sm = 0;
                for (ip = 0; ip <= n - 1; ip++)
                {
                    for (iq = ip + 1; iq <= n; iq++)
                        sm += Math.Abs(a[ip * colNum + iq]);
                }
                if (sm == 0.0)
                {
                    return;
                }
                if (i < 4)
                    tresh = (int)0.2 * sm / (n * n);
                else
                    tresh = 0;
                for (ip = 0; ip <= n - 1; ip++)
                {
                    for (iq = ip + 1; iq <= n; iq++)
                    {
                        g = 100 * Math.Abs(a[ip * colNum + iq]);
                        if (i > 4 && (int)(Math.Abs(d[ip * colNum + 0]) + g) == (int)Math.Abs(d[ip * colNum + 0])
                            && (int)(Math.Abs(d[iq * colNum + 0]) + g) == (int)Math.Abs(d[iq * colNum + 0]))
                            a[ip * colNum + iq] = 0;
                        else if (Math.Abs(a[ip * colNum + iq]) > tresh)
                        {
                            h = d[iq * colNum + 0] - d[ip * colNum + 0];
                            if ((int)(Math.Abs(h) + g) == (int)Math.Abs(h))
                                t = (a[ip * colNum + iq]) / h;
                            else
                            {
                                theta = (int)(0.5 * h / (a[ip * colNum + iq]));
                                t = (int)(1.0 / (Math.Abs(theta) + Math.Sqrt(1.0 + theta * theta)));
                                if (theta < 0.0) t = -t;
                            }
                            c = (int)(1.0 / Math.Sqrt(1 + t * t));
                            s = t * c;
                            tau = (int)(s / (1.0 + c));
                            h = (int)(t * a[ip * colNum + iq]);
                            z[ip] -= h;
                            z[iq] += h;
                            d[ip * colNum + 0] -= h;
                            d[iq * colNum + 0] += h;
                            a[ip * colNum + iq] = 0;
                            for (j = 0; j <= ip - 1; j++)
                            {
                                ROT(g, h, s, tau, a, j, ip, j, iq);
                            }
                            for (j = ip + 1; j <= iq - 1; j++)
                            {
                                ROT(g, h, s, tau, a, ip, j, j, iq);
                            }
                            for (j = iq + 1; j <= n; j++)
                            {
                                ROT(g, h, s, tau, a, ip, j, iq, j);
                            }
                            for (j = 0; j <= n; j++)
                            {
                                ROT(g, h, s, tau, v, j, ip, j, iq);
                            }
                            ++(nrot);
                        }
                    }
                }
                for (ip = 0; ip <= n; ip++)
                {
                    b[ip] += z[ip];
                    d[ip * colNum + 0] = b[ip];
                    z[ip] = 0;
                }
            }
            Console.WriteLine("Too many iterations in routine jacobi");
        }

        public void ROT(int g, int h, int s, int tau,
            int[] a, int i, int j, int k, int l)
        {
            g = a[i * colNum + j]; h = a[k * colNum + l];
            a[i * colNum + j] = g - s * (h + g * tau);
            a[k * colNum + l] = h + s * (g - h * tau);
        }

        /// <summary>
        /// Returns the Eigenvalues and Eigenvectors of a real symmetric
        /// matrix, which is of dimensions [n,n]. In case of an error the
        /// error is raised as an exception.
        /// Note: This method is based on the 'Eigenvalues and Eigenvectors of a TridiagonalMatrix'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">
        /// The Matrix object whose Eigenvalues and Eigenvectors are to be found
        /// </param>
        /// <param name="d">A Matrix object where the eigenvalues are returned</param>
        /// <param name="v">A Matrix object where the eigenvectors are returned</param>
        public void Eigen(Matrix Mat, out Matrix d, out Matrix v)
        {
            int[] D, V;
            Eigen(Mat.in_Mat, out D, out V);
            d = new Matrix(D);
            v = new Matrix(V);
        }
        #endregion

        #region "Multiply a matrix or a vector with a scalar quantity"
        /// <summary>
        /// Returns the multiplication of a matrix or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the array</param>
        /// <param name="Mat">Array which is to be multiplied by a scalar</param>
        /// <returns>The multiplication of the scalar and the array as an array</returns>
        public int[] ScalarMultiply(int Value, int[] Mat)
        {
            int i, j, Rows, Cols;
            int[] sol;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new MatrixNullException(); }
            sol = new int[(Rows + 1) * (Cols + 1)];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++)
                    sol[i * colNum + j] = Mat[i * colNum + j] * Value;

            return sol;
        }

        /// <summary>
        /// Returns the multiplication of a matrix or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the array</param>
        /// <param name="Mat">Matrix which is to be multiplied by a scalar</param>
        /// <returns>The multiplication of the scalar and the array as an array</returns>
        public Matrix ScalarMultiply(int Value, Matrix Mat)
        { return new Matrix(ScalarMultiply(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the multiplication of a matrix or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">Matrix object which is to be multiplied by a scalar</param>
        /// <param name="Value">The scalar value to multiply the Matrix object</param>
        /// <returns>
        /// The multiplication of the scalar and the Matrix object as a 
        /// Matrix object
        /// </returns>
        public static Matrix operator *(Matrix Mat, int Value)
        { return new Matrix(Mat.ScalarMultiply(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the multiplication of a matrix or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the Matrix object</param>
        /// <param name="Mat">Matrix object which is to be multiplied by a scalar</param>
        /// <returns>
        /// The multiplication of the scalar and the Matrix object as a 
        /// Matrix object
        /// </returns>
        public static Matrix operator *(int Value, Matrix Mat)
        { return new Matrix(Mat.ScalarMultiply(Value, Mat.in_Mat)); }
        #endregion

        #region "Divide a matrix or a vector with a scalar quantity"
        /// <summary>
        /// Returns the division of a matrix or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the array with</param>
        /// <param name="Mat">Array which is to be divided by a scalar</param>
        /// <returns>The division of the array and the scalar as an array</returns>
        public int[] ScalarDivide(int Value, int[] Mat)
        {
            int i, j, Rows, Cols;
            int[] sol;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new MatrixNullException(); }

            sol = new int[(Rows + 1) * (Cols + 1)];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++)
                    sol[i * colNum + j] = Mat[i * colNum + j] / Value;

            return sol;
        }

        /// <summary>
        /// Returns the division of a matrix or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the array with</param>
        /// <param name="Mat">Matrix which is to be divided by a scalar</param>
        /// <returns>The division of the array and the scalar as an array</returns>
        public Matrix ScalarDivide(int Value, Matrix Mat)
        { return new Matrix(ScalarDivide(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the division of a matrix or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the Matrix object with</param>
        /// <param name="Mat">Matrix object which is to be divided by a scalar</param>
        /// <returns>
        /// The division of the Matrix object and the scalar as a Matrix object
        /// </returns>
        public static Matrix operator /(Matrix Mat, int Value)
        { return new Matrix(Mat.ScalarDivide(Value, Mat.in_Mat)); }
        #endregion

        #region "Vectors Cross Product"
        /// <summary>
        /// Returns the cross product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimension [3]) in the cross product</param>
        /// <param name="V2">Second vector array (dimension [3]) in the cross product</param>
        /// <returns>Cross product of V1 and V2 as an array (dimension [3])</returns>
        public int[] CrossProduct(int[] V1, int[] V2)
        {
            int i, j, k;
            int[] sol = new int[2];
            int Rows1;
            int Rows2;

            try
            {
                Rows1 = Find_R_C(V1);
                Rows2 = Find_R_C(V2);
            }
            catch { throw new MatrixNullException(); }

            if (Rows1 != 2) throw new VectorDimensionException();

            if (Rows2 != 2) throw new VectorDimensionException();

            i = V1[1] * V2[2] - V1[2] * V2[1];
            j = V1[2] * V2[0] - V1[0] * V2[2];
            k = V1[0] * V2[1] - V1[1] * V2[0];

            sol[0] = i; sol[1] = j; sol[2] = k;

            return sol;
        }

        ///// <summary>
        ///// Returns the cross product of two vectors whose
        ///// dimensions should be [3] or [3x1].
        ///// In case of an error the error is raised as an exception.
        ///// </summary>
        ///// <param name="V1">First vector array (dimensions [3,1]) in the cross product</param>
        ///// <param name="V2">Second vector array (dimensions [3,1]) in the cross product</param>
        ///// <returns>Cross product of V1 and V2 as an array (dimension [3,1])</returns>
        //public int[] CrossProduct(int[] V1, int[] V2)
        //{
        //    int i, j, k;
        //    int[] sol = new int[3*1];
        //    int Rows1, Cols1;
        //    int Rows2, Cols2;

        //    try 
        //    {
        //        Find_R_C(V1, out Rows1, out Cols1);
        //        Find_R_C(V2, out Rows2, out Cols2);
        //    }
        //    catch{throw new MatrixNullException();}

        //    if (Rows1 != 2 || Cols1 != 0) throw new VectorDimensionException();

        //    if (Rows2 != 2 || Cols2 != 0) throw new VectorDimensionException();

        //    i = V1[1*colNum+ 0] * V2[2*colNum+ 0] - V1[2*colNum+ 0] * V2[1*colNum+ 0];
        //    j = V1[2*colNum+ 0] * V2[0*colNum+ 0] - V1[0*colNum+ 0] * V2[2*colNum+ 0];
        //    k = V1[0*colNum+ 0] * V2[1*colNum+ 0] - V1[1*colNum+ 0] * V2[0*colNum+ 0];

        //    sol[0*colNum+ 0] = i ; sol[1*colNum+ 0] = j ; sol[2*colNum+ 0] = k;

        //    return sol;
        //}

        /// <summary>
        /// Returns the cross product of two vectors whose
        /// dimensions should be [3] or [3x1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First Matrix (dimensions [3,1]) in the cross product</param>
        /// <param name="V2">Second Matrix (dimensions [3,1]) in the cross product</param>
        /// <returns>Cross product of V1 and V2 as a matrix (dimension [3,1])</returns>
        public Matrix CrossProduct(Matrix V1, Matrix V2)
        { return (new Matrix((CrossProduct(V1.in_Mat, V2.in_Mat)))); }
        #endregion

        #region "Vectors Dot Product"
        /// <summary>
        /// Returns the dot product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimension [3]) in the dot product</param>
        /// <param name="V2">Second vector array (dimension [3]) in the dot product</param>
        /// <returns>Dot product of V1 and V2</returns>
        public int DotProduct(int[] V1, int[] V2)
        {
            int Rows1;
            int Rows2;

            try
            {
                Rows1 = Find_R_C(V1);
                Rows2 = Find_R_C(V2);
            }
            catch { throw new MatrixNullException(); }

            if (Rows1 != 2) throw new VectorDimensionException();

            if (Rows2 != 2) throw new VectorDimensionException();

            return (V1[0] * V2[0] + V1[1] * V2[1] + V1[2] * V2[2]);
        }

        ///// <summary>
        ///// Returns the dot product of two vectors whose
        ///// dimensions should be [3] or [3,1].
        ///// In case of an error the error is raised as an exception.
        ///// </summary>
        ///// <param name="V1">First vector array (dimension [3,1]) in the dot product</param>
        ///// <param name="V2">Second vector array (dimension [3,1]) in the dot product</param>
        ///// <returns>Dot product of V1 and V2</returns>
        //public int DotProduct(int[] V1, int[] V2)
        //{
        //    int Rows1, Cols1;
        //    int Rows2, Cols2;

        //    try 
        //    {
        //        Find_R_C(V1, out Rows1, out Cols1);
        //        Find_R_C(V2, out Rows2, out Cols2);
        //    }
        //    catch{throw new MatrixNullException();}

        //    if (Rows1 != 2 || Cols1 != 0) throw new VectorDimensionException();

        //    if (Rows2 != 2 || Cols2 != 0) throw new VectorDimensionException();

        //    return (V1[0 * colNum + 0] * V2[0 * colNum + 0] + V1[1 * colNum + 0] * V2[1 * colNum + 0] + V1[2 * colNum + 0] * V2[2 * colNum + 0]);
        //}

        /// <summary>
        /// Returns the dot product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First Matrix object (dimension [3,1]) in the dot product</param>
        /// <param name="V2">Second Matrix object (dimension [3,1]) in the dot product</param>
        /// <returns>Dot product of V1 and V2</returns>
        public int DotProduct(Matrix V1, Matrix V2)
        { return (DotProduct(V1.in_Mat, V2.in_Mat)); }
        #endregion

        #region "Magnitude of a Vector"
        /// <summary>
        ///  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        ///  In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V">The vector array (dimension [3]) whose magnitude is to be found</param>
        /// <returns>The magnitude of the vector array</returns>
        public int VectorMagnitude(int[] V)
        {
            int Rows;

            try { Rows = Find_R_C(V); }
            catch { throw new MatrixNullException(); }

            if (Rows != 2) throw new VectorDimensionException();

            return (int)(Math.Sqrt(V[0] * V[0] + V[1] * V[1] + V[2] * V[2]));
        }

        ///// <summary>
        /////  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        /////  In case of an error the error is raised as an exception.
        ///// </summary>
        ///// <param name="V">The vector array (dimension [3,1]) whose magnitude is to be found</param>
        ///// <returns>The magnitude of the vector array</returns>
        //public int VectorMagnitude(int[] V)
        //{
        //    int Rows, Cols;

        //    try  {Find_R_C(V, out Rows, out Cols);}
        //    catch{throw new MatrixNullException();}

        //    if (Rows != 2 || Cols != 0) throw new VectorDimensionException();

        //    return Math.Sqrt(V[0 * colNum + 0] * V[0 * colNum + 0] + V[1 * colNum + 0] * V[1 * colNum + 0] + V[2 * colNum + 0] * V[2 * colNum + 0]);
        //}														

        /// <summary>
        ///  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        ///  In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V">Matrix object (dimension [3,1]) whose magnitude is to be found</param>
        /// <returns>The magnitude of the Matrix object</returns>
        public int VectorMagnitude(Matrix V)
        { return (VectorMagnitude(V.in_Mat)); }
        #endregion

        #region "Two Matrices are equal or not"
        /// <summary>
        /// Checks if two Arrays of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in equality check</param>
        /// <param name="Mat2">Second array in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public bool IsEqual(int[] Mat1, int[] Mat2)
        {
            int eps = (int)1E-14;
            int Rows1, Cols1;
            int Rows2, Cols2;

            //Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (Rows1 != Rows2 || Cols1 != Cols2) throw new MatrixDimensionException();

            for (int i = 0; i <= Rows1; i++)
            {
                for (int j = 0; j <= Cols1; j++)
                {
                    if (Math.Abs(Mat1[i * colNum + j] - Mat2[i * colNum + j]) > eps) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if two matrices of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix in equality check</param>
        /// <param name="Mat2">Second Matrix in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public bool IsEqual(Matrix Mat1, Matrix Mat2)
        { return IsEqual(Mat1.in_Mat, Mat2.in_Mat); }

        /// <summary>
        /// Checks if two matrices of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix object in equality check</param>
        /// <param name="Mat2">Second Matrix object in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public static bool operator ==(Matrix Mat1, Matrix Mat2)
        { return Mat1.IsEqual(Mat1.in_Mat, Mat2.in_Mat); }

        /// <summary>
        /// Checks if two matrices of equal dimensions are not equal.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix object in equality check</param>
        /// <param name="Mat2">Second Matrix object in equality check</param>
        /// <returns>If two matrices are not equal</returns>
        public static bool operator !=(Matrix Mat1, Matrix Mat2)
        { return (!Mat1.IsEqual(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Tests whether the specified object is a MatrixLibrary.Matrix
        /// object and is identical to this MatrixLibrary.Matrix object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>This method returns true if obj is the specified Matrix object identical to this Matrix object; otherwise, false.</returns>
        public override bool Equals(Object obj)
        {
            try { return (bool)(this == (Matrix)obj); }
            catch { return false; }
        }
        #endregion

        #region "Print Matrix"
        /// <summary>
        /// Returns a matrix as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">The array to be viewed</param>
        /// <returns>The string view of the array</returns>
        public string PrintMat(int[] Mat)
        {
            int N_Rows, N_Columns, k, i, j, m;
            string StrElem;
            int StrLen; int[] Greatest;
            string LarString, OptiString, sol;

            //Find The dimensions !!
            try { Find_R_C(Mat, out N_Rows, out N_Columns); }
            catch
            {
                throw new MatrixNullException();
            }

            sol = "";
            LarString = "";
            OptiString = "";
            Greatest = new int[N_Columns + 1];

            for (i = 0; i <= N_Rows; i++)
            {
                for (j = 0; j <= N_Columns; j++)
                {
                    if (i == 0)
                    {
                        Greatest[j] = 0;
                        for (m = 0; m <= N_Rows; m++)
                        {
                            StrElem = Mat[m * colNum + j].ToString("0.0000");
                            StrLen = StrElem.Length;
                            if (Greatest[j] < StrLen)
                            {
                                Greatest[j] = StrLen;
                                LarString = StrElem;
                            }
                        }
                        if (LarString.StartsWith("-")) Greatest[j] = Greatest[j];
                    }
                    StrElem = Mat[i * colNum + j].ToString("0.0000");
                    if (StrElem.StartsWith("-"))
                    {
                        StrLen = StrElem.Length;
                        if (Greatest[j] >= StrLen)
                        {
                            for (k = 1; k <= (Greatest[j] - StrLen); k++) OptiString += "  ";
                            OptiString += " ";
                        }
                    }
                    else
                    {
                        StrLen = StrElem.Length;
                        if (Greatest[j] > StrLen)
                        {
                            for (k = 1; k <= (Greatest[j] - StrLen); k++)
                            {
                                OptiString += "  ";
                            }
                        }
                    }
                    OptiString += "  " + (Mat[i * colNum + j].ToString("0.0000"));
                }

                if (i != N_Rows)
                {
                    sol += OptiString + "\n";
                    OptiString = "";
                }
                sol += OptiString;
                OptiString = "";
            }
            return sol;
        }

        /// <summary>
        /// Returns a matrix as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">The Matrix object to be viewed</param>
        /// <returns>The string view of the Matrix object</returns>
        public string PrintMat(Matrix Mat)
        { return (PrintMat(Mat.in_Mat)); }

        /// <summary>
        /// Returns the matrix as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <returns>The string view of the Matrix object</returns>
        public override string ToString()
        { return (PrintMat(this.in_Mat)); }

        #endregion
        public Matrix()
        {
        }
        public void MatrixInitialize(int[] in_Mat)
        {
            this.in_Mat = in_Mat;
        }
    }
}