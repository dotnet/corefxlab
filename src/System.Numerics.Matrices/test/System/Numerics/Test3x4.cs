// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix3x4 structure.
    /// </summary>
    public class Test3x4
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix3x4 matrix3x4;

            matrix3x4 = new Matrix3x4();

            for (int x = 0; x < matrix3x4.Columns; x++)
            {
                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    Assert.Equal(0, matrix3x4[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix3x4 = new Matrix3x4(value);

            for (int x = 0; x < matrix3x4.Columns; x++)
            {
                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    Assert.Equal(value, matrix3x4[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix3x4);

            for (int y = 0; y < matrix3x4.Rows; y++)
            {
                for (int x = 0; x < matrix3x4.Columns; x++)
                {
                    Assert.Equal(y * matrix3x4.Columns + x, matrix3x4[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            for (int x = 0; x < matrix3x4.Columns; x++)
            {
                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    matrix3x4[x, y] = y * matrix3x4.Columns + x;
                }
            }

            for (int y = 0; y < matrix3x4.Rows; y++)
            {
                for (int x = 0; x < matrix3x4.Columns; x++)
                {
                    Assert.Equal(y * matrix3x4.Columns + x, matrix3x4[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            Assert.Equal(3, matrix3x4.Columns);
            Assert.Equal(4, matrix3x4.Rows);
            Assert.Equal(Matrix3x4.ColumnCount, matrix3x4.Columns);
            Assert.Equal(Matrix3x4.RowCount, matrix3x4.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix3x4 matrix3x4;

            GenerateFilledMatrixWithValues(out matrix3x4);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix3x4 result = matrix3x4 * c;

                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    for (int x = 0; x < matrix3x4.Columns; x++)
                    {
                        Assert.Equal(matrix3x4[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            matrix3x4.M11 = 0;
            matrix3x4.M21 = 1;
            matrix3x4.M31 = 2;
            matrix3x4.M12 = 3;
            matrix3x4.M22 = 4;
            matrix3x4.M32 = 5;
            matrix3x4.M13 = 6;
            matrix3x4.M23 = 7;
            matrix3x4.M33 = 8;
            matrix3x4.M14 = 9;
            matrix3x4.M24 = 10;
            matrix3x4.M34 = 11;

            Assert.Equal(0, matrix3x4.M11, Epsilon);
            Assert.Equal(1, matrix3x4.M21, Epsilon);
            Assert.Equal(2, matrix3x4.M31, Epsilon);
            Assert.Equal(3, matrix3x4.M12, Epsilon);
            Assert.Equal(4, matrix3x4.M22, Epsilon);
            Assert.Equal(5, matrix3x4.M32, Epsilon);
            Assert.Equal(6, matrix3x4.M13, Epsilon);
            Assert.Equal(7, matrix3x4.M23, Epsilon);
            Assert.Equal(8, matrix3x4.M33, Epsilon);
            Assert.Equal(9, matrix3x4.M14, Epsilon);
            Assert.Equal(10, matrix3x4.M24, Epsilon);
            Assert.Equal(11, matrix3x4.M34, Epsilon);

            Assert.Equal(matrix3x4[0, 0], matrix3x4.M11, Epsilon);
            Assert.Equal(matrix3x4[1, 0], matrix3x4.M21, Epsilon);
            Assert.Equal(matrix3x4[2, 0], matrix3x4.M31, Epsilon);
            Assert.Equal(matrix3x4[0, 1], matrix3x4.M12, Epsilon);
            Assert.Equal(matrix3x4[1, 1], matrix3x4.M22, Epsilon);
            Assert.Equal(matrix3x4[2, 1], matrix3x4.M32, Epsilon);
            Assert.Equal(matrix3x4[0, 2], matrix3x4.M13, Epsilon);
            Assert.Equal(matrix3x4[1, 2], matrix3x4.M23, Epsilon);
            Assert.Equal(matrix3x4[2, 2], matrix3x4.M33, Epsilon);
            Assert.Equal(matrix3x4[0, 3], matrix3x4.M14, Epsilon);
            Assert.Equal(matrix3x4[1, 3], matrix3x4.M24, Epsilon);
            Assert.Equal(matrix3x4[2, 3], matrix3x4.M34, Epsilon);
        }

        [Fact]
        public void ColumnAccessorAreCorrect()
        {
            Matrix3x4 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x4 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[0, y], column1[0, y]);
            }

            Matrix1x4 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[1, y], column2[0, y]);
            }
        }

        [Fact]
        public void RowAccessorAreCorrect()
        {
            Matrix3x4 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix3x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 0], row1[x, 0]);
            }

            Matrix3x1 row2 = value.Row2;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 1], row2[x, 0]);
            }

            Matrix3x1 row3 = value.Row3;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 2], row3[x, 0]);
            }
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix3x4 value = new Matrix3x4(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix3x4 value1 = new Matrix3x4(1);
            Matrix3x4 value2 = new Matrix3x4(99);
            Matrix3x4 result = value1 + value2;

            for (int y = 0; y < Matrix3x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x4.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix3x4 value1 = new Matrix3x4(100);
            Matrix3x4 value2 = new Matrix3x4(1);
            Matrix3x4 result = value1 - value2;

            for (int y = 0; y < Matrix3x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x4.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix3x4 value1 = new Matrix3x4(100);
            Matrix3x4 value2 = new Matrix3x4(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x4[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x4[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x4[3, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x4[0, 4] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix1x3ProducesMatrix1x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix1x3 matrix2 = new Matrix1x3(2);
            Matrix1x4 result = matrix1 * matrix2;
            Matrix1x4 expected = new Matrix1x4(18, 
                                               18, 
                                               18, 
                                               18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix2x3ProducesMatrix2x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix2x3 matrix2 = new Matrix2x3(2);
            Matrix2x4 result = matrix1 * matrix2;
            Matrix2x4 expected = new Matrix2x4(18, 18, 
                                               18, 18, 
                                               18, 18, 
                                               18, 18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x3ProducesMatrix3x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix3x3 matrix2 = new Matrix3x3(2);
            Matrix3x4 result = matrix1 * matrix2;
            Matrix3x4 expected = new Matrix3x4(18, 18, 18, 
                                               18, 18, 18, 
                                               18, 18, 18, 
                                               18, 18, 18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x3ProducesMatrix4x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix4x3 matrix2 = new Matrix4x3(2);
            Matrix4x4 result = matrix1 * matrix2;
            Matrix4x4 expected = new Matrix4x4(18, 18, 18, 18, 
                                               18, 18, 18, 18, 
                                               18, 18, 18, 18, 
                                               18, 18, 18, 18);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix3x4 matrix)
        {
            matrix = new Matrix3x4( 0,  1,  2, 
                                    3,  4,  5, 
                                    6,  7,  8, 
                                    9, 10, 11);
        }
    }
}
