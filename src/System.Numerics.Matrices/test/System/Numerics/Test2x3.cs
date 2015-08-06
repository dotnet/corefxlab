// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix2x3 structure.
    /// </summary>
    public class Test2x3
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix2x3 matrix2x3;

            matrix2x3 = new Matrix2x3();

            for (int x = 0; x < matrix2x3.Columns; x++)
            {
                for (int y = 0; y < matrix2x3.Rows; y++)
                {
                    Assert.Equal(0, matrix2x3[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix2x3 = new Matrix2x3(value);

            for (int x = 0; x < matrix2x3.Columns; x++)
            {
                for (int y = 0; y < matrix2x3.Rows; y++)
                {
                    Assert.Equal(value, matrix2x3[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix2x3);

            for (int y = 0; y < matrix2x3.Rows; y++)
            {
                for (int x = 0; x < matrix2x3.Columns; x++)
                {
                    Assert.Equal(y * matrix2x3.Columns + x, matrix2x3[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix2x3 matrix2x3 = new Matrix2x3();

            for (int x = 0; x < matrix2x3.Columns; x++)
            {
                for (int y = 0; y < matrix2x3.Rows; y++)
                {
                    matrix2x3[x, y] = y * matrix2x3.Columns + x;
                }
            }

            for (int y = 0; y < matrix2x3.Rows; y++)
            {
                for (int x = 0; x < matrix2x3.Columns; x++)
                {
                    Assert.Equal(y * matrix2x3.Columns + x, matrix2x3[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix2x3 matrix2x3 = new Matrix2x3();

            Assert.Equal(2, matrix2x3.Columns);
            Assert.Equal(3, matrix2x3.Rows);
            Assert.Equal(Matrix2x3.ColumnCount, matrix2x3.Columns);
            Assert.Equal(Matrix2x3.RowCount, matrix2x3.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix2x3 matrix2x3;

            GenerateFilledMatrixWithValues(out matrix2x3);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix2x3 result = matrix2x3 * c;

                for (int y = 0; y < matrix2x3.Rows; y++)
                {
                    for (int x = 0; x < matrix2x3.Columns; x++)
                    {
                        Assert.Equal(matrix2x3[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix2x3 matrix2x3 = new Matrix2x3();

            matrix2x3.M11 = 0;
            matrix2x3.M21 = 1;
            matrix2x3.M12 = 2;
            matrix2x3.M22 = 3;
            matrix2x3.M13 = 4;
            matrix2x3.M23 = 5;

            Assert.Equal(0, matrix2x3.M11, Epsilon);
            Assert.Equal(1, matrix2x3.M21, Epsilon);
            Assert.Equal(2, matrix2x3.M12, Epsilon);
            Assert.Equal(3, matrix2x3.M22, Epsilon);
            Assert.Equal(4, matrix2x3.M13, Epsilon);
            Assert.Equal(5, matrix2x3.M23, Epsilon);

            Assert.Equal(matrix2x3[0, 0], matrix2x3.M11, Epsilon);
            Assert.Equal(matrix2x3[1, 0], matrix2x3.M21, Epsilon);
            Assert.Equal(matrix2x3[0, 1], matrix2x3.M12, Epsilon);
            Assert.Equal(matrix2x3[1, 1], matrix2x3.M22, Epsilon);
            Assert.Equal(matrix2x3[0, 2], matrix2x3.M13, Epsilon);
            Assert.Equal(matrix2x3[1, 2], matrix2x3.M23, Epsilon);
        }

        [Fact]
        public void ColumnAccessorAreCorrect()
        {
            Matrix2x3 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x3 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[0, y], column1[0, y]);
            }
        }

        [Fact]
        public void RowAccessorAreCorrect()
        {
            Matrix2x3 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix2x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 0], row1[x, 0]);
            }

            Matrix2x1 row2 = value.Row2;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 1], row2[x, 0]);
            }
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix2x3 value = new Matrix2x3(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix2x3 value1 = new Matrix2x3(1);
            Matrix2x3 value2 = new Matrix2x3(99);
            Matrix2x3 result = value1 + value2;

            for (int y = 0; y < Matrix2x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x3.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix2x3 value1 = new Matrix2x3(100);
            Matrix2x3 value2 = new Matrix2x3(1);
            Matrix2x3 result = value1 - value2;

            for (int y = 0; y < Matrix2x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x3.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix2x3 value1 = new Matrix2x3(100);
            Matrix2x3 value2 = new Matrix2x3(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix2x3 matrix2x3 = new Matrix2x3();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x3[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x3[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x3[2, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x3[0, 3] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix1x2ProducesMatrix1x3()
        {
            Matrix2x3 matrix1 = new Matrix2x3(3);
            Matrix1x2 matrix2 = new Matrix1x2(2);
            Matrix1x3 result = matrix1 * matrix2;
            Matrix1x3 expected = new Matrix1x3(12, 
                                               12, 
                                               12);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix2x2ProducesMatrix2x3()
        {
            Matrix2x3 matrix1 = new Matrix2x3(3);
            Matrix2x2 matrix2 = new Matrix2x2(2);
            Matrix2x3 result = matrix1 * matrix2;
            Matrix2x3 expected = new Matrix2x3(12, 12, 
                                               12, 12, 
                                               12, 12);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x2ProducesMatrix3x3()
        {
            Matrix2x3 matrix1 = new Matrix2x3(3);
            Matrix3x2 matrix2 = new Matrix3x2(2);
            Matrix3x3 result = matrix1 * matrix2;
            Matrix3x3 expected = new Matrix3x3(12, 12, 12, 
                                               12, 12, 12, 
                                               12, 12, 12);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x2ProducesMatrix4x3()
        {
            Matrix2x3 matrix1 = new Matrix2x3(3);
            Matrix4x2 matrix2 = new Matrix4x2(2);
            Matrix4x3 result = matrix1 * matrix2;
            Matrix4x3 expected = new Matrix4x3(12, 12, 12, 12, 
                                               12, 12, 12, 12, 
                                               12, 12, 12, 12);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix2x3 matrix)
        {
            matrix = new Matrix2x3(0, 1, 
                                   2, 3, 
                                   4, 5);
        }
    }
}
