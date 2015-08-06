// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Xunit;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix4x2 structure.
    /// </summary>
    public class Test4x2
    {
        const int Epsilon = 10;

        [Fact]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix4x2 matrix4x2;

            matrix4x2 = new Matrix4x2();

            for (int x = 0; x < matrix4x2.Columns; x++)
            {
                for (int y = 0; y < matrix4x2.Rows; y++)
                {
                    Assert.Equal(0, matrix4x2[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix4x2 = new Matrix4x2(value);

            for (int x = 0; x < matrix4x2.Columns; x++)
            {
                for (int y = 0; y < matrix4x2.Rows; y++)
                {
                    Assert.Equal(value, matrix4x2[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix4x2);

            for (int y = 0; y < matrix4x2.Rows; y++)
            {
                for (int x = 0; x < matrix4x2.Columns; x++)
                {
                    Assert.Equal(y * matrix4x2.Columns + x, matrix4x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix4x2 matrix4x2 = new Matrix4x2();

            for (int x = 0; x < matrix4x2.Columns; x++)
            {
                for (int y = 0; y < matrix4x2.Rows; y++)
                {
                    matrix4x2[x, y] = y * matrix4x2.Columns + x;
                }
            }

            for (int y = 0; y < matrix4x2.Rows; y++)
            {
                for (int x = 0; x < matrix4x2.Columns; x++)
                {
                    Assert.Equal(y * matrix4x2.Columns + x, matrix4x2[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void ConstantValuesAreCorrect()
        {
            Matrix4x2 matrix4x2 = new Matrix4x2();

            Assert.Equal(4, matrix4x2.Columns);
            Assert.Equal(2, matrix4x2.Rows);
            Assert.Equal(Matrix4x2.ColumnCount, matrix4x2.Columns);
            Assert.Equal(Matrix4x2.RowCount, matrix4x2.Rows);
        }

        [Fact]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix4x2 matrix4x2;

            GenerateFilledMatrixWithValues(out matrix4x2);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix4x2 result = matrix4x2 * c;

                for (int y = 0; y < matrix4x2.Rows; y++)
                {
                    for (int x = 0; x < matrix4x2.Columns; x++)
                    {
                        Assert.Equal(matrix4x2[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [Fact]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix4x2 matrix4x2 = new Matrix4x2();

            matrix4x2.M11 = 0;
            matrix4x2.M21 = 1;
            matrix4x2.M31 = 2;
            matrix4x2.M41 = 3;
            matrix4x2.M12 = 4;
            matrix4x2.M22 = 5;
            matrix4x2.M32 = 6;
            matrix4x2.M42 = 7;

            Assert.Equal(0, matrix4x2.M11, Epsilon);
            Assert.Equal(1, matrix4x2.M21, Epsilon);
            Assert.Equal(2, matrix4x2.M31, Epsilon);
            Assert.Equal(3, matrix4x2.M41, Epsilon);
            Assert.Equal(4, matrix4x2.M12, Epsilon);
            Assert.Equal(5, matrix4x2.M22, Epsilon);
            Assert.Equal(6, matrix4x2.M32, Epsilon);
            Assert.Equal(7, matrix4x2.M42, Epsilon);

            Assert.Equal(matrix4x2[0, 0], matrix4x2.M11, Epsilon);
            Assert.Equal(matrix4x2[1, 0], matrix4x2.M21, Epsilon);
            Assert.Equal(matrix4x2[2, 0], matrix4x2.M31, Epsilon);
            Assert.Equal(matrix4x2[3, 0], matrix4x2.M41, Epsilon);
            Assert.Equal(matrix4x2[0, 1], matrix4x2.M12, Epsilon);
            Assert.Equal(matrix4x2[1, 1], matrix4x2.M22, Epsilon);
            Assert.Equal(matrix4x2[2, 1], matrix4x2.M32, Epsilon);
            Assert.Equal(matrix4x2[3, 1], matrix4x2.M42, Epsilon);
        }

        [Fact]
        public void ColumnAccessorAreCorrect()
        {
            Matrix4x2 value;
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

            Matrix1x2 column3 = value.Column3;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.Equal(value[2, y], column3[0, y]);
            }
        }

        [Fact]
        public void RowAccessorAreCorrect()
        {
            Matrix4x2 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix4x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.Equal(value[x, 0], row1[x, 0]);
            }
        }

        [Fact]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix4x2 value = new Matrix4x2(1);

            for (int i = 2; i <= 100; i++)
            {
                Assert.True(hashCodes.Add(value.GetHashCode()), "Unique hash code generation failure.");

                value *= i;
            }
        }

        [Fact]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix4x2 value1 = new Matrix4x2(1);
            Matrix4x2 value2 = new Matrix4x2(99);
            Matrix4x2 result = value1 + value2;

            for (int y = 0; y < Matrix4x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x2.ColumnCount; x++)
                {
                    Assert.Equal(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix4x2 value1 = new Matrix4x2(100);
            Matrix4x2 value2 = new Matrix4x2(1);
            Matrix4x2 result = value1 - value2;

            for (int y = 0; y < Matrix4x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x2.ColumnCount; x++)
                {
                    Assert.Equal(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [Fact]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix4x2 value1 = new Matrix4x2(100);
            Matrix4x2 value2 = new Matrix4x2(50) * 2;

            Assert.Equal(value1, value2);
            Assert.True(value1 == value2, "Equality operator failed.");
        }

        [Fact]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix4x2 matrix4x2 = new Matrix4x2();

            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x2[-1, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x2[0, -1] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x2[4, 0] = 0; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { matrix4x2[0, 2] = 0; });
        }

        [Fact]
        public void MuliplyByMatrix1x4ProducesMatrix1x2()
        {
            Matrix4x2 matrix1 = new Matrix4x2(3);
            Matrix1x4 matrix2 = new Matrix1x4(2);
            Matrix1x2 result = matrix1 * matrix2;
            Matrix1x2 expected = new Matrix1x2(24, 
                                               24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix2x4ProducesMatrix2x2()
        {
            Matrix4x2 matrix1 = new Matrix4x2(3);
            Matrix2x4 matrix2 = new Matrix2x4(2);
            Matrix2x2 result = matrix1 * matrix2;
            Matrix2x2 expected = new Matrix2x2(24, 24, 
                                               24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix3x4ProducesMatrix3x2()
        {
            Matrix4x2 matrix1 = new Matrix4x2(3);
            Matrix3x4 matrix2 = new Matrix3x4(2);
            Matrix3x2 result = matrix1 * matrix2;
            Matrix3x2 expected = new Matrix3x2(24, 24, 24, 
                                               24, 24, 24);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void MuliplyByMatrix4x4ProducesMatrix4x2()
        {
            Matrix4x2 matrix1 = new Matrix4x2(3);
            Matrix4x4 matrix2 = new Matrix4x4(2);
            Matrix4x2 result = matrix1 * matrix2;
            Matrix4x2 expected = new Matrix4x2(24, 24, 24, 24, 
                                               24, 24, 24, 24);

            Assert.Equal(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix4x2 matrix)
        {
            matrix = new Matrix4x2(0, 1, 2, 3, 
                                   4, 5, 6, 7);
        }
    }
}
