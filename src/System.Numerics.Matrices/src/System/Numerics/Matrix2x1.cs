// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.InteropServices;
using System.Text;

namespace System.Numerics.Matrices
{
    /// <summary>
    /// Represents a matrix of double precision floating-point values defined by its number of columns and rows
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2x1: IEquatable<Matrix2x1>, IMatrix
    {
        public const int ColumnCount = 2;
        public const int RowCount = 1;

        static Matrix2x1()
        {
            Zero = new Matrix2x1(0);
        }

        /// <summary>
        /// Gets the smallest value used to determine equality
        /// </summary>
        public double Epsilon { get { return MatrixHelper.Epsilon; } }

        /// <summary>
        /// Constant Matrix2x1 with all values initialized to zero
        /// </summary>
        public static readonly Matrix2x1 Zero;

        /// <summary>
        /// Initializes a Matrix2x1 with all of it values specifically set
        /// </summary>
        /// <param name="m11">The column 1, row 1 value</param>
        /// <param name="m21">The column 2, row 1 value</param>
        public Matrix2x1(double m11, double m21)
        {
            M11 = m11; M21 = m21; 
        }

        /// <summary>
        /// Initialized a Matrix2x1 with all values set to the same value
        /// </summary>
        /// <param name="value">The value to set all values to</param>
        public Matrix2x1(double value)
        {
            M11 = M21 = value;
        }

        public double M11;
        public double M21;

        public unsafe double this[int col, int row]
        {
            get
            {
                if (col < 0 || col >= ColumnCount)
                    throw new ArgumentOutOfRangeException("col", String.Format("Expected greater than or equal to 0 and less than {0}, found {1}.", ColumnCount, col));
                if (row < 0 || row >= RowCount)
                    throw new ArgumentOutOfRangeException("row", String.Format("Expected greater than or equal to 0 and less than {0}, found {1}.", RowCount, row));

                fixed (Matrix2x1* p = &this)
                {
                    double* d = (double*)p;
                    return d[row * ColumnCount + col];
                }
            }
            set
            {
                if (col < 0 || col >= ColumnCount)
                    throw new ArgumentOutOfRangeException("col", String.Format("Expected greater than or equal to 0 and less than {0}, found {1}.", ColumnCount, col));
                if (row < 0 || row >= RowCount)
                    throw new ArgumentOutOfRangeException("row", String.Format("Expected greater than or equal to 0 and less than {0}, found {1}.", RowCount, row));

                fixed (Matrix2x1* p = &this)
                {
                    double* d = (double*)p;
                    d[row * ColumnCount + col] = value;
                }
            }
        }

        /// <summary>
        /// Gets the number of columns in the matrix
        /// </summary>
        public int Columns { get { return ColumnCount; } }
        /// <summary>
        /// Get the number of rows in the matrix
        /// </summary>
        public int Rows { get { return RowCount; } }


        public override bool Equals(object obj)
        {
            if (obj is Matrix2x1)
                return this == (Matrix2x1)obj;

            return false;
        }

        public bool Equals(Matrix2x1 other)
        {
            return this == other;
        }

        public unsafe override int GetHashCode()
        {
            fixed (Matrix2x1* p = &this)
            {
                int* x = (int*)p;
                unchecked
                {
                    return 0xFFE1
                         + 7 * ((((x[00] ^ x[01]) << 0) + ((x[02] ^ x[03]) << 1)) << 0);
                }
            }
        }

        public override string ToString()
        {
		    var sb = new StringBuilder();
            sb.Append("Matrix2x1: ");
            sb.AppendFormat("{{|{0}|{1}|}}", M11, M21);
			return sb.ToString();
        }

        /// <summary>
        /// Creates and returns a transposed matrix
        /// </summary>
        /// <returns>Matrix with transposed values</returns>
        public Matrix1x2 Transpose()
        {
            return new Matrix1x2(M11, 
                                 M21);
        }

        public static bool operator ==(Matrix2x1 matrix1, Matrix2x1 matrix2)
        {
            return MatrixHelper.AreEqual(matrix1.M11, matrix2.M11)
                && MatrixHelper.AreEqual(matrix1.M21, matrix2.M21);
        }

        public static bool operator !=(Matrix2x1 matrix1, Matrix2x1 matrix2)
        {
            return MatrixHelper.NotEqual(matrix1.M11, matrix2.M11)
                || MatrixHelper.NotEqual(matrix1.M21, matrix2.M21);
        }

        public static Matrix2x1 operator +(Matrix2x1 matrix1, Matrix2x1 matrix2)
        {
            double m11 = matrix1.M11 + matrix2.M11;
            double m21 = matrix1.M21 + matrix2.M21;

            return new Matrix2x1(m11, m21);
        }

        public static Matrix2x1 operator -(Matrix2x1 matrix1, Matrix2x1 matrix2)
        {
            double m11 = matrix1.M11 - matrix2.M11;
            double m21 = matrix1.M21 - matrix2.M21;

            return new Matrix2x1(m11, m21);
        }

        public static Matrix2x1 operator *(Matrix2x1 matrix, double scalar)
        {
            double m11 = matrix.M11 * scalar;
            double m21 = matrix.M21 * scalar;

            return new Matrix2x1(m11, m21);
        }

        public static Matrix2x1 operator *(double scalar, Matrix2x1 matrix)
        {
            double m11 = scalar * matrix.M11;
            double m21 = scalar * matrix.M21;

            return new Matrix2x1(m11, m21);
        }

        public static Matrix2x1 operator *(Matrix2x1 matrix1, Matrix2x2 matrix2)
        {
            double m11 = matrix1.M11 * matrix2.M11 + matrix1.M21 * matrix2.M12;
            double m21 = matrix1.M11 * matrix2.M21 + matrix1.M21 * matrix2.M22;

            return new Matrix2x1(m11, m21);
        }
        public static Matrix3x1 operator *(Matrix2x1 matrix1, Matrix3x2 matrix2)
        {
            double m11 = matrix1.M11 * matrix2.M11 + matrix1.M21 * matrix2.M12;
            double m21 = matrix1.M11 * matrix2.M21 + matrix1.M21 * matrix2.M22;
            double m31 = matrix1.M11 * matrix2.M31 + matrix1.M21 * matrix2.M32;

            return new Matrix3x1(m11, m21, m31);
        }
        public static Matrix4x1 operator *(Matrix2x1 matrix1, Matrix4x2 matrix2)
        {
            double m11 = matrix1.M11 * matrix2.M11 + matrix1.M21 * matrix2.M12;
            double m21 = matrix1.M11 * matrix2.M21 + matrix1.M21 * matrix2.M22;
            double m31 = matrix1.M11 * matrix2.M31 + matrix1.M21 * matrix2.M32;
            double m41 = matrix1.M11 * matrix2.M41 + matrix1.M21 * matrix2.M42;

            return new Matrix4x1(m11, m21, m31, m41);
        }
    }
}
