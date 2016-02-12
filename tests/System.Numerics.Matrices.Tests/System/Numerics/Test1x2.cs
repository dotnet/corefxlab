// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix1x2 structure.
    /// </summary>
    public class Test1x2
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix1x2 matrix1x2;

            matrix1x2 = new Matrix1x2();

            for (int x = 0; x < matrix1x2.Columns; x++)
            {
                for (int y = 0; y < matrix1x2.Rows; y++)
                {
                    Assert.Equal(0, matrix1x2[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix1x2 = new Matrix1x2(value);

            for (int x = 0; x < matrix1x2.Columns; x++)
            {
                for (int y = 0; y < matrix1x2.Rows; y++)
                {
                    Assert.Equal(value, matrix1x2[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix1x2);

            for (int y = 0; y < matrix1x2.Rows; y++)
            {
                for (int x = 0; x < matrix1x2.Columns; x++)
                {
                    Assert.Equal(y * matrix1x2.Columns + x, matrix1x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix1x2 matrix1x2 = new Matrix1x2();

            for (int x = 0; x < matrix1x2.Columns; x++)
            {
                for (int y = 0; y < matrix1x2.Rows; y++)
                {
                    matrix1x2[x, y] = y * matrix1x2.Columns + x;
                }
            }

            for (int y = 0; y < matrix1x2.Rows; y++)
            {
                for (int x = 0; x < matrix1x2.Columns; x++)
                {
                    Assert.Equal(y * matrix1x2.Columns + x, matrix1x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix1x2 matrix1x2 = new Matrix1x2();

            Assert.Equal(1, matrix1x2.Columns);
            Assert.Equal(2, matrix1x2.Rows);
            Assert.Equal(Matrix1x2.ColumnCount, matrix1x2.Columns);
            Assert.Equal(Matrix1x2.RowCount, matrix1x2.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix1x2 matrix1x2;

            GenerateFilledMatrixWithValues(out matrix1x2);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix1x2 result = matrix1x2 * c;

                for (int y = 0; y < matrix1x2.Rows; y++)
                {
                    for (int x = 0; x < matrix1x2.Columns; x++)
                    {
                        Assert.Equal(matrix1x2[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix1x2 matrix1x2 = new Matrix1x2();

            matrix1x2.M11 = 0;
            matrix1x2.M12 = 1;

            Assert.Equal(0, matrix1x2.M11, Epsilon);
            Assert.Equal(1, matrix1x2.M12, Epsilon);

            Assert.Equal(matrix1x2[0, 0], matrix1x2.M11, Epsilon);
            Assert.Equal(matrix1x2[0, 1], matrix1x2.M12, Epsilon);
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix1x2 value = new Matrix1x2(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix1x2 value1 = new Matrix1x2(1);
            Matrix1x2 value2 = new Matrix1x2(99);
            Matrix1x2 result = value1 + value2;

            for (int y = 0; y < Matrix1x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix1x2.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix1x2 value1 = new Matrix1x2(100);
            Matrix1x2 value2 = new Matrix1x2(1);
            Matrix1x2 result = value1 - value2;

            for (int y = 0; y < Matrix1x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix1x2.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix1x2 value1 = new Matrix1x2(100);
            Matrix1x2 value2 = new Matrix1x2(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix1x2 matrix1x2 = new Matrix1x2();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix1x2[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix1x2[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix1x2[1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix1x2[0, 2] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix2x1ProducesMatrix2x2()
        {
            Matrix1x2 matrix1 = new Matrix1x2(3);
            Matrix2x1 matrix2 = new Matrix2x1(2);
            Matrix2x2 result = matrix1 * matrix2;
            Matrix2x2 expected = new Matrix2x2(6, 6, 
                                               6, 6);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x1ProducesMatrix3x2()
        {
            Matrix1x2 matrix1 = new Matrix1x2(3);
            Matrix3x1 matrix2 = new Matrix3x1(2);
            Matrix3x2 result = matrix1 * matrix2;
            Matrix3x2 expected = new Matrix3x2(6, 6, 6, 
                                               6, 6, 6);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x1ProducesMatrix4x2()
        {
            Matrix1x2 matrix1 = new Matrix1x2(3);
            Matrix4x1 matrix2 = new Matrix4x1(2);
            Matrix4x2 result = matrix1 * matrix2;
            Matrix4x2 expected = new Matrix4x2(6, 6, 6, 6, 
                                               6, 6, 6, 6);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix1x2 matrix)
        {
            matrix = new Matrix1x2(0, 
                                   1);
        }
    }
}
