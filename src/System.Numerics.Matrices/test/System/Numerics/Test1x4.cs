// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix1x4 structure.
    /// </summary>
    [TestClass]
    public class Test1x4
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix1x4 matrix1x4;

            matrix1x4 = new Matrix1x4();

            for (int x = 0; x < matrix1x4.Columns; x++)
            {
                for (int y = 0; y < matrix1x4.Rows; y++)
                {
                    Assert.AreEqual(0, matrix1x4[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix1x4 = new Matrix1x4(value);

            for (int x = 0; x < matrix1x4.Columns; x++)
            {
                for (int y = 0; y < matrix1x4.Rows; y++)
                {
                    Assert.AreEqual(value, matrix1x4[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix1x4);

            for (int y = 0; y < matrix1x4.Rows; y++)
            {
                for (int x = 0; x < matrix1x4.Columns; x++)
                {
                    Assert.AreEqual(y * matrix1x4.Columns + x, matrix1x4[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix1x4 matrix1x4 = new Matrix1x4();

            for (int x = 0; x < matrix1x4.Columns; x++)
            {
                for (int y = 0; y < matrix1x4.Rows; y++)
                {
                    matrix1x4[x, y] = y * matrix1x4.Columns + x;
                }
            }

            for (int y = 0; y < matrix1x4.Rows; y++)
            {
                for (int x = 0; x < matrix1x4.Columns; x++)
                {
                    Assert.AreEqual(y * matrix1x4.Columns + x, matrix1x4[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix1x4 matrix1x4 = new Matrix1x4();

            Assert.AreEqual(1, matrix1x4.Columns);
            Assert.AreEqual(4, matrix1x4.Rows);
            Assert.AreEqual(Matrix1x4.ColumnCount, matrix1x4.Columns);
            Assert.AreEqual(Matrix1x4.RowCount, matrix1x4.Rows);
        }

        [TestMethod]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix1x4 matrix1x4;

            GenerateFilledMatrixWithValues(out matrix1x4);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix1x4 result = matrix1x4 * c;

                for (int y = 0; y < matrix1x4.Rows; y++)
                {
                    for (int x = 0; x < matrix1x4.Columns; x++)
                    {
                        Assert.AreEqual(matrix1x4[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix1x4 matrix1x4 = new Matrix1x4();

            matrix1x4.M11 = 0;
            matrix1x4.M12 = 1;
            matrix1x4.M13 = 2;
            matrix1x4.M14 = 3;

            Assert.AreEqual(0, matrix1x4.M11, Epsilon);
            Assert.AreEqual(1, matrix1x4.M12, Epsilon);
            Assert.AreEqual(2, matrix1x4.M13, Epsilon);
            Assert.AreEqual(3, matrix1x4.M14, Epsilon);

            Assert.AreEqual(matrix1x4[0, 0], matrix1x4.M11, Epsilon);
            Assert.AreEqual(matrix1x4[0, 1], matrix1x4.M12, Epsilon);
            Assert.AreEqual(matrix1x4[0, 2], matrix1x4.M13, Epsilon);
            Assert.AreEqual(matrix1x4[0, 3], matrix1x4.M14, Epsilon);
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix1x4 value = new Matrix1x4(1);

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
            Matrix1x4 value1 = new Matrix1x4(1);
            Matrix1x4 value2 = new Matrix1x4(99);
            Matrix1x4 result = value1 + value2;

            for (int y = 0; y < Matrix1x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix1x4.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix1x4 value1 = new Matrix1x4(100);
            Matrix1x4 value2 = new Matrix1x4(1);
            Matrix1x4 result = value1 - value2;

            for (int y = 0; y < Matrix1x4.RowCount; y++)
            {
                for (int x = 0; x < Matrix1x4.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix1x4 value1 = new Matrix1x4(100);
            Matrix1x4 value2 = new Matrix1x4(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix1x4 matrix1x4 = new Matrix1x4();

            try
            {
                matrix1x4[-1, 0] = 0;
                Assert.Fail("Matrix1x4[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix1x4[0, -1] = 0;
                Assert.Fail("Matrix1x4[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix1x4[1, 0] = 0;
                Assert.Fail("Matrix1x4[1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix1x4[0, 4] = 0;
                Assert.Fail("Matrix1x4[0, 4] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix2x1ProducesMatrix2x4()
        {
            Matrix1x4 matrix1 = new Matrix1x4(3);
            Matrix2x1 matrix2 = new Matrix2x1(2);
            Matrix2x4 result = matrix1 * matrix2;
            Matrix2x4 expected = new Matrix2x4(6, 6, 
                                               6, 6, 
                                               6, 6, 
                                               6, 6);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x1ProducesMatrix3x4()
        {
            Matrix1x4 matrix1 = new Matrix1x4(3);
            Matrix3x1 matrix2 = new Matrix3x1(2);
            Matrix3x4 result = matrix1 * matrix2;
            Matrix3x4 expected = new Matrix3x4(6, 6, 6, 
                                               6, 6, 6, 
                                               6, 6, 6, 
                                               6, 6, 6);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x1ProducesMatrix4x4()
        {
            Matrix1x4 matrix1 = new Matrix1x4(3);
            Matrix4x1 matrix2 = new Matrix4x1(2);
            Matrix4x4 result = matrix1 * matrix2;
            Matrix4x4 expected = new Matrix4x4(6, 6, 6, 6, 
                                               6, 6, 6, 6, 
                                               6, 6, 6, 6, 
                                               6, 6, 6, 6);

            Assert.AreEqual(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix1x4 matrix)
        {
            matrix = new Matrix1x4(0, 
                                   1, 
                                   2, 
                                   3);
        }
    }
}
