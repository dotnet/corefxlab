// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix2x1 structure.
    /// </summary>
    [TestClass]
    public class Test2x1
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix2x1 matrix2x1;

            matrix2x1 = new Matrix2x1();

            for (int x = 0; x < matrix2x1.Columns; x++)
            {
                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    Assert.AreEqual(0, matrix2x1[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix2x1 = new Matrix2x1(value);

            for (int x = 0; x < matrix2x1.Columns; x++)
            {
                for (int y = 0; y < matrix2x1.Rows; y++)
                {
                    Assert.AreEqual(value, matrix2x1[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix2x1);

            for (int y = 0; y < matrix2x1.Rows; y++)
            {
                for (int x = 0; x < matrix2x1.Columns; x++)
                {
                    Assert.AreEqual(y * matrix2x1.Columns + x, matrix2x1[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
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
                    Assert.AreEqual(y * matrix2x1.Columns + x, matrix2x1[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            Assert.AreEqual(2, matrix2x1.Columns);
            Assert.AreEqual(1, matrix2x1.Rows);
            Assert.AreEqual(Matrix2x1.ColumnCount, matrix2x1.Columns);
            Assert.AreEqual(Matrix2x1.RowCount, matrix2x1.Rows);
        }

        [TestMethod]
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
                        Assert.AreEqual(matrix2x1[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            matrix2x1.M11 = 0;
            matrix2x1.M21 = 1;

            Assert.AreEqual(0, matrix2x1.M11, Epsilon);
            Assert.AreEqual(1, matrix2x1.M21, Epsilon);

            Assert.AreEqual(matrix2x1[0, 0], matrix2x1.M11, Epsilon);
            Assert.AreEqual(matrix2x1[1, 0], matrix2x1.M21, Epsilon);
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix2x1 value = new Matrix2x1(1);

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
            Matrix2x1 value1 = new Matrix2x1(1);
            Matrix2x1 value2 = new Matrix2x1(99);
            Matrix2x1 result = value1 + value2;

            for (int y = 0; y < Matrix2x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x1.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix2x1 value1 = new Matrix2x1(100);
            Matrix2x1 value2 = new Matrix2x1(1);
            Matrix2x1 result = value1 - value2;

            for (int y = 0; y < Matrix2x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix2x1.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix2x1 value1 = new Matrix2x1(100);
            Matrix2x1 value2 = new Matrix2x1(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix2x1 matrix2x1 = new Matrix2x1();

            try
            {
                matrix2x1[-1, 0] = 0;
                Assert.Fail("Matrix2x1[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix2x1[0, -1] = 0;
                Assert.Fail("Matrix2x1[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix2x1[2, 0] = 0;
                Assert.Fail("Matrix2x1[2, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix2x1[0, 1] = 0;
                Assert.Fail("Matrix2x1[0, 1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix2x2ProducesMatrix2x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix2x2 matrix2 = new Matrix2x2(2);
            Matrix2x1 result = matrix1 * matrix2;
            Matrix2x1 expected = new Matrix2x1(12, 12);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x2ProducesMatrix3x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix3x2 matrix2 = new Matrix3x2(2);
            Matrix3x1 result = matrix1 * matrix2;
            Matrix3x1 expected = new Matrix3x1(12, 12, 12);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x2ProducesMatrix4x1()
        {
            Matrix2x1 matrix1 = new Matrix2x1(3);
            Matrix4x2 matrix2 = new Matrix4x2(2);
            Matrix4x1 result = matrix1 * matrix2;
            Matrix4x1 expected = new Matrix4x1(12, 12, 12, 12);

            Assert.AreEqual(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix2x1 matrix)
        {
            matrix = new Matrix2x1(0, 1);
        }
    }
}
