// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix4x3 structure.
    /// </summary>
    public class Test4x3
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix4x3 matrix4x3;

            matrix4x3 = new Matrix4x3();

            for (int x = 0; x < matrix4x3.Columns; x++)
            {
                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    Assert.Equal(0, matrix4x3[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix4x3 = new Matrix4x3(value);

            for (int x = 0; x < matrix4x3.Columns; x++)
            {
                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    Assert.Equal(value, matrix4x3[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix4x3);

            for (int y = 0; y < matrix4x3.Rows; y++)
            {
                for (int x = 0; x < matrix4x3.Columns; x++)
                {
                    Assert.Equal(y * matrix4x3.Columns + x, matrix4x3[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            for (int x = 0; x < matrix4x3.Columns; x++)
            {
                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    matrix4x3[x, y] = y * matrix4x3.Columns + x;
                }
            }

            for (int y = 0; y < matrix4x3.Rows; y++)
            {
                for (int x = 0; x < matrix4x3.Columns; x++)
                {
                    Assert.Equal(y * matrix4x3.Columns + x, matrix4x3[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            Assert.Equal(4, matrix4x3.Columns);
            Assert.Equal(3, matrix4x3.Rows);
            Assert.Equal(Matrix4x3.ColumnCount, matrix4x3.Columns);
            Assert.Equal(Matrix4x3.RowCount, matrix4x3.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix4x3 matrix4x3;

            GenerateFilledMatrixWithValues(out matrix4x3);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix4x3 result = matrix4x3 * c;

                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    for (int x = 0; x < matrix4x3.Columns; x++)
                    {
                        Assert.Equal(matrix4x3[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            matrix4x3.M11 = 0;
            matrix4x3.M21 = 1;
            matrix4x3.M31 = 2;
            matrix4x3.M41 = 3;
            matrix4x3.M12 = 4;
            matrix4x3.M22 = 5;
            matrix4x3.M32 = 6;
            matrix4x3.M42 = 7;
            matrix4x3.M13 = 8;
            matrix4x3.M23 = 9;
            matrix4x3.M33 = 10;
            matrix4x3.M43 = 11;

            Assert.Equal(0, matrix4x3.M11, Epsilon);
            Assert.Equal(1, matrix4x3.M21, Epsilon);
            Assert.Equal(2, matrix4x3.M31, Epsilon);
            Assert.Equal(3, matrix4x3.M41, Epsilon);
            Assert.Equal(4, matrix4x3.M12, Epsilon);
            Assert.Equal(5, matrix4x3.M22, Epsilon);
            Assert.Equal(6, matrix4x3.M32, Epsilon);
            Assert.Equal(7, matrix4x3.M42, Epsilon);
            Assert.Equal(8, matrix4x3.M13, Epsilon);
            Assert.Equal(9, matrix4x3.M23, Epsilon);
            Assert.Equal(10, matrix4x3.M33, Epsilon);
            Assert.Equal(11, matrix4x3.M43, Epsilon);

            Assert.Equal(matrix4x3[0, 0], matrix4x3.M11, Epsilon);
            Assert.Equal(matrix4x3[1, 0], matrix4x3.M21, Epsilon);
            Assert.Equal(matrix4x3[2, 0], matrix4x3.M31, Epsilon);
            Assert.Equal(matrix4x3[3, 0], matrix4x3.M41, Epsilon);
            Assert.Equal(matrix4x3[0, 1], matrix4x3.M12, Epsilon);
            Assert.Equal(matrix4x3[1, 1], matrix4x3.M22, Epsilon);
            Assert.Equal(matrix4x3[2, 1], matrix4x3.M32, Epsilon);
            Assert.Equal(matrix4x3[3, 1], matrix4x3.M42, Epsilon);
            Assert.Equal(matrix4x3[0, 2], matrix4x3.M13, Epsilon);
            Assert.Equal(matrix4x3[1, 2], matrix4x3.M23, Epsilon);
            Assert.Equal(matrix4x3[2, 2], matrix4x3.M33, Epsilon);
            Assert.Equal(matrix4x3[3, 2], matrix4x3.M43, Epsilon);
        }

        [Fact]
        public void ColumnAccessorAreCorrect()
        {
            Matrix4x3 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x3 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[0, y], column1[0, y]);
            }

            Matrix1x3 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[1, y], column2[0, y]);
            }

            Matrix1x3 column3 = value.Column3;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[2, y], column3[0, y]);
            }
        }

        [Fact]
        public void RowAccessorAreCorrect()
        {
            Matrix4x3 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix4x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 0], row1[x, 0]);
            }

            Matrix4x1 row2 = value.Row2;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 1], row2[x, 0]);
            }
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix4x3 value = new Matrix4x3(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix4x3 value1 = new Matrix4x3(1);
            Matrix4x3 value2 = new Matrix4x3(99);
            Matrix4x3 result = value1 + value2;

            for (int y = 0; y < Matrix4x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x3.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix4x3 value1 = new Matrix4x3(100);
            Matrix4x3 value2 = new Matrix4x3(1);
            Matrix4x3 result = value1 - value2;

            for (int y = 0; y < Matrix4x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x3.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix4x3 value1 = new Matrix4x3(100);
            Matrix4x3 value2 = new Matrix4x3(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x3[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x3[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x3[4, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x3[0, 3] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix1x4ProducesMatrix1x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix1x4 matrix2 = new Matrix1x4(2);
            Matrix1x3 result = matrix1 * matrix2;
            Matrix1x3 expected = new Matrix1x3(24, 
                                               24, 
                                               24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix2x4ProducesMatrix2x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix2x4 matrix2 = new Matrix2x4(2);
            Matrix2x3 result = matrix1 * matrix2;
            Matrix2x3 expected = new Matrix2x3(24, 24, 
                                               24, 24, 
                                               24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x4ProducesMatrix3x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix3x4 matrix2 = new Matrix3x4(2);
            Matrix3x3 result = matrix1 * matrix2;
            Matrix3x3 expected = new Matrix3x3(24, 24, 24, 
                                               24, 24, 24, 
                                               24, 24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x4ProducesMatrix4x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix4x4 matrix2 = new Matrix4x4(2);
            Matrix4x3 result = matrix1 * matrix2;
            Matrix4x3 expected = new Matrix4x3(24, 24, 24, 24, 
                                               24, 24, 24, 24, 
                                               24, 24, 24, 24);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix4x3 matrix)
        {
            matrix = new Matrix4x3( 0,  1,  2,  3, 
                                    4,  5,  6,  7, 
                                    8,  9, 10, 11);
        }
    }
}
