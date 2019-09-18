// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Xunit;

namespace Microsoft.Data.Tests
{
    public partial class DataFrameTests
    {
        public static DataFrame MakeDataFrameWithTwoColumns(int length, bool withNulls = true)
        {
            BaseColumn dataFrameColumn1 = new PrimitiveColumn<int>("Int1", Enumerable.Range(0, length).Select(x => x));
            BaseColumn dataFrameColumn2 = new PrimitiveColumn<int>("Int2", Enumerable.Range(10, length).Select(x => x));
            if (withNulls)
            {
                dataFrameColumn1[length / 2] = null;
                dataFrameColumn2[length / 2] = null;
            }
            Data.DataFrame dataFrame = new Data.DataFrame();
            dataFrame.InsertColumn(0, dataFrameColumn1);
            dataFrame.InsertColumn(1, dataFrameColumn2);
            return dataFrame;
        }

        public static BaseColumn CreateArrowStringColumn(int length)
        {
            byte[] foo = new byte[] { 102, 111, 111 }; // bytes for the string "foo"

            byte[] dataMemory = new byte[length * 3];
            byte[] nullMemory = new byte[length];
            byte[] offsetMemory = new byte[(length + 1) * 4];

            // Initialize offset with 0 as the first value
            offsetMemory[0] = 0;
            offsetMemory[1] = 0;
            offsetMemory[2] = 0;
            offsetMemory[3] = 0;

            // Append "foo" length times
            for (int i = 0; i < length; i++)
            {
                int dataMemoryIndex = i * 3;
                dataMemory[dataMemoryIndex++] = 102;
                dataMemory[dataMemoryIndex++] = 111;
                dataMemory[dataMemoryIndex++] = 111;
                nullMemory[i] = 0;
                int offsetIndex = (i + 1) * 4;
                offsetMemory[offsetIndex++] = (byte)(3 * (i + 1));
                offsetMemory[offsetIndex++] = 0;
                offsetMemory[offsetIndex++] = 0;
                offsetMemory[offsetIndex++] = 0;
            }
            return new ArrowStringColumn("ArrowString", dataMemory, offsetMemory, nullMemory, length, 0);
        }

        public static DataFrame MakeDataFrameWithAllColumnTypes(int length, bool withNulls = true)
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(length, withNulls);
            BaseColumn arrowStringColumn = CreateArrowStringColumn(length);
            df.InsertColumn(df.ColumnCount, arrowStringColumn);
            return df;
        }

        public static DataFrame MakeDataFrameWithAllMutableColumnTypes(int length, bool withNulls = true)
        {
            DataFrame df = MakeDataFrameWithNumericAndStringColumns(length, withNulls);
            BaseColumn boolColumn = new PrimitiveColumn<bool>("Bool", Enumerable.Range(0, length).Select(x => x % 2 == 0));
            df.InsertColumn(df.ColumnCount, boolColumn);
            if (withNulls)
            {
                boolColumn[length / 2] = null;
            }
            return df;
        }

        public static DataFrame MakeDataFrameWithNumericAndBoolColumns(int length)
        {
            DataFrame df = MakeDataFrameWithNumericColumns(length);
            BaseColumn boolColumn = new PrimitiveColumn<bool>("Bool", Enumerable.Range(0, length).Select(x => x % 2 == 0));
            df.InsertColumn(df.ColumnCount, boolColumn);
            boolColumn[length / 2] = null;
            return df;
        }

        public static DataFrame MakeDataFrameWithNumericAndStringColumns(int length, bool withNulls = true)
        {
            DataFrame df = MakeDataFrameWithNumericColumns(length, withNulls);
            BaseColumn stringColumn = new StringColumn("String", Enumerable.Range(0, length).Select(x => x.ToString()));
            df.InsertColumn(df.ColumnCount, stringColumn);
            if (withNulls)
            {
                stringColumn[length / 2] = null;
            }

            BaseColumn charColumn = new PrimitiveColumn<char>("Char", Enumerable.Range(0, length).Select(x => (char)(x + 65)));
            df.InsertColumn(df.ColumnCount, charColumn);
            if (withNulls)
            {
                charColumn[length / 2] = null;
            }
            return df;
        }

        public static DataFrame MakeDataFrameWithNumericColumns(int length, bool withNulls = true)
        {
            BaseColumn byteColumn = new PrimitiveColumn<byte>("Byte", Enumerable.Range(0, length).Select(x => (byte)x));
            BaseColumn decimalColumn = new PrimitiveColumn<decimal>("Decimal", Enumerable.Range(0, length).Select(x => (decimal)x));
            BaseColumn doubleColumn = new PrimitiveColumn<double>("Double", Enumerable.Range(0, length).Select(x => (double)x));
            BaseColumn floatColumn = new PrimitiveColumn<float>("Float", Enumerable.Range(0, length).Select(x => (float)x));
            BaseColumn intColumn = new PrimitiveColumn<int>("Int", Enumerable.Range(0, length).Select(x => x));
            BaseColumn longColumn = new PrimitiveColumn<long>("Long", Enumerable.Range(0, length).Select(x => (long)x));
            BaseColumn sbyteColumn = new PrimitiveColumn<sbyte>("Sbyte", Enumerable.Range(0, length).Select(x => (sbyte)x));
            BaseColumn shortColumn = new PrimitiveColumn<short>("Short", Enumerable.Range(0, length).Select(x => (short)x));
            BaseColumn uintColumn = new PrimitiveColumn<uint>("Uint", Enumerable.Range(0, length).Select(x => (uint)x));
            BaseColumn ulongColumn = new PrimitiveColumn<ulong>("Ulong", Enumerable.Range(0, length).Select(x => (ulong)x));
            BaseColumn ushortColumn = new PrimitiveColumn<ushort>("Ushort", Enumerable.Range(0, length).Select(x => (ushort)x));

            DataFrame dataFrame = new DataFrame(new List<BaseColumn> { byteColumn, decimalColumn, doubleColumn, floatColumn, intColumn, longColumn, sbyteColumn, shortColumn, uintColumn, ulongColumn, ushortColumn });

            if (withNulls)
            {
                for (int i = 0; i < dataFrame.ColumnCount; i++)
                {
                    dataFrame.Column(i)[length / 2] = null;
                }
            }
            return dataFrame;
        }

        public static DataFrame MakeDataFrame<T1, T2>(int length, bool withNulls = true)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            BaseColumn baseColumn1 = new PrimitiveColumn<T1>("Column1", Enumerable.Range(0, length).Select(x => (T1)Convert.ChangeType(x % 2 == 0 ? 0 : 1, typeof(T1))));
            BaseColumn baseColumn2 = new PrimitiveColumn<T2>("Column2", Enumerable.Range(0, length).Select(x => (T2)Convert.ChangeType(x % 2 == 0 ? 0 : 1, typeof(T2))));
            DataFrame dataFrame = new DataFrame(new List<BaseColumn> { baseColumn1, baseColumn2 });

            if (withNulls)
            {
                for (int i = 0; i < dataFrame.ColumnCount; i++)
                {
                    dataFrame.Column(i)[length / 2] = null;
                }
            }

