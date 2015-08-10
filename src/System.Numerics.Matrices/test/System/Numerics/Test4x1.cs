// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix4x1 structure.
    /// </summary>
    public class Test4x1
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix4x1 matrix4x1;

            matrix4x1 = new Matrix4x1();

            for (int x = 0; x < matrix4x1.Columns; x++)
            {
                for (int y = 0; y < matrix4x1.Rows; y++)
                {
                    Assert.Equal(0, matrix4x1[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix4x1 = new Matrix4x1(value);

            for (int x = 0; x < matrix4x1.Columns; x++)
            {
                for (int y = 0; y < matrix4x1.Rows; y++)
                {
                    Assert.Equal(value, matrix4x1[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix4x1);

            for (int y = 0; y < matrix4x1.Rows; y++)
            {
                for (int x = 0; x < matrix4x1.Columns; x++)
                {
                    Assert.Equal(y * matrix4x1.Columns + x, matrix4x1[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix4x1 matrix4x1 = new Matrix4x1();

            for (int x = 0; x < matrix4x1.Columns; x++)
            {
                for (int y = 0; y < matrix4x1.Rows; y++)
                {
                    matrix4x1[x, y] = y * matrix4x1.Columns + x;
                }
            }

            for (int y = 0; y < matrix4x1.Rows; y++)
            {
                for (int x = 0; x < matrix4x1.Columns; x++)
                {
                    Assert.Equal(y * matrix4x1.Columns + x, matrix4x1[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix4x1 matrix4x1 = new Matrix4x1();

            Assert.Equal(4, matrix4x1.Columns);
            Assert.Equal(1, matrix4x1.Rows);
            Assert.Equal(Matrix4x1.ColumnCount, matrix4x1.Columns);
            Assert.Equal(Matrix4x1.RowCount, matrix4x1.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix4x1 matrix4x1;

            GenerateFilledMatrixWithValues(out matrix4x1);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix4x1 result = matrix4x1 * c;

                for (int y = 0; y < matrix4x1.Rows; y++)
                {
                    for (int x = 0; x < matrix4x1.Columns; x++)
                    {
                        Assert.Equal(matrix4x1[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix4x1 matrix4x1 = new Matrix4x1();

            matrix4x1.M11 = 0;
            matrix4x1.M21 = 1;
            matrix4x1.M31 = 2;
            matrix4x1.M41 = 3;

            Assert.Equal(0, matrix4x1.M11, Epsilon);
            Assert.Equal(1, matrix4x1.M21, Epsilon);
            Assert.Equal(2, matrix4x1.M31, Epsilon);
            Assert.Equal(3, matrix4x1.M41, Epsilon);

            Assert.Equal(matrix4x1[0, 0], matrix4x1.M11, Epsilon);
            Assert.Equal(matrix4x1[1, 0], matrix4x1.M21, Epsilon);
            Assert.Equal(matrix4x1[2, 0], matrix4x1.M31, Epsilon);
            Assert.Equal(matrix4x1[3, 0], matrix4x1.M41, Epsilon);
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix4x1 value = new Matrix4x1(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix4x1 value1 = new Matrix4x1(1);
            Matrix4x1 value2 = new Matrix4x1(99);
            Matrix4x1 result = value1 + value2;

            for (int y = 0; y < Matrix4x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x1.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix4x1 value1 = new Matrix4x1(100);
            Matrix4x1 value2 = new Matrix4x1(1);
            Matrix4x1 result = value1 - value2;

            for (int y = 0; y < Matrix4x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x1.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix4x1 value1 = new Matrix4x1(100);
            Matrix4x1 value2 = new Matrix4x1(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix4x1 matrix4x1 = new Matrix4x1();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x1[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x1[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x1[4, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x1[0, 1] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix2x4ProducesMatrix2x1()
        {
            Matrix4x1 matrix1 = new Matrix4x1(3);
            Matrix2x4 matrix2 = new Matrix2x4(2);
            Matrix2x1 result = matrix1 * matrix2;
            Matrix2x1 expected = new Matrix2x1(24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x4ProducesMatrix3x1()
        {
            Matrix4x1 matrix1 = new Matrix4x1(3);
            Matrix3x4 matrix2 = new Matrix3x4(2);
            Matrix3x1 result = matrix1 * matrix2;
            Matrix3x1 expected = new Matrix3x1(24, 24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x4ProducesMatrix4x1()
        {
            Matrix4x1 matrix1 = new Matrix4x1(3);
            Matrix4x4 matrix2 = new Matrix4x4(2);
            Matrix4x1 result = matrix1 * matrix2;
            Matrix4x1 expected = new Matrix4x1(24, 24, 24, 24);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix4x1 matrix)
        {
            matrix = new Matrix4x1(0, 1, 2, 3);
        }
    }
}
