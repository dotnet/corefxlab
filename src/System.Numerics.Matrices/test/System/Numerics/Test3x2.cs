// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Matrices.Tests
{
    /// <summary>
    /// Tests for the Matrix3x2 structure.
    /// </summary>
    [TestClass]
    public class Test3x2
    {
        const double Epsilon = Double.Epsilon * 10;

        [TestMethod]
        public void ConstructorValuesAreAccessibleByIndexer()
        {
            Matrix3x2 matrix3x2;

            matrix3x2 = new Matrix3x2();

            for (int x = 0; x < matrix3x2.Columns; x++)
            {
                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    Assert.AreEqual(0, matrix3x2[x, y], Epsilon);
                }
            }

            double value = 33.33;
            matrix3x2 = new Matrix3x2(value);

            for (int x = 0; x < matrix3x2.Columns; x++)
            {
                for (int y = 0; y < matrix3x2.Rows; y++)
                {
                    Assert.AreEqual(value, matrix3x2[x, y], Epsilon);
                }
            }

            GenerateFilledMatrixWithValues(out matrix3x2);

            for (int y = 0; y < matrix3x2.Rows; y++)
            {
                for (int x = 0; x < matrix3x2.Columns; x++)
                {
                    Assert.AreEqual(y * matrix3x2.Columns + x, matrix3x2[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
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
                    Assert.AreEqual(y * matrix3x2.Columns + x, matrix3x2[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void ConstantValuesAreCorrect()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            Assert.AreEqual(3, matrix3x2.Columns);
            Assert.AreEqual(2, matrix3x2.Rows);
            Assert.AreEqual(Matrix3x2.ColumnCount, matrix3x2.Columns);
            Assert.AreEqual(Matrix3x2.RowCount, matrix3x2.Rows);
        }

        [TestMethod]
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
                        Assert.AreEqual(matrix3x2[x, y] * c, result[x, y], Epsilon);
                    }
                }
            }
        }

        [TestMethod]
        public void MemberGetAndSetValuesCorrectly()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            matrix3x2.M11 = 0;
            matrix3x2.M21 = 1;
            matrix3x2.M31 = 2;
            matrix3x2.M12 = 3;
            matrix3x2.M22 = 4;
            matrix3x2.M32 = 5;

            Assert.AreEqual(0, matrix3x2.M11, Epsilon);
            Assert.AreEqual(1, matrix3x2.M21, Epsilon);
            Assert.AreEqual(2, matrix3x2.M31, Epsilon);
            Assert.AreEqual(3, matrix3x2.M12, Epsilon);
            Assert.AreEqual(4, matrix3x2.M22, Epsilon);
            Assert.AreEqual(5, matrix3x2.M32, Epsilon);

            Assert.AreEqual(matrix3x2[0, 0], matrix3x2.M11, Epsilon);
            Assert.AreEqual(matrix3x2[1, 0], matrix3x2.M21, Epsilon);
            Assert.AreEqual(matrix3x2[2, 0], matrix3x2.M31, Epsilon);
            Assert.AreEqual(matrix3x2[0, 1], matrix3x2.M12, Epsilon);
            Assert.AreEqual(matrix3x2[1, 1], matrix3x2.M22, Epsilon);
            Assert.AreEqual(matrix3x2[2, 1], matrix3x2.M32, Epsilon);
        }

        [TestMethod]
        public void ColumnAccessorAreCorrect()
        {
            Matrix3x2 value;
            GenerateFilledMatrixWithValues(out value);

            Matrix1x2 column1 = value.Column1;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[0, y], column1[0, y]);
            }

            Matrix1x2 column2 = value.Column2;
            for (int y = 0; y < value.Rows; y++)
            {
                Assert.AreEqual(value[1, y], column2[0, y]);
            }
        }

        [TestMethod]
        public void RowAccessorAreCorrect()
        {
            Matrix3x2 value;
            GenerateFilledMatrixWithValues(out value);


            Matrix3x1 row1 = value.Row1;
            for (int x = 0; x < value.Columns; x++)
            {
                Assert.AreEqual(value[x, 0], row1[x, 0]);
            }
        }

        [TestMethod]
        public void HashCodeGenerationWorksCorrectly()
        {
            HashSet<int> hashCodes = new HashSet<int>();
            Matrix3x2 value = new Matrix3x2(1);

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
            Matrix3x2 value1 = new Matrix3x2(1);
            Matrix3x2 value2 = new Matrix3x2(99);
            Matrix3x2 result = value1 + value2;

            for (int y = 0; y < Matrix3x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x2.ColumnCount; x++)
                {
                    Assert.AreEqual(1 + 99, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void SimpleSubtractionGeneratesCorrectValues()
        {
            Matrix3x2 value1 = new Matrix3x2(100);
            Matrix3x2 value2 = new Matrix3x2(1);
            Matrix3x2 result = value1 - value2;

            for (int y = 0; y < Matrix3x2.RowCount; y++)
            {
                for (int x = 0; x < Matrix3x2.ColumnCount; x++)
                {
                    Assert.AreEqual(100 - 1, result[x, y], Epsilon);
                }
            }
        }

        [TestMethod]
        public void EqualityOperatorWorksCorrectly()
        {
            Matrix3x2 value1 = new Matrix3x2(100);
            Matrix3x2 value2 = new Matrix3x2(50) * 2;

            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1 == value2, "Equality operator failed.");
        }

        [TestMethod]
        public void AccessorThrowsWhenOutOfBounds()
        {
            Matrix3x2 matrix3x2 = new Matrix3x2();

            try
            {
                matrix3x2[-1, 0] = 0;
                Assert.Fail("Matrix3x2[-1, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x2[0, -1] = 0;
                Assert.Fail("Matrix3x2[0, -1] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x2[3, 0] = 0;
                Assert.Fail("Matrix3x2[3, 0] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                matrix3x2[0, 2] = 0;
                Assert.Fail("Matrix3x2[0, 2] did not throw when it should have.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [TestMethod]
        public void MuliplyByMatrix1x3ProducesMatrix1x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix1x3 matrix2 = new Matrix1x3(2);
            Matrix1x2 result = matrix1 * matrix2;
            Matrix1x2 expected = new Matrix1x2(18, 
                                               18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix2x3ProducesMatrix2x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix2x3 matrix2 = new Matrix2x3(2);
            Matrix2x2 result = matrix1 * matrix2;
            Matrix2x2 expected = new Matrix2x2(18, 18, 
                                               18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix3x3ProducesMatrix3x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix3x3 matrix2 = new Matrix3x3(2);
            Matrix3x2 result = matrix1 * matrix2;
            Matrix3x2 expected = new Matrix3x2(18, 18, 18, 
                                               18, 18, 18);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void MuliplyByMatrix4x3ProducesMatrix4x2()
        {
            Matrix3x2 matrix1 = new Matrix3x2(3);
            Matrix4x3 matrix2 = new Matrix4x3(2);
            Matrix4x2 result = matrix1 * matrix2;
            Matrix4x2 expected = new Matrix4x2(18, 18, 18, 18, 
                                               18, 18, 18, 18);

            Assert.AreEqual(expected, result);
        }

        private void GenerateFilledMatrixWithValues(out Matrix3x2 matrix)
        {
            matrix = new Matrix3x2(0, 1, 2, 
                                   3, 4, 5);
        }
    }
}