            return dataFrame;
        }

        [Fact]
        public void TestIndexer()
        {
            DataFrame dataFrame = MakeDataFrameWithTwoColumns(length: 10);
            var foo = dataFrame[0, 0];
            Assert.Equal(0, dataFrame[0, 0]);
            Assert.Equal(11, dataFrame[1, 1]);
            Assert.Equal(2, dataFrame.Columns.Count);
            Assert.Equal("Int1", dataFrame.Columns[0]);

            var headList = dataFrame.Head(5);
            Assert.Equal(14, (int)headList[4][1]);

            var tailList = dataFrame.Tail(5);
            Assert.Equal(19, (int)tailList[4][1]);

            dataFrame[2, 1] = 1000;
            Assert.Equal(1000, dataFrame[2, 1]);

            var row = dataFrame[4];
            Assert.Equal(14, (int)row[1]);

            var column = dataFrame["Int2"] as PrimitiveColumn<int>;
            Assert.Equal(1000, (int)column[2]);

            Assert.Throws<ArgumentException>(() => dataFrame["Int5"]);
        }

        [Fact]
        public void ColumnAndTableCreationTest()
        {
            BaseColumn intColumn = new PrimitiveColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            BaseColumn floatColumn = new PrimitiveColumn<float>("FloatColumn", Enumerable.Range(0, 10).Select(x => (float)x));
            DataFrame dataFrame = new DataFrame();
            dataFrame.InsertColumn(0, intColumn);
            dataFrame.InsertColumn(1, floatColumn);
            Assert.Equal(10, dataFrame.RowCount);
            Assert.Equal(2, dataFrame.ColumnCount);
            Assert.Equal(10, dataFrame.Column(0).Length);
            Assert.Equal("IntColumn", dataFrame.Column(0).Name);
            Assert.Equal(10, dataFrame.Column(1).Length);
            Assert.Equal("FloatColumn", dataFrame.Column(1).Name);

            BaseColumn bigColumn = new PrimitiveColumn<float>("BigColumn", Enumerable.Range(0, 11).Select(x => (float)x));
            BaseColumn repeatedName = new PrimitiveColumn<float>("FloatColumn", Enumerable.Range(0, 10).Select(x => (float)x));
            Assert.Throws<ArgumentException>(() => dataFrame.InsertColumn(2, bigColumn));
            Assert.Throws<ArgumentException>(() => dataFrame.InsertColumn(2, repeatedName));
            Assert.Throws<ArgumentOutOfRangeException>(() => dataFrame.InsertColumn(10, repeatedName));

            Assert.Equal(2, dataFrame.ColumnCount);
            BaseColumn intColumnCopy = new PrimitiveColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            Assert.Throws<ArgumentException>(() => dataFrame.SetColumn(1, intColumnCopy));

            BaseColumn differentIntColumn = new PrimitiveColumn<int>("IntColumn1", Enumerable.Range(0, 10).Select(x => x));
            dataFrame.SetColumn(1, differentIntColumn);
            Assert.True(object.ReferenceEquals(differentIntColumn, dataFrame.Column(1)));

            dataFrame.RemoveColumn(1);
            Assert.Equal(1, dataFrame.ColumnCount);
            Assert.True(ReferenceEquals(intColumn, dataFrame.Column(0)));
        }

        [Fact]
        public void InsertAndRemoveColumnTests()
        {
            DataFrame dataFrame = MakeDataFrameWithAllMutableColumnTypes(10);
            BaseColumn intColumn = new PrimitiveColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            BaseColumn charColumn = dataFrame["Char"];
            int insertedIndex = dataFrame.ColumnCount;
            dataFrame.InsertColumn(dataFrame.ColumnCount, intColumn);
            dataFrame.RemoveColumn(0);
            BaseColumn intColumn_1 = dataFrame["IntColumn"];
            BaseColumn charColumn_1 = dataFrame["Char"];
            Assert.True(ReferenceEquals(intColumn, intColumn_1));
            Assert.True(ReferenceEquals(charColumn, charColumn_1));
        }

        [Fact]
        public void TestBinaryOperations()
        {
            DataFrame df = MakeDataFrameWithTwoColumns(12);
            // Binary ops always return a copy
            Assert.Equal(5, df.Add(5)[0, 0]);
            IReadOnlyList<int> listOfInts = new List<int>() { 5, 5 };
            Assert.Equal(5, df.Add(listOfInts)[0, 0]);
            Assert.Equal(-5, df.Subtract(5)[0, 0]);
            Assert.Equal(-5, df.Subtract(listOfInts)[0, 0]);
            Assert.Equal(5, df.Multiply(5)[1, 0]);
            Assert.Equal(5, df.Multiply(listOfInts)[1, 0]);
            Assert.Equal(1, df.Divide(5)[5, 0]);
            Assert.Equal(1, df.Divide(listOfInts)[5, 0]);
            Assert.Equal(0, df.Modulo(5)[5, 0]);
            Assert.Equal(0, df.Modulo(listOfInts)[5, 0]);
            Assert.Throws<NotSupportedException>(() => df.And(5)[5, 0]);
            Assert.Throws<NotSupportedException>(() => df.And(listOfInts)[5, 0]);
            Assert.Throws<NotSupportedException>(() => df.Or(5)[5, 0]);
            Assert.Throws<NotSupportedException>(() => df.Or(listOfInts)[5, 0]);
            Assert.Throws<NotSupportedException>(() => df.Xor(5)[5, 0]);
            Assert.Equal(2, df.LeftShift(1)[1, 0]);
            Assert.Equal(1, df.RightShift(1)[2, 0]);
            Assert.Equal(true, df.Equals(5)[5, 0]);
            Assert.Equal(true, df.Equals(listOfInts)[5, 0]);
            Assert.Equal(true, df.NotEquals(5)[4, 0]);
            Assert.Equal(true, df.NotEquals(listOfInts)[4, 0]);
            Assert.Equal(true, df.GreaterThanOrEqual(5)[7, 0]);
            Assert.Equal(true, df.GreaterThanOrEqual(listOfInts)[7, 0]);
            Assert.Equal(true, df.LessThanOrEqual(5)[4, 0]);
            Assert.Equal(true, df.LessThanOrEqual(listOfInts)[4, 0]);
            Assert.Equal(false, df.GreaterThan(5)[5, 0]);
            Assert.Equal(false, df.GreaterThan(listOfInts)[5, 0]);
            Assert.Equal(false, df.LessThan(5)[5, 0]);
            Assert.Equal(false, df.LessThan(listOfInts)[5, 0]);

            // The original DF is untouched
            Assert.Equal(0, df[0, 0]);
        }

        [Fact]
        public void TestBinaryOperationsWithColumns()
        {
            int length = 10;
            var df1 = MakeDataFrameWithNumericColumns(length);
            var df2 = MakeDataFrameWithNumericColumns(length);

            BaseColumn newColumn;
            BaseColumn verify;
            for (int i = 0; i < df1.ColumnCount; i++)
            {
                newColumn = df1[df1.Column(i).Name] + df2[df2.Column(i).Name];
                verify = newColumn.Equals(df1.Column(i) * 2);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] - df2[df2.Column(i).Name];
                verify = newColumn.Equals(0);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] * df2[df2.Column(i).Name];
                verify = newColumn.Equals(df1.Column(i) * df1.Column(i));
                Assert.Equal(true, verify[0]);

                var df1Column = df1.Column(i) + 1;
                var df2Column = df2.Column(i) + 1;
                newColumn = df1Column / df2Column;
                verify = newColumn.Equals(1);
                Assert.Equal(true, verify[0]);

                newColumn = df1Column % df2Column;
                verify = newColumn.Equals(0);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] == df2[df2.Column(i).Name];
                verify = newColumn.Equals(true);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] != df2[df2.Column(i).Name];
                verify = newColumn.Equals(false);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] >= df2[df2.Column(i).Name];
                verify = newColumn.Equals(true);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] <= df2[df2.Column(i).Name];
                verify = newColumn.Equals(true);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] > df2[df2.Column(i).Name];
                verify = newColumn.Equals(false);
                Assert.Equal(true, verify[0]);

                newColumn = df1[df1.Column(i).Name] < df2[df2.Column(i).Name];
                verify = newColumn.Equals(false);
                Assert.Equal(true, verify[0]);
            }
        }

        [Fact]
        public void TestBinaryOperationsWithConversions()
        {
            DataFrame df = DataFrameTests.MakeDataFrameWithTwoColumns(10);

            // Add a double to an int column
            DataFrame dfd = df.Add(5.0f);
            var dtype = dfd.Column(0).DataType;
            Assert.True(dtype == typeof(double));

            // Add a decimal to an int column
            DataFrame dfm = df.Add(5.0m);
            dtype = dfm.Column(0).DataType;
            Assert.True(dtype == typeof(decimal));

            // int + bool should throw
            Assert.Throws<NotSupportedException>(() => df.Add(true));

            var dataFrameColumn1 = new PrimitiveColumn<double>("Double1", Enumerable.Range(0, 10).Select(x => (double)x));
            df.SetColumn(0, dataFrameColumn1);
            // Double + comparison ops should throw
            Assert.Throws<NotSupportedException>(() => df.And(true));
        }

        [Fact]
        public void TestBinaryOperationsOnBoolColumn()
        {
            var df = new DataFrame();
            var dataFrameColumn1 = new PrimitiveColumn<bool>("Bool1", Enumerable.Range(0, 10).Select(x => true));
            var dataFrameColumn2 = new PrimitiveColumn<bool>("Bool2", Enumerable.Range(0, 10).Select(x => true));
            df.InsertColumn(0, dataFrameColumn1);
            df.InsertColumn(1, dataFrameColumn2);

            // bool + int should throw
            Assert.Throws<NotSupportedException>(() => df.Add(5));
            // Left shift should throw
            Assert.Throws<NotSupportedException>(() => df.LeftShift(5));

            IReadOnlyList<bool> listOfBools = new List<bool>() { true, false };
            // bool equals and And should work
            var newdf = df.Equals(true);
            Assert.Equal(true, newdf[4, 0]);
            var newdf1 = df.Equals(listOfBools);
            Assert.Equal(false, newdf1[4, 1]);

            newdf = df.And(true);
            Assert.Equal(true, newdf[4, 0]);
            newdf1 = df.And(listOfBools);
            Assert.Equal(false, newdf1[4, 1]);

            newdf = df.Or(true);
            Assert.Equal(true, newdf[4, 0]);
            newdf1 = df.Or(listOfBools);
            Assert.Equal(true, newdf1[4, 1]);

            newdf = df.Xor(true);
            Assert.Equal(false, newdf[4, 0]);
            newdf1 = df.Xor(listOfBools);
            Assert.Equal(true, newdf1[4, 1]);
        }

        [Fact]
        public void TestBinaryOperationsOnStringColumn()
        {
            var df = new DataFrame();
            BaseColumn stringColumn = new StringColumn("String", Enumerable.Range(0, 10).Select(x => x.ToString()));
            df.InsertColumn(0, stringColumn);

            BaseColumn newCol = stringColumn.Equals(5);
            Assert.Equal(true, newCol[5]);
            Assert.Equal(false, newCol[0]);

            BaseColumn stringColumnCopy = new StringColumn("String", Enumerable.Range(0, 10).Select(x => x.ToString()));
            newCol = stringColumn.Equals(stringColumnCopy);
            Assert.Equal(true, newCol[5]);
            Assert.Equal(true, newCol[0]);

            newCol = stringColumn.NotEquals(5);
            Assert.Equal(false, newCol[5]);
            Assert.Equal(true, newCol[0]);

            newCol = stringColumn.NotEquals(stringColumnCopy);
            Assert.Equal(false, newCol[5]);
            Assert.Equal(false, newCol[0]);
        }

        [Fact]
        public void TestBinaryOperatorsWithConversions()
        {
            var df = MakeDataFrameWithNumericColumns(10);

            DataFrame tempDf = df + 1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] + (double)1);
            tempDf = df + 1.1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] + 1.1);
            tempDf = df + 1.1m;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] + 1.1m);
            Assert.True(typeof(decimal) == tempDf["Int"].DataType);

            Assert.Throws<NotSupportedException>(() => df + true);
            Assert.Throws<NotSupportedException>(() => df & 5);

            tempDf = df - 1.1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] - 1.1);
            tempDf = df - 1.1m;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] - 1.1m);
            Assert.True(typeof(decimal) == tempDf["Int"].DataType);

            tempDf = df * 1.1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] * 1.1);
            tempDf = df * 1.1m;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] * 1.1m);
            Assert.True(typeof(decimal) == tempDf["Int"].DataType);

            tempDf = df / 1.1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] / 1.1);
            tempDf = df / 1.1m;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] / 1.1m);
            Assert.True(typeof(decimal) == tempDf["Int"].DataType);

            tempDf = df % 1.1;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] % 1.1);
            tempDf = df % 1.1m;
            Assert.Equal(tempDf[0, 0], (byte)df[0, 0] % 1.1m);
            Assert.True(typeof(decimal) == tempDf["Int"].DataType);

            tempDf = df == 1.1;
            Assert.Equal(false, tempDf[0, 0]);
            tempDf = df == 1.1m;
            Assert.Equal(false, tempDf[0, 0]);
            Assert.True(typeof(bool) == tempDf["Int"].DataType);

            tempDf = df != 1.1;
            Assert.Equal(true, tempDf[0, 0]);
            tempDf = df != 1.1m;
            Assert.Equal(true, tempDf[0, 0]);

            tempDf = df >= 1.1;
            Assert.Equal(false, tempDf[0, 0]);
            tempDf = df >= 1.1m;
            Assert.Equal(false, tempDf[0, 0]);

            tempDf = df <= 1.1;
            Assert.Equal(true, tempDf[0, 0]);
            tempDf = df <= 1.1m;
            Assert.Equal(true, tempDf[0, 0]);

            tempDf = df > 1.1;
            Assert.Equal(false, tempDf[0, 0]);
            tempDf = df > 1.1m;
            Assert.Equal(false, tempDf[0, 0]);

            tempDf = df < 1.1;
            Assert.Equal(true, tempDf[0, 0]);
            tempDf = df < 1.1m;
            Assert.Equal(true, tempDf[0, 0]);

            Assert.Equal((byte)0, df[0, 0]);
        }

        [Fact]
        public void TestProjectionAndAppend()
        {
            DataFrame df = MakeDataFrameWithTwoColumns(10);

            df["Int3"] = df["Int1"] * 2 + df["Int2"];
            Assert.Equal(16, df["Int3"][2]);
        }

        [Fact]
        public void TestComputations()
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(10);
            df["Int"][0] = -10;
            Assert.Equal(-10, df["Int"][0]);

            BaseColumn absColumn = df["Int"].Abs();
            Assert.Equal(10, absColumn[0]);
            Assert.Equal(-10, df["Int"][0]);
            df["Int"].Abs(true);
            Assert.Equal(10, df["Int"][0]);

            Assert.Throws<NotSupportedException>(() => df["Byte"].All());
            Assert.Throws<NotSupportedException>(() => df["Byte"].Any());
            Assert.Throws<NotSupportedException>(() => df["Char"].All());
            Assert.Throws<NotSupportedException>(() => df["Char"].Any());
            Assert.Throws<NotSupportedException>(() => df["Decimal"].All());
            Assert.Throws<NotSupportedException>(() => df["Decimal"].Any());
            Assert.Throws<NotSupportedException>(() => df["Double"].All());
            Assert.Throws<NotSupportedException>(() => df["Double"].Any());
            Assert.Throws<NotSupportedException>(() => df["Float"].All());
            Assert.Throws<NotSupportedException>(() => df["Float"].Any());
            Assert.Throws<NotSupportedException>(() => df["Int"].All());
            Assert.Throws<NotSupportedException>(() => df["Int"].Any());
            Assert.Throws<NotSupportedException>(() => df["Long"].All());
            Assert.Throws<NotSupportedException>(() => df["Long"].Any());
            Assert.Throws<NotSupportedException>(() => df["Sbyte"].All());
            Assert.Throws<NotSupportedException>(() => df["Sbyte"].Any());
            Assert.Throws<NotSupportedException>(() => df["Short"].All());
            Assert.Throws<NotSupportedException>(() => df["Short"].Any());
            Assert.Throws<NotSupportedException>(() => df["Uint"].All());
            Assert.Throws<NotSupportedException>(() => df["Uint"].Any());
            Assert.Throws<NotSupportedException>(() => df["Ulong"].All());
            Assert.Throws<NotSupportedException>(() => df["Ulong"].Any());
            Assert.Throws<NotSupportedException>(() => df["Ushort"].All());
            Assert.Throws<NotSupportedException>(() => df["Ushort"].Any());

            bool any = df["Bool"].Any();
            bool all = df["Bool"].All();
            Assert.True(any);
            Assert.False(all);

            // Test the computation results
            df["Double"][0] = 100.0;
            BaseColumn doubleColumn = df["Double"].CumulativeMax();
            Assert.Equal(100.0, doubleColumn[9]);
            Assert.Equal(1.0, df["Double"][1]);
            df["Double"].CumulativeMax(true);
            Assert.Equal(100.0, df["Double"][9]);

            df["Float"][0] = -10.0f;
            BaseColumn floatColumn = df["Float"].CumulativeMin();
            Assert.Equal(-10.0f, floatColumn[9]);
            Assert.Equal(9.0f, df["Float"][9]);
            df["Float"].CumulativeMin(true);
            Assert.Equal(-10.0f, df["Float"][9]);

            BaseColumn uintColumn = df["Uint"].CumulativeProduct();
            Assert.Equal((uint)0, uintColumn[8]);
            Assert.Equal((uint)8, df["Uint"][8]);
            df["Uint"].CumulativeProduct(true);
            Assert.Equal((uint)0, df["Uint"][9]);

            BaseColumn ushortColumn = df["Ushort"].CumulativeSum();
            Assert.Equal((ushort)40, ushortColumn[9]);
            Assert.Equal((ushort)9, df["Ushort"][9]);
            df["Ushort"].CumulativeSum(true);
            Assert.Equal((ushort)40, df["Ushort"][9]);

            Assert.Equal(100.0, df["Double"].Max());
            Assert.Equal(-10.0f, df["Float"].Min());
            Assert.Equal((uint)0, df["Uint"].Product());
            Assert.Equal((ushort)140, df["Ushort"].Sum());

            df["Double"][0] = 100.1;
            Assert.Equal(100.1, df["Double"][0]);
            BaseColumn roundColumn = df["Double"].Round();
            Assert.Equal(100.0, roundColumn[0]);
            Assert.Equal(100.1, df["Double"][0]);
            df["Double"].Round(true);
            Assert.Equal(100.0, df["Double"][0]);

            // Test that none of the numeric column types throw
            for (int i = 0; i < df.ColumnCount; i++)
            {
                BaseColumn column = df.Column(i);
                if (column.DataType == typeof(bool))
                {
                    Assert.Throws<NotSupportedException>(() => column.CumulativeMax());
                    Assert.Throws<NotSupportedException>(() => column.CumulativeMin());
                    Assert.Throws<NotSupportedException>(() => column.CumulativeProduct());
                    Assert.Throws<NotSupportedException>(() => column.CumulativeSum());
                    Assert.Throws<NotSupportedException>(() => column.Max());
                    Assert.Throws<NotSupportedException>(() => column.Min());
                    Assert.Throws<NotSupportedException>(() => column.Product());
                    Assert.Throws<NotSupportedException>(() => column.Sum());
                    continue;
                }
                else if (column.DataType == typeof(string))
                {
                    Assert.Throws<NotImplementedException>(() => column.CumulativeMax());
                    Assert.Throws<NotImplementedException>(() => column.CumulativeMin());
                    Assert.Throws<NotImplementedException>(() => column.CumulativeProduct());
                    Assert.Throws<NotImplementedException>(() => column.CumulativeSum());
                    Assert.Throws<NotImplementedException>(() => column.Max());
                    Assert.Throws<NotImplementedException>(() => column.Min());
                    Assert.Throws<NotImplementedException>(() => column.Product());
                    Assert.Throws<NotImplementedException>(() => column.Sum());
                    continue;
                }
                column.CumulativeMax();
                column.CumulativeMin();
                column.CumulativeProduct();
                column.CumulativeSum();
                column.Max();
                column.Min();
                column.Product();
                column.Sum();
            }
        }

        [Fact]
        public void TestSort()
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(20);
            df["Int"][0] = 100;
            df["Int"][19] = -1;
            df["Int"][5] = 2000;

            // Sort by "Int" in ascending order
            var sortedDf = df.Sort("Int");
            Assert.Null(sortedDf["Int"][19]);
            Assert.Equal(-1, sortedDf["Int"][0]);
            Assert.Equal(100, sortedDf["Int"][17]);
            Assert.Equal(2000, sortedDf["Int"][18]);

            // Sort by "Int" in descending order
            sortedDf = df.Sort("Int", false);
            Assert.Null(sortedDf["Int"][19]);
            Assert.Equal(-1, sortedDf["Int"][18]);
            Assert.Equal(100, sortedDf["Int"][1]);
            Assert.Equal(2000, sortedDf["Int"][0]);

            // Sort by "String" in ascending order
            sortedDf = df.Sort("String");
            Assert.Null(sortedDf["Int"][19]);
            Assert.Equal(1, sortedDf["Int"][1]);
            Assert.Equal(8, sortedDf["Int"][17]);
            Assert.Equal(9, sortedDf["Int"][18]);

            sortedDf = df.Sort("String", false);
            Assert.Null(sortedDf["Int"][19]);
            Assert.Equal(8, sortedDf["Int"][1]);
            Assert.Equal(9, sortedDf["Int"][0]);
        }

        [Fact]
        public void TestStringColumnSort()
        {
            // StringColumn specific sort tests
            StringColumn strColumn = new StringColumn("String", 0);
            Assert.Equal(0, strColumn.NullCount);
            for (int i = 0; i < 5; i++)
            {
                strColumn.Append(null);
            }
            Assert.Equal(5, strColumn.NullCount);
            // Should handle all nulls
            StringColumn sortedStrColumn = strColumn.Sort() as StringColumn;
            Assert.Equal(5, sortedStrColumn.NullCount);
            Assert.Null(sortedStrColumn[0]);

            for (int i = 0; i < 5; i++)
            {
                strColumn.Append(i.ToString());
            }
            Assert.Equal(5, strColumn.NullCount);

            // Ascending sort
            sortedStrColumn = strColumn.Sort() as StringColumn;
            Assert.Equal("0", sortedStrColumn[0]);
            Assert.Null(sortedStrColumn[9]);

            // Descending sort
            sortedStrColumn = strColumn.Sort(false) as StringColumn;
            Assert.Equal("4", sortedStrColumn[0]);
            Assert.Null(sortedStrColumn[9]);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(12)]
        [InlineData(100)]
        [InlineData(1000)]
        public void TestPrimitiveColumnSort(int numberOfNulls)
        {
            // Primitive Column Sort
            PrimitiveColumn<int> intColumn = new PrimitiveColumn<int>("Int", 0);
            Assert.Equal(0, intColumn.NullCount);
            intColumn.AppendMany(null, numberOfNulls);
            Assert.Equal(numberOfNulls, intColumn.NullCount);

            // Should handle all nulls
            PrimitiveColumn<int> sortedIntColumn = intColumn.Sort() as PrimitiveColumn<int>;
            Assert.Equal(numberOfNulls, sortedIntColumn.NullCount);
            Assert.Null(sortedIntColumn[0]);

            for (int i = 0; i < 5; i++)
            {
                intColumn.Append(i);
            }
            Assert.Equal(numberOfNulls, intColumn.NullCount);

            // Ascending sort
            sortedIntColumn = intColumn.Sort() as PrimitiveColumn<int>;
            Assert.Equal(0, sortedIntColumn[0]);
            Assert.Null(sortedIntColumn[9]);

            // Descending sort
            sortedIntColumn = intColumn.Sort(false) as PrimitiveColumn<int>;
            Assert.Equal(4, sortedIntColumn[0]);
            Assert.Null(sortedIntColumn[9]);
        }

        private void VerifyJoin(DataFrame join, DataFrame left, DataFrame right, JoinAlgorithm joinAlgorithm)
        {
            PrimitiveColumn<long> mapIndices = new PrimitiveColumn<long>("map", join.RowCount);
            for (long i = 0; i < join.RowCount; i++)
            {
                mapIndices[i] = i;
            }
            for (int i = 0; i < join.ColumnCount; i++)
            {
                BaseColumn joinColumn = join.Column(i);
                BaseColumn isEqual;

                if (joinAlgorithm == JoinAlgorithm.Left)
                {
                    if (i < left.ColumnCount)
                    {
                        BaseColumn leftColumn = left.Column(i);
                        isEqual = joinColumn == leftColumn;
                    }
                    else
                    {
                        int columnIndex = i - left.ColumnCount;
                        BaseColumn rightColumn = right.Column(columnIndex);
                        BaseColumn compareColumn = rightColumn.Length <= join.RowCount ? rightColumn.Clone(numberOfNullsToAppend: join.RowCount - rightColumn.Length) : rightColumn.Clone(mapIndices);
                        isEqual = joinColumn == compareColumn;
                    }
                }
                else if (joinAlgorithm == JoinAlgorithm.Right)
                {
                    if (i < left.ColumnCount)
                    {
                        BaseColumn leftColumn = left.Column(i);
                        BaseColumn compareColumn = leftColumn.Length <= join.RowCount ? leftColumn.Clone(numberOfNullsToAppend: join.RowCount - leftColumn.Length) : leftColumn.Clone(mapIndices);
                        isEqual = joinColumn == compareColumn;
                    }
                    else
                    {
                        int columnIndex = i - left.ColumnCount;
                        BaseColumn rightColumn = right.Column(columnIndex);
                        isEqual = joinColumn == rightColumn;
                    }
                }
                else if (joinAlgorithm == JoinAlgorithm.Inner)
                {
                    if (i < left.ColumnCount)
                    {
                        BaseColumn leftColumn = left.Column(i);
                        isEqual = joinColumn == leftColumn.Clone(mapIndices);
                    }
                    else
                    {
                        int columnIndex = i - left.ColumnCount;
                        BaseColumn rightColumn = right.Column(columnIndex);
                        isEqual = joinColumn == rightColumn.Clone(mapIndices);
                    }
                }
                else
                {
                    if (i < left.ColumnCount)
                    {
                        BaseColumn leftColumn = left.Column(i);
                        isEqual = joinColumn == leftColumn.Clone(numberOfNullsToAppend: join.RowCount - leftColumn.Length);
                    }
                    else
                    {
                        int columnIndex = i - left.ColumnCount;
                        BaseColumn rightColumn = right.Column(columnIndex);
                        isEqual = joinColumn == rightColumn.Clone(numberOfNullsToAppend: join.RowCount - rightColumn.Length);
                    }
                }
                for (int j = 0; j < join.RowCount; j++)
                {
                    Assert.Equal(true, isEqual[j]);
                }
            }
        }

        private void VerifyMerge(DataFrame merge, DataFrame left, DataFrame right, JoinAlgorithm joinAlgorithm)
        {
            if (joinAlgorithm == JoinAlgorithm.Left || joinAlgorithm == JoinAlgorithm.Inner)
            {
                HashSet<int> intersection = new HashSet<int>();
                for (int i = 0; i < merge["Int_left"].Length; i++)
                {
                    if (merge["Int_left"][i] == null)
                        continue;
                    intersection.Add((int)merge["Int_left"][i]);
                }
                for (int i = 0; i < left["Int"].Length; i++)
                {
                    if (left["Int"][i] != null && intersection.Contains((int)left["Int"][i]))
                        intersection.Remove((int)left["Int"][i]);
                }
                Assert.Empty(intersection);
            }
            else if (joinAlgorithm == JoinAlgorithm.Right)
            {
                HashSet<int> intersection = new HashSet<int>();
                for (int i = 0; i < merge["Int_right"].Length; i++)
                {
                    if (merge["Int_right"][i] == null)
                        continue;
                    intersection.Add((int)merge["Int_right"][i]);
                }
                for (int i = 0; i < right["Int"].Length; i++)
                {
                    if (right["Int"][i] != null && intersection.Contains((int)right["Int"][i]))
                        intersection.Remove((int)right["Int"][i]);
                }
                Assert.Empty(intersection);
            }
            else if (joinAlgorithm == JoinAlgorithm.FullOuter)
            {
                VerifyMerge(merge, left, right, JoinAlgorithm.Left);
                VerifyMerge(merge, left, right, JoinAlgorithm.Right);
            }
        }

        [Fact]
        public void TestJoin()
        {
            DataFrame left = MakeDataFrameWithAllMutableColumnTypes(10);
            DataFrame right = MakeDataFrameWithAllMutableColumnTypes(5);

            // Tests with right.RowCount < left.RowCount
            // Left join
            DataFrame join = left.Join(right);
            Assert.Equal(join.RowCount, left.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(join["Int_right"][6]);
            VerifyJoin(join, left, right, JoinAlgorithm.Left);

            // Right join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.Right);
            Assert.Equal(join.RowCount, right.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(join["Int_right"][3], right["Int"][3]);
            Assert.Null(join["Int_right"][2]);
            VerifyJoin(join, left, right, JoinAlgorithm.Right);

            // Outer join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.FullOuter);
            Assert.Equal(join.RowCount, left.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(join["Int_right"][6]);
            VerifyJoin(join, left, right, JoinAlgorithm.FullOuter);

            // Inner join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.Inner);
            Assert.Equal(join.RowCount, right.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(join["Int_right"][3], right["Int"][3]);
            Assert.Null(join["Int_right"][2]);
            VerifyJoin(join, left, right, JoinAlgorithm.Inner);

            // Tests with right.RowCount > left.RowCount
            // Left join
            right = MakeDataFrameWithAllMutableColumnTypes(15);
            join = left.Join(right);
            Assert.Equal(join.RowCount, left.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(join["Int_right"][6], right["Int"][6]);
            VerifyJoin(join, left, right, JoinAlgorithm.Left);

            // Right join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.Right);
            Assert.Equal(join.RowCount, right.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(join["Int_right"][2], right["Int"][2]);
            Assert.Null(join["Int_left"][12]);
            VerifyJoin(join, left, right, JoinAlgorithm.Right);

            // Outer join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.FullOuter);
            Assert.Equal(join.RowCount, right.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(join["Int_left"][12]);
            VerifyJoin(join, left, right, JoinAlgorithm.FullOuter);

            // Inner join
            join = left.Join(right, joinAlgorithm: JoinAlgorithm.Inner);
            Assert.Equal(join.RowCount, left.RowCount);
            Assert.Equal(join.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(join["Int_right"][2], right["Int"][2]);
            VerifyJoin(join, left, right, JoinAlgorithm.Inner);
        }

        [Fact]
        public void TestGroupBy()
        {
            DataFrame df = MakeDataFrameWithNumericAndBoolColumns(10);
            DataFrame count = df.GroupBy("Bool").Count();
            Assert.Equal(2, count.RowCount);
            Assert.Equal((long)5, count["Int"][0]);
            Assert.Equal((long)4, count["Decimal"][1]);
            for (int r = 0; r < count.RowCount; r++)
            {
                for (int c = 1; c < count.ColumnCount; c++)
                {
                    Assert.Equal((long)(r == 0 ? 5 : 4), count.Column(c)[r]);
                }
            }

            DataFrame first = df.GroupBy("Bool").First();
            Assert.Equal(2, first.RowCount);
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < count.ColumnCount; c++)
                {
                    BaseColumn originalColumn = df.Column(c);
                    BaseColumn firstColumn = first[originalColumn.Name];
                    Assert.Equal(originalColumn[r], firstColumn[r]);
                }
            }

            DataFrame head = df.GroupBy("Bool").Head(3);
            List<int> verify = new List<int>() { 0, 3, 1, 4, 2, 5 };
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < count.ColumnCount; c++)
                {
                    BaseColumn originalColumn = df.Column(c);
                    BaseColumn headColumn = head[originalColumn.Name];
                    Assert.Equal(originalColumn[r].ToString(), headColumn[verify[r]].ToString());
                }
            }
            for (int c = 0; c < count.ColumnCount; c++)
            {
                BaseColumn originalColumn = df.Column(c);
                if (originalColumn.Name == "Bool")
                    continue;
                BaseColumn headColumn = head[originalColumn.Name];
                Assert.Equal(originalColumn[5], headColumn[verify[5]]);
            }
            Assert.Equal(6, head.RowCount);

            DataFrame tail = df.GroupBy("Bool").Tail(3);
            Assert.Equal(6, tail.RowCount);
            List<int> originalColumnVerify = new List<int>() { 6, 8, 7, 9 };
            List<int> tailColumnVerity = new List<int>() { 1, 2, 4, 5 };
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < count.ColumnCount; c++)
                {
                    BaseColumn originalColumn = df.Column(c);
                    BaseColumn tailColumn = tail[originalColumn.Name];
                    Assert.Equal(originalColumn[originalColumnVerify[r]].ToString(), tailColumn[tailColumnVerity[r]].ToString());
                }
            }

            DataFrame max = df.GroupBy("Bool").Max();
            Assert.Equal(2, max.RowCount);
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < count.ColumnCount; c++)
                {
                    BaseColumn originalColumn = df.Column(c);
                    if (originalColumn.Name == "Bool" || originalColumn.Name == "Char")
                        continue;
                    BaseColumn maxColumn = max[originalColumn.Name];
                    Assert.Equal(((long)(r == 0 ? 8 : 9)).ToString(), maxColumn[r].ToString());
                }
            }

            DataFrame min = df.GroupBy("Bool").Min();
            Assert.Equal(2, min.RowCount);

            DataFrame product = df.GroupBy("Bool").Product();
            Assert.Equal(2, product.RowCount);

            DataFrame sum = df.GroupBy("Bool").Sum();
            Assert.Equal(2, sum.RowCount);

            DataFrame mean = df.GroupBy("Bool").Mean();
            Assert.Equal(2, mean.RowCount);
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < count.ColumnCount; c++)
                {
                    BaseColumn originalColumn = df.Column(c);
                    if (originalColumn.Name == "Bool" || originalColumn.Name == "Char")
                        continue;
                    BaseColumn minColumn = min[originalColumn.Name];
                    Assert.Equal("0", minColumn[r].ToString());

                    BaseColumn productColumn = product[originalColumn.Name];
                    Assert.Equal("0", productColumn[r].ToString());

                    BaseColumn sumColumn = sum[originalColumn.Name];
                    Assert.Equal("20", sumColumn[r].ToString());
                }
            }

            DataFrame columnSum = df.GroupBy("Bool").Sum("Int");
            Assert.Equal(2, columnSum.ColumnCount);
            Assert.Equal(20, columnSum["Int"][0]);
            Assert.Equal(20, columnSum["Int"][1]);
            DataFrame columnMax = df.GroupBy("Bool").Max("Int");
            Assert.Equal(2, columnMax.ColumnCount);
            Assert.Equal(8, columnMax["Int"][0]);
            Assert.Equal(9, columnMax["Int"][1]);
            DataFrame columnProduct = df.GroupBy("Bool").Product("Int");
            Assert.Equal(2, columnProduct.ColumnCount);
            Assert.Equal(0, columnProduct["Int"][0]);
            Assert.Equal(0, columnProduct["Int"][1]);
            DataFrame columnMin = df.GroupBy("Bool").Min("Int");
            Assert.Equal(2, columnMin.ColumnCount);
            Assert.Equal(0, columnMin["Int"][0]);
            Assert.Equal(0, columnMin["Int"][1]);

            DataFrame countIntColumn = df.GroupBy("Bool").Count("Int");
            Assert.Equal(2, countIntColumn.ColumnCount);
            Assert.Equal(2, countIntColumn.RowCount);
            Assert.Equal((long)5, countIntColumn["Int"][0]);
            Assert.Equal((long)4, countIntColumn["Int"][1]);

            DataFrame firstDecimalColumn = df.GroupBy("Bool").First("Decimal");
            Assert.Equal(2, firstDecimalColumn.ColumnCount);
            Assert.Equal(2, firstDecimalColumn.RowCount);
            Assert.Equal((decimal)0, firstDecimalColumn["Decimal"][0]);
            Assert.Equal((decimal)1, firstDecimalColumn["Decimal"][1]);
        }

        [Fact]
        public void TestGoupByDifferentColumnTypes()
        {
            void GroupCountAndAssert(DataFrame frame)
            {
                DataFrame grouped = frame.GroupBy("Column1").Count();
                Assert.Equal(2, grouped.RowCount);
            }

            DataFrame df = MakeDataFrame<byte, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<char, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<decimal, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<double, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<float, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<int, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<long, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<sbyte, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<short, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<uint, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<ulong, bool>(10, false);
            GroupCountAndAssert(df);

            df = MakeDataFrame<ushort, bool>(10, false);
            GroupCountAndAssert(df);
        }

        [Fact]
        public void TestIEnumerable()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10);

            int totalValueCount = 0;
            for (int i = 0; i < df.ColumnCount; i++)
            {
                BaseColumn baseColumn = df.Column(i);
                foreach (object value in baseColumn)
                {
                    totalValueCount++;
                }
            }
            Assert.Equal(10 * df.ColumnCount, totalValueCount);

            // spot check a few column types:

            StringColumn stringColumn = (StringColumn)df["String"];
            StringBuilder actualStrings = new StringBuilder();
            foreach (string value in stringColumn)
            {
                if (value == null)
                {
                    actualStrings.Append("<null>");
                }
                else
                {
                    actualStrings.Append(value);
                }
            }
            Assert.Equal("01234<null>6789", actualStrings.ToString());

            ArrowStringColumn arrowStringColumn = (ArrowStringColumn)df["ArrowString"];
            actualStrings.Clear();
            foreach (string value in arrowStringColumn)
            {
                if (value == null)
                {
                    actualStrings.Append("<null>");
                }
                else
                {
                    actualStrings.Append(value);
                }
            }
            Assert.Equal("foofoofoofoofoofoofoofoofoofoo", actualStrings.ToString());

            PrimitiveColumn<float> floatColumn = (PrimitiveColumn<float>)df["Float"];
            actualStrings.Clear();
            foreach (float? value in floatColumn)
            {
                if (value == null)
                {
                    actualStrings.Append("<null>");
                }
                else
                {
                    actualStrings.Append(value);
                }
            }
            Assert.Equal("01234<null>6789", actualStrings.ToString());

            PrimitiveColumn<int> intColumn = (PrimitiveColumn<int>)df["Int"];
            actualStrings.Clear();
            foreach (int? value in intColumn)
            {
                if (value == null)
                {
                    actualStrings.Append("<null>");
                }
                else
                {
                    actualStrings.Append(value);
                }
            }
            Assert.Equal("01234<null>6789", actualStrings.ToString());
        }

        [Fact]
        public void TestColumnClip()
        {
            DataFrame df = MakeDataFrameWithNumericColumns(10);
            BaseColumn clipped = df["Int"].Clip(3, 7);
            Assert.Equal(3, clipped[0]);
            Assert.Equal(3, clipped[1]);
            Assert.Equal(3, clipped[2]);
            Assert.Equal(3, clipped[3]);
            Assert.Equal(4, clipped[4]);
            Assert.Null(clipped[5]);
            Assert.Equal(6, clipped[6]);
            Assert.Equal(7, clipped[7]);
            Assert.Equal(7, clipped[8]);
            Assert.Equal(7, clipped[9]);
        }

        [Fact]
        public void TestColumnFilter()
        {
            DataFrame df = MakeDataFrameWithNumericColumns(10);
            BaseColumn filtered = df["Int"].Filter(3, 7);
            Assert.Equal(4, filtered.Length);
            Assert.Equal(3, filtered[0]);
            Assert.Equal(4, filtered[1]);
            Assert.Equal(6, filtered[2]);
            Assert.Equal(7, filtered[3]);
        }

        [Fact]
        public void TestDataFrameClip()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10);
            IList<string> dfColumns = df.Columns;
            DataFrame clipped = df.Clip(3, 7);
            IList<string> clippedColumns = clipped.Columns;
            Assert.Equal(df.ColumnCount, clipped.ColumnCount);
            Assert.Equal(dfColumns, clippedColumns);
            for (int c = 0; c < df.ColumnCount; c++)
            {
                BaseColumn column = clipped.Column(c);
                if (column.IsNumericColumn())
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Assert.Equal("3", column[i].ToString());
                    }
                    Assert.Equal(4.ToString(), column[4].ToString());
                    Assert.Null(column[5]);
                    Assert.Equal(6.ToString(), column[6].ToString());
                    for (int i = 7; i < 10; i++)
                    {
                        Assert.Equal("7", column[i].ToString());
                    }
                }
                else
                {
                    for (int i = 0; i < column.Length; i++)
                    {
                        Assert.Equal(df.Column(c)[i], column[i]);
                    }
                }
            }
        }

        [Fact]
        public void TestDataFrameFilter()
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(10);
            DataFrame filtered = df[df["Bool"] == true];
            List<int> verify = new List<int> { 0, 2, 4, 6, 8 };
            Assert.Equal(5, filtered.RowCount);
            for (int i = 0; i < filtered.ColumnCount; i++)
            {
                BaseColumn column = filtered.Column(i);
                if (column.Name == "Char" || column.Name == "Bool" || column.Name == "String")
                    continue;
                for (int j = 0; j < column.Length; j++)
                {
                    Assert.Equal(verify[j].ToString(), column[j].ToString());
                }
            }
        }

        [Fact]
        public void TestPrefixAndSuffix()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10);
            IList<string> columnNames = df.Columns;
            df.AddPrefix("Prefix_");
            IList<string> prefixNames = df.Columns;
            for (int i = 0; i < prefixNames.Count; i++)
            {
                Assert.Equal("Prefix_" + columnNames[i], prefixNames[i]);
            }
            df.AddSuffix("_Suffix");
            IList<string> suffixNames = df.Columns;
            for (int i = 0; i < suffixNames.Count; i++)
            {
                Assert.Equal("Prefix_" + columnNames[i] + "_Suffix", suffixNames[i]);
            }
        }

        [Fact]
        public void TestSample()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10);
            DataFrame sampled = df.Sample(3);
            Assert.Equal(3, sampled.RowCount);
            Assert.Equal(df.ColumnCount, sampled.ColumnCount);
        }

        [Fact]
        public void TestMerge()
        {
            DataFrame left = MakeDataFrameWithAllMutableColumnTypes(10);
            DataFrame right = MakeDataFrameWithAllMutableColumnTypes(5);

            // Tests with right.RowCount < left.RowCount 
            // Left merge 
            DataFrame merge = left.Merge<int>(right, "Int", "Int");
            Assert.Equal(10, merge.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(merge["Int_right"][6]);
            Assert.Null(merge["Int_left"][5]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Left);

            // Right merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.Right);
            Assert.Equal(5, merge.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(merge["Int_right"][3], right["Int"][3]);
            Assert.Null(merge["Int_right"][2]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Right);

            // Outer merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.FullOuter);
            Assert.Equal(merge.RowCount, left.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(merge["Int_right"][6]);
            VerifyMerge(merge, left, right, JoinAlgorithm.FullOuter);

            // Inner merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.Inner);
            Assert.Equal(merge.RowCount, right.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(merge["Int_right"][2], right["Int"][3]);
            Assert.Null(merge["Int_right"][4]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Inner);

            // Tests with right.RowCount > left.RowCount 
            // Left merge 
            right = MakeDataFrameWithAllMutableColumnTypes(15);
            merge = left.Merge<int>(right, "Int", "Int");
            Assert.Equal(merge.RowCount, left.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(merge["Int_right"][6], right["Int"][6]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Left);

            // Right merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.Right);
            Assert.Equal(merge.RowCount, right.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(merge["Int_right"][2], right["Int"][2]);
            Assert.Null(merge["Int_left"][12]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Right);

            // Outer merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.FullOuter);
            Assert.Equal(16, merge.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Null(merge["Int_left"][12]);
            Assert.Null(merge["Int_left"][5]);
            VerifyMerge(merge, left, right, JoinAlgorithm.FullOuter);

            // Inner merge 
            merge = left.Merge<int>(right, "Int", "Int", joinAlgorithm: JoinAlgorithm.Inner);
            Assert.Equal(9, merge.RowCount);
            Assert.Equal(merge.ColumnCount, left.ColumnCount + right.ColumnCount);
            Assert.Equal(merge["Int_right"][2], right["Int"][2]);
            VerifyMerge(merge, left, right, JoinAlgorithm.Inner);
        }

        [Fact]
        public void TestDescription()
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(10);
            DataFrame description = df.Description();
            for (int i = 1; i < description.ColumnCount; i++)
            {
                BaseColumn column = description.Column(i);
                Assert.Equal(4, column.Length);
                Assert.Equal((float)9, column[0]);
                Assert.Equal((float)9, column[1]);
                Assert.Equal((float)0, column[2]);
                Assert.Equal((float)4, column[3]);
            }
        }

        [Fact]
        public void TestDropNulls()
        {
            DataFrame df = MakeDataFrameWithAllMutableColumnTypes(20);
            DataFrame anyNulls = df.DropNulls();
            Assert.Equal(19, anyNulls.RowCount);

            DataFrame allNulls = df.DropNulls(DropNullOptions.All);
            Assert.Equal(19, allNulls.RowCount);
        }

        [Fact]
        public void TestFillNulls()
        {
            DataFrame df = MakeDataFrameWithTwoColumns(20);
            Assert.Null(df[10, 0]);
            DataFrame fillNulls = df.FillNulls(1000);
            Assert.Equal(1000, (int)fillNulls[10, 1]);

            StringColumn strColumn = new StringColumn("String", 0);
            strColumn.Append(null);
            strColumn.Append(null);
            Assert.Equal(2, strColumn.Length);
            Assert.Equal(2, strColumn.NullCount);
            strColumn.FillNulls("foo", true);
            Assert.Equal(2, strColumn.Length);
            Assert.Equal(0, strColumn.NullCount);
            Assert.Equal("foo", strColumn[0]);
            Assert.Equal("foo", strColumn[1]);
        }

        [Fact]
        public void TestValueCounts()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10, withNulls: false);
            DataFrame valueCounts = df["Bool"].ValueCounts();
            Assert.Equal(2, valueCounts.RowCount);
            Assert.Equal((long)5, valueCounts["Counts"][0]);
            Assert.Equal((long)5, valueCounts["Counts"][1]);
        }

        [Fact]
        public void TestApplyElementwiseNullCount()
        {
            DataFrame df = MakeDataFrameWithTwoColumns(10);
            PrimitiveColumn<int> column = df["Int1"] as PrimitiveColumn<int>;
            Assert.Equal(1, column.NullCount);

            // Change all existing values to null
            column.ApplyElementwise((int? value, long rowIndex) =>
            {
                if (!(value is null))
                    return null;
                return value;
            });
            Assert.Equal(column.Length, column.NullCount);

            // Don't change null values
            column.ApplyElementwise((int? value, long rowIndex) =>
            {
                return value;
            });
            Assert.Equal(column.Length, column.NullCount);

            // Change all null values to real values
            column.ApplyElementwise((int? value, long rowIndex) =>
            {
                return 5;
            });
            Assert.Equal(0, column.NullCount);

            // Don't change real values
            column.ApplyElementwise((int? value, long rowIndex) =>
            {
                return value;
            });
            Assert.Equal(0, column.NullCount);

        }

        [Fact]
        public void TestClone()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10, withNulls: false);
            DataFrame intDf = MakeDataFrameWithTwoColumns(5, false);
            BaseColumn intColumn = intDf["Int1"];
            DataFrame clone = df[intColumn];
            Assert.Equal(5, clone.RowCount);
            Assert.Equal(df.ColumnCount, clone.ColumnCount);
            for (int i = 0; i < df.ColumnCount; i++)
            {
                BaseColumn dfColumn = df.Column(i);
                BaseColumn cloneColumn = clone.Column(i);
                for (long r = 0; r < clone.RowCount; r++)
                {
                    Assert.Equal(dfColumn[r], cloneColumn[r]);
                }
            }
        }

    }
}
