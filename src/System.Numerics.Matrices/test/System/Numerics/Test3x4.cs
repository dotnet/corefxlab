// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix3x4 structure.
    /// </summary>
    [TestClass]
    public class Test3x4
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix3x4 matrix3x4;

            matrix3x4 = new Matrix3x4();

            for (int x = 0; x < matrix3x4.Columns; x++)
            {
                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    Assert.AreEqual(0, matrix3x4[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix3x4 = new Matrix3x4(value);

            for (int x = 0; x < matrix3x4.Columns; x++)
            {
                for (int y = 0; y < matrix3x4.Rows; y++)
                {
                    Assert.AreEqual(value, matrix3x4[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix3x4);

            for (int y = 0; y < matrix3x4.Rows; y++)
            {
                for (int x = 0; x < matrix3x4.Columns; x++)
                {
                    Assert.AreEqual(y * matrix3x4.Columns + x, matrix3x4[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
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
                    Assert.AreEqual(y * matrix3x4.Columns + x, matrix3x4[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            Assert.AreEqual(3, matrix3x4.Columns);
            Assert.AreEqual(4, matrix3x4.Rows);
            Assert.AreEqual(Matrix3x4.ColumnCount, matrix3x4.Columns);
            Assert.AreEqual(Matrix3x4.RowCount, matrix3x4.Rows);
        }

        [TestMethod]
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
                        Assert.AreEqual(matrix3x4[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
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

            Assert.AreEqual(0, matrix3x4.M11, Epsilon);
            Assert.AreEqual(1, matrix3x4.M21, Epsilon);
            Assert.AreEqual(2, matrix3x4.M31, Epsilon);
            Assert.AreEqual(3, matrix3x4.M12, Epsilon);
            Assert.AreEqual(4, matrix3x4.M22, Epsilon);
            Assert.AreEqual(5, matrix3x4.M32, Epsilon);
            Assert.AreEqual(6, matrix3x4.M13, Epsilon);
            Assert.AreEqual(7, matrix3x4.M23, Epsilon);
            Assert.AreEqual(8, matrix3x4.M33, Epsilon);
            Assert.AreEqual(9, matrix3x4.M14, Epsilon);
            Assert.AreEqual(10, matrix3x4.M24, Epsilon);
            Assert.AreEqual(11, matrix3x4.M34, Epsilon);

            Assert.AreEqual(matrix3x4[0, 0], matrix3x4.M11, Epsilon);
            Assert.AreEqual(matrix3x4[1, 0], matrix3x4.M21, Epsilon);
            Assert.AreEqual(matrix3x4[2, 0], matrix3x4.M31, Epsilon);
            Assert.AreEqual(matrix3x4[0, 1], matrix3x4.M12, Epsilon);
            Assert.AreEqual(matrix3x4[1, 1], matrix3x4.M22, Epsilon);
            Assert.AreEqual(matrix3x4[2, 1], matrix3x4.M32, Epsilon);
            Assert.AreEqual(matrix3x4[0, 2], matrix3x4.M13, Epsilon);
            Assert.AreEqual(matrix3x4[1, 2], matrix3x4.M23, Epsilon);
            Assert.AreEqual(matrix3x4[2, 2], matrix3x4.M33, Epsilon);
            Assert.AreEqual(matrix3x4[0, 3], matrix3x4.M14, Epsilon);
            Assert.AreEqual(matrix3x4[1, 3], matrix3x4.M24, Epsilon);
            Assert.AreEqual(matrix3x4[2, 3], matrix3x4.M34, Epsilon);
        }

        [TestMethod]
        public void ColumnAccessorAreCorrect()
        {
            Matrix3x4 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x4 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[0, y], column1[0, y]);
            }

            Matrix1x4 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[1, y], column2[0, y]);
            }
        }

        [TestMethod]
        public void RowAccessorAreCorrect()
        {
            Matrix3x4 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix3x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 0], row1[x, 0]);
            }

            Matrix3x1 row2 = value.Row2;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 1], row2[x, 0]);
            }

            Matrix3x1 row3 = value.Row3;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 2], row3[x, 0]);
            }
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix3x4 value = new Matrix3x4(1);

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
            Matrix3x4 value1 = new Matrix3x4(1);
            Matrix3x4 value2 = new Matrix3x4(99);
            Matrix3x4 result = value1 + value2;

            for (int y = 0; y < Matrix3x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x4.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix3x4 value1 = new Matrix3x4(100);
            Matrix3x4 value2 = new Matrix3x4(1);
            Matrix3x4 result = value1 - value2;

            for (int y = 0; y < Matrix3x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x4.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix3x4 value1 = new Matrix3x4(100);
            Matrix3x4 value2 = new Matrix3x4(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix3x4 matrix3x4 = new Matrix3x4();

            try
            {
                matrix3x4[-1, 0] = 0;
                Assert.Fail("Matrix3x4[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x4[0, -1] = 0;
                Assert.Fail("Matrix3x4[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x4[3, 0] = 0;
                Assert.Fail("Matrix3x4[3, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x4[0, 4] = 0;
                Assert.Fail("Matrix3x4[0, 4] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix1x3ProducesMatrix1x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix1x3 matrix2 = new Matrix1x3(2);
            Matrix1x4 result = matrix1 * matrix2;
            Matrix1x4 expected = new Matrix1x4(18, 
                                               18, 
                                               18, 
                                               18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix2x3ProducesMatrix2x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix2x3 matrix2 = new Matrix2x3(2);
            Matrix2x4 result = matrix1 * matrix2;
            Matrix2x4 expected = new Matrix2x4(18, 18, 
                                               18, 18, 
                                               18, 18, 
                                               18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x3ProducesMatrix3x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix3x3 matrix2 = new Matrix3x3(2);
            Matrix3x4 result = matrix1 * matrix2;
            Matrix3x4 expected = new Matrix3x4(18, 18, 18, 
                                               18, 18, 18, 
                                               18, 18, 18, 
                                               18, 18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x3ProducesMatrix4x4()
        {
            Matrix3x4 matrix1 = new Matrix3x4(3);
            Matrix4x3 matrix2 = new Matrix4x3(2);
            Matrix4x4 result = matrix1 * matrix2;
            Matrix4x4 expected = new Matrix4x4(18, 18, 18, 18, 
                                               18, 18, 18, 18, 
                                               18, 18, 18, 18, 
                                               18, 18, 18, 18);

            Assert.AreEqual(expected, result);
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
