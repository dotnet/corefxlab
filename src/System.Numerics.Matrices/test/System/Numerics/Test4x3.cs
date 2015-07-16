// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix4x3 structure.
    /// </summary>
    [TestClass]
    public class Test4x3
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix4x3 matrix4x3;

            matrix4x3 = new Matrix4x3();

            for (int x = 0; x < matrix4x3.Columns; x++)
            {
                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    Assert.AreEqual(0, matrix4x3[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix4x3 = new Matrix4x3(value);

            for (int x = 0; x < matrix4x3.Columns; x++)
            {
                for (int y = 0; y < matrix4x3.Rows; y++)
                {
                    Assert.AreEqual(value, matrix4x3[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix4x3);

            for (int y = 0; y < matrix4x3.Rows; y++)
            {
                for (int x = 0; x < matrix4x3.Columns; x++)
                {
                    Assert.AreEqual(y * matrix4x3.Columns + x, matrix4x3[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
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
                    Assert.AreEqual(y * matrix4x3.Columns + x, matrix4x3[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            Assert.AreEqual(4, matrix4x3.Columns);
            Assert.AreEqual(3, matrix4x3.Rows);
            Assert.AreEqual(Matrix4x3.ColumnCount, matrix4x3.Columns);
            Assert.AreEqual(Matrix4x3.RowCount, matrix4x3.Rows);
        }

        [TestMethod]
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
                        Assert.AreEqual(matrix4x3[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
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

            Assert.AreEqual(0, matrix4x3.M11, Epsilon);
            Assert.AreEqual(1, matrix4x3.M21, Epsilon);
            Assert.AreEqual(2, matrix4x3.M31, Epsilon);
            Assert.AreEqual(3, matrix4x3.M41, Epsilon);
            Assert.AreEqual(4, matrix4x3.M12, Epsilon);
            Assert.AreEqual(5, matrix4x3.M22, Epsilon);
            Assert.AreEqual(6, matrix4x3.M32, Epsilon);
            Assert.AreEqual(7, matrix4x3.M42, Epsilon);
            Assert.AreEqual(8, matrix4x3.M13, Epsilon);
            Assert.AreEqual(9, matrix4x3.M23, Epsilon);
            Assert.AreEqual(10, matrix4x3.M33, Epsilon);
            Assert.AreEqual(11, matrix4x3.M43, Epsilon);

            Assert.AreEqual(matrix4x3[0, 0], matrix4x3.M11, Epsilon);
            Assert.AreEqual(matrix4x3[1, 0], matrix4x3.M21, Epsilon);
            Assert.AreEqual(matrix4x3[2, 0], matrix4x3.M31, Epsilon);
            Assert.AreEqual(matrix4x3[3, 0], matrix4x3.M41, Epsilon);
            Assert.AreEqual(matrix4x3[0, 1], matrix4x3.M12, Epsilon);
            Assert.AreEqual(matrix4x3[1, 1], matrix4x3.M22, Epsilon);
            Assert.AreEqual(matrix4x3[2, 1], matrix4x3.M32, Epsilon);
            Assert.AreEqual(matrix4x3[3, 1], matrix4x3.M42, Epsilon);
            Assert.AreEqual(matrix4x3[0, 2], matrix4x3.M13, Epsilon);
            Assert.AreEqual(matrix4x3[1, 2], matrix4x3.M23, Epsilon);
            Assert.AreEqual(matrix4x3[2, 2], matrix4x3.M33, Epsilon);
            Assert.AreEqual(matrix4x3[3, 2], matrix4x3.M43, Epsilon);
        }

        [TestMethod]
        public void ColumnAccessorAreCorrect()
        {
            Matrix4x3 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x3 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[0, y], column1[0, y]);
            }

            Matrix1x3 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[1, y], column2[0, y]);
            }

            Matrix1x3 column3 = value.Column3;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[2, y], column3[0, y]);
            }
        }

        [TestMethod]
        public void RowAccessorAreCorrect()
        {
            Matrix4x3 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix4x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 0], row1[x, 0]);
            }

            Matrix4x1 row2 = value.Row2;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 1], row2[x, 0]);
            }
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix4x3 value = new Matrix4x3(1);

            for (int i = 2; i <= 100; i++)
            {
                if (!hashCodes.Add(value.GetHashCode()))
                {
                    Assert.Fail("Unique hash code generation failure.");
                }

                value *= i;
            }
        }

        [TestMethod]
        public void SimpleAdditionGeneratesCorrectValues()
        {
            Matrix4x3 value1 = new Matrix4x3(1);
            Matrix4x3 value2 = new Matrix4x3(99);
            Matrix4x3 result = value1 + value2;

            for (int y = 0; y < Matrix4x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x3.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix4x3 value1 = new Matrix4x3(100);
            Matrix4x3 value2 = new Matrix4x3(1);
            Matrix4x3 result = value1 - value2;

            for (int y = 0; y < Matrix4x3.RowCount; y++)
            {
                for (int x = 0; x < Matrix4x3.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix4x3 value1 = new Matrix4x3(100);
            Matrix4x3 value2 = new Matrix4x3(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix4x3 matrix4x3 = new Matrix4x3();

            try
            {
                matrix4x3[-1, 0] = 0;
                Assert.Fail("Matrix4x3[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix4x3[0, -1] = 0;
                Assert.Fail("Matrix4x3[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix4x3[4, 0] = 0;
                Assert.Fail("Matrix4x3[4, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix4x3[0, 3] = 0;
                Assert.Fail("Matrix4x3[0, 3] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix1x4ProducesMatrix1x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix1x4 matrix2 = new Matrix1x4(2);
            Matrix1x3 result = matrix1 * matrix2;
            Matrix1x3 expected = new Matrix1x3(24, 
                                               24, 
                                               24);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix2x4ProducesMatrix2x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix2x4 matrix2 = new Matrix2x4(2);
            Matrix2x3 result = matrix1 * matrix2;
            Matrix2x3 expected = new Matrix2x3(24, 24, 
                                               24, 24, 
                                               24, 24);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x4ProducesMatrix3x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix3x4 matrix2 = new Matrix3x4(2);
            Matrix3x3 result = matrix1 * matrix2;
            Matrix3x3 expected = new Matrix3x3(24, 24, 24, 
                                               24, 24, 24, 
                                               24, 24, 24);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x4ProducesMatrix4x3()
        {
            Matrix4x3 matrix1 = new Matrix4x3(3);
            Matrix4x4 matrix2 = new Matrix4x4(2);
            Matrix4x3 result = matrix1 * matrix2;
            Matrix4x3 expected = new Matrix4x3(24, 24, 24, 24, 
                                               24, 24, 24, 24, 
                                               24, 24, 24, 24);

            Assert.AreEqual(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix4x3 matrix)
        {
            matrix = new Matrix4x3( 0,  1,  2,  3, 
                                    4,  5,  6,  7, 
                                    8,  9, 10, 11);
        }
    }
}
