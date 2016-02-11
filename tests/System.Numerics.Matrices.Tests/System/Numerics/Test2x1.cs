// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix2x1 structure.
    /// </summary>
    public class Test2x1
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix2x1 matrix2x1;

            matrix2x1 = new Matrix2x1();

            for (int x = 0; x < matrix2x1.Columns; x++)
            {
                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    Assert.Equal(0, matrix2x1[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix2x1 = new Matrix2x1(value);

            for (int x = 0; x < matrix2x1.Columns; x++)
            {
                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    Assert.Equal(value, matrix2x1[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix2x1);

            for (int y = 0; y < matrix2x1.Rows; y++)
            {
                for (int x = 0; x < matrix2x1.Columns; x++)
                {
                    Assert.Equal(y * matrix2x1.Columns + x, matrix2x1[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            for (int x = 0; x < matrix2x1.Columns; x++)
            {
                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    matrix2x1[x, y] = y * matrix2x1.Columns + x;
                }
            }

            for (int y = 0; y < matrix2x1.Rows; y++)
            {
                for (int x = 0; x < matrix2x1.Columns; x++)
                {
                    Assert.Equal(y * matrix2x1.Columns + x, matrix2x1[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            Assert.Equal(2, matrix2x1.Columns);
            Assert.Equal(1, matrix2x1.Rows);
            Assert.Equal(Matrix2x1.ColumnCount, matrix2x1.Columns);
            Assert.Equal(Matrix2x1.RowCount, matrix2x1.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix2x1 matrix2x1;

            GenerateFilledMatrixWithValues(out matrix2x1);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix2x1 result = matrix2x1 * c;

                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    for (int x = 0; x < matrix2x1.Columns; x++)
                    {
                        Assert.Equal(matrix2x1[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            matrix2x1.M11 = 0;
            matrix2x1.M21 = 1;

            Assert.Equal(0, matrix2x1.M11, Epsilon);
            Assert.Equal(1, matrix2x1.M21, Epsilon);

            Assert.Equal(matrix2x1[0, 0], matrix2x1.M11, Epsilon);
            Assert.Equal(matrix2x1[1, 0], matrix2x1.M21, Epsilon);
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix2x1 value = new Matrix2x1(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix2x1 value1 = new Matrix2x1(1);
            Matrix2x1 value2 = new Matrix2x1(99);
            Matrix2x1 result = value1 + value2;

            for (int y = 0; y < Matrix2x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x1.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix2x1 value1 = new Matrix2x1(100);
            Matrix2x1 value2 = new Matrix2x1(1);
            Matrix2x1 result = value1 - value2;

            for (int y = 0; y < Matrix2x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x1.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix2x1 value1 = new Matrix2x1(100);
            Matrix2x1 value2 = new Matrix2x1(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x1[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x1[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x1[2, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix2x1[0, 1] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix2x2ProducesMatrix2x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix2x2 matrix2 = new Matrix2x2(2);
            Matrix2x1 result = matrix1 * matrix2;
            Matrix2x1 expected = new Matrix2x1(12, 12);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x2ProducesMatrix3x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix3x2 matrix2 = new Matrix3x2(2);
            Matrix3x1 result = matrix1 * matrix2;
            Matrix3x1 expected = new Matrix3x1(12, 12, 12);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x2ProducesMatrix4x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix4x2 matrix2 = new Matrix4x2(2);
            Matrix4x1 result = matrix1 * matrix2;
            Matrix4x1 expected = new Matrix4x1(12, 12, 12, 12);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix2x1 matrix)
        {
            matrix = new Matrix2x1(0, 1);
        }
    }
}
