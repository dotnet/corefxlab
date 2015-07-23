// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix3x1 structure.
    /// </summary>
    [TestClass]
    public class Test3x1
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix3x1 matrix3x1;

            matrix3x1 = new Matrix3x1();

            for (int x = 0; x < matrix3x1.Columns; x++)
            {
                for (int y = 0; y < matrix3x1.Rows; y++)
                {
                    Assert.AreEqual(0, matrix3x1[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix3x1 = new Matrix3x1(value);

            for (int x = 0; x < matrix3x1.Columns; x++)
            {
                for (int y = 0; y < matrix3x1.Rows; y++)
                {
                    Assert.AreEqual(value, matrix3x1[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix3x1);

            for (int y = 0; y < matrix3x1.Rows; y++)
            {
                for (int x = 0; x < matrix3x1.Columns; x++)
                {
                    Assert.AreEqual(y * matrix3x1.Columns + x, matrix3x1[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void IndexerGetAndSetValuesCorrectly()
        {
            Matrix3x1 matrix3x1 = new Matrix3x1();

            for (int x = 0; x < matrix3x1.Columns; x++)
            {
                for (int y = 0; y < matrix3x1.Rows; y++)
                {
                    matrix3x1[x, y] = y * matrix3x1.Columns + x;
                }
            }

            for (int y = 0; y < matrix3x1.Rows; y++)
            {
                for (int x = 0; x < matrix3x1.Columns; x++)
                {
                    Assert.AreEqual(y * matrix3x1.Columns + x, matrix3x1[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix3x1 matrix3x1 = new Matrix3x1();

            Assert.AreEqual(3, matrix3x1.Columns);
            Assert.AreEqual(1, matrix3x1.Rows);
            Assert.AreEqual(Matrix3x1.ColumnCount, matrix3x1.Columns);
            Assert.AreEqual(Matrix3x1.RowCount, matrix3x1.Rows);
        }

        [TestMethod]
        public void ScalarMultiplicationIsCorrect()
        {
            Matrix3x1 matrix3x1;

            GenerateFilledMatrixWithValues(out matrix3x1);

            for (double c = -10; c <= 10; c += 0.5)
            {
                Matrix3x1 result = matrix3x1 * c;

                for (int y = 0; y < matrix3x1.Rows; y++)
                {
                    for (int x = 0; x < matrix3x1.Columns; x++)
                    {
                        Assert.AreEqual(matrix3x1[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix3x1 matrix3x1 = new Matrix3x1();

            matrix3x1.M11 = 0;
            matrix3x1.M21 = 1;
            matrix3x1.M31 = 2;

            Assert.AreEqual(0, matrix3x1.M11, Epsilon);
            Assert.AreEqual(1, matrix3x1.M21, Epsilon);
            Assert.AreEqual(2, matrix3x1.M31, Epsilon);

            Assert.AreEqual(matrix3x1[0, 0], matrix3x1.M11, Epsilon);
            Assert.AreEqual(matrix3x1[1, 0], matrix3x1.M21, Epsilon);
            Assert.AreEqual(matrix3x1[2, 0], matrix3x1.M31, Epsilon);
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix3x1 value = new Matrix3x1(1);

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
            Matrix3x1 value1 = new Matrix3x1(1);
            Matrix3x1 value2 = new Matrix3x1(99);
            Matrix3x1 result = value1 + value2;

            for (int y = 0; y < Matrix3x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x1.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix3x1 value1 = new Matrix3x1(100);
            Matrix3x1 value2 = new Matrix3x1(1);
            Matrix3x1 result = value1 - value2;

            for (int y = 0; y < Matrix3x1.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x1.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix3x1 value1 = new Matrix3x1(100);
            Matrix3x1 value2 = new Matrix3x1(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix3x1 matrix3x1 = new Matrix3x1();

            try
            {
                matrix3x1[-1, 0] = 0;
                Assert.Fail("Matrix3x1[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x1[0, -1] = 0;
                Assert.Fail("Matrix3x1[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x1[3, 0] = 0;
                Assert.Fail("Matrix3x1[3, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x1[0, 1] = 0;
                Assert.Fail("Matrix3x1[0, 1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix2x3ProducesMatrix2x1()
        {
            Matrix3x1 matrix1 = new Matrix3x1(3);
            Matrix2x3 matrix2 = new Matrix2x3(2);
            Matrix2x1 result = matrix1 * matrix2;
            Matrix2x1 expected = new Matrix2x1(18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x3ProducesMatrix3x1()
        {
            Matrix3x1 matrix1 = new Matrix3x1(3);
            Matrix3x3 matrix2 = new Matrix3x3(2);
            Matrix3x1 result = matrix1 * matrix2;
            Matrix3x1 expected = new Matrix3x1(18, 18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x3ProducesMatrix4x1()
        {
            Matrix3x1 matrix1 = new Matrix3x1(3);
            Matrix4x3 matrix2 = new Matrix4x3(2);
            Matrix4x1 result = matrix1 * matrix2;
            Matrix4x1 expected = new Matrix4x1(18, 18, 18, 18);

            Assert.AreEqual(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix3x1 matrix)
        {
            matrix = new Matrix3x1(0, 1, 2);
        }
    }
}
