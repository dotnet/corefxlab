// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix3x2 structure.
    /// </summary>
    public class Test3x2
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix3x2 matrix3x2;

            matrix3x2 = new Matrix3x2();

            for (int x = 0; x < matrix3x2.Columns; x++)
            {
                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    Assert.Equal(0, matrix3x2[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix3x2 = new Matrix3x2(value);

            for (int x = 0; x < matrix3x2.Columns; x++)
            {
                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    Assert.Equal(value, matrix3x2[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix3x2);

            for (int y = 0; y < matrix3x2.Rows; y++)
            {
                for (int x = 0; x < matrix3x2.Columns; x++)
                {
                    Assert.Equal(y * matrix3x2.Columns + x, matrix3x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            for (int x = 0; x < matrix3x2.Columns; x++)
            {
                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    matrix3x2[x, y] = y * matrix3x2.Columns + x;
                }
            }

            for (int y = 0; y < matrix3x2.Rows; y++)
            {
                for (int x = 0; x < matrix3x2.Columns; x++)
                {
                    Assert.Equal(y * matrix3x2.Columns + x, matrix3x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            Assert.Equal(3, matrix3x2.Columns);
            Assert.Equal(2, matrix3x2.Rows);
            Assert.Equal(Matrix3x2.ColumnCount, matrix3x2.Columns);
            Assert.Equal(Matrix3x2.RowCount, matrix3x2.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix3x2 matrix3x2;

            GenerateFilledMatrixWithValues(out matrix3x2);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix3x2 result = matrix3x2 * c;

                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    for (int x = 0; x < matrix3x2.Columns; x++)
                    {
                        Assert.Equal(matrix3x2[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            matrix3x2.M11 = 0;
            matrix3x2.M21 = 1;
            matrix3x2.M31 = 2;
            matrix3x2.M12 = 3;
            matrix3x2.M22 = 4;
            matrix3x2.M32 = 5;

            Assert.Equal(0, matrix3x2.M11, Epsilon);
            Assert.Equal(1, matrix3x2.M21, Epsilon);
            Assert.Equal(2, matrix3x2.M31, Epsilon);
            Assert.Equal(3, matrix3x2.M12, Epsilon);
            Assert.Equal(4, matrix3x2.M22, Epsilon);
            Assert.Equal(5, matrix3x2.M32, Epsilon);

            Assert.Equal(matrix3x2[0, 0], matrix3x2.M11, Epsilon);
            Assert.Equal(matrix3x2[1, 0], matrix3x2.M21, Epsilon);
            Assert.Equal(matrix3x2[2, 0], matrix3x2.M31, Epsilon);
            Assert.Equal(matrix3x2[0, 1], matrix3x2.M12, Epsilon);
            Assert.Equal(matrix3x2[1, 1], matrix3x2.M22, Epsilon);
            Assert.Equal(matrix3x2[2, 1], matrix3x2.M32, Epsilon);
        }

        [Fact]
        public void ColumnAccessorAreCorrect()
        {
            Matrix3x2 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x2 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[0, y], column1[0, y]);
            }

            Matrix1x2 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[1, y], column2[0, y]);
            }
        }

        [Fact]
        public void RowAccessorAreCorrect()
        {
            Matrix3x2 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix3x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 0], row1[x, 0]);
            }
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix3x2 value = new Matrix3x2(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix3x2 value1 = new Matrix3x2(1);
            Matrix3x2 value2 = new Matrix3x2(99);
            Matrix3x2 result = value1 + value2;

            for (int y = 0; y < Matrix3x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x2.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix3x2 value1 = new Matrix3x2(100);
            Matrix3x2 value2 = new Matrix3x2(1);
            Matrix3x2 result = value1 - value2;

            for (int y = 0; y < Matrix3x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x2.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix3x2 value1 = new Matrix3x2(100);
            Matrix3x2 value2 = new Matrix3x2(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x2[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x2[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x2[3, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix3x2[0, 2] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix1x3ProducesMatrix1x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix1x3 matrix2 = new Matrix1x3(2);
            Matrix1x2 result = matrix1 * matrix2;
            Matrix1x2 expected = new Matrix1x2(18, 
                                               18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix2x3ProducesMatrix2x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix2x3 matrix2 = new Matrix2x3(2);
            Matrix2x2 result = matrix1 * matrix2;
            Matrix2x2 expected = new Matrix2x2(18, 18, 
                                               18, 18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x3ProducesMatrix3x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix3x3 matrix2 = new Matrix3x3(2);
            Matrix3x2 result = matrix1 * matrix2;
            Matrix3x2 expected = new Matrix3x2(18, 18, 18, 
                                               18, 18, 18);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x3ProducesMatrix4x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix4x3 matrix2 = new Matrix4x3(2);
            Matrix4x2 result = matrix1 * matrix2;
            Matrix4x2 expected = new Matrix4x2(18, 18, 18, 18, 
                                               18, 18, 18, 18);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix3x2 matrix)
        {
            matrix = new Matrix3x2(0, 1, 2, 
                                   3, 4, 5);
        }
    }
}
