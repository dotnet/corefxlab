// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data;
using Xunit;

namespace Microsoft.Data.Tests
{
    public class DataFrameTests
    {
        static public DataFrame MakeDataFrameWithTwoColumns(int length)
        {
            BaseColumn dataFrameColumn1 = new PrimitiveColumn<int>("Int1", Enumerable.Range(0, length).Select(x => x));
            BaseColumn dataFrameColumn2 = new PrimitiveColumn<int>("Int2", Enumerable.Range(10, length).Select(x => x));
            Data.DataFrame dataFrame = new Data.DataFrame();
            dataFrame.InsertColumn(0, dataFrameColumn1);
            dataFrame.InsertColumn(1, dataFrameColumn2);
            return dataFrame;
        }

        static public DataFrame MakeDataFrameWithAllColumnTypes(int length)
        {
            DataFrame df = MakeDataFrameWithNumericAndStringColumns(length);
            BaseColumn boolColumn = new PrimitiveColumn<bool>("Bool", Enumerable.Range(0, length).Select(x => x % 2 == 0));
            df.InsertColumn(df.ColumnCount, boolColumn);
            return df;
        }

        static public DataFrame MakeDataFrameWithNumericAndStringColumns(int length)
        {
            DataFrame df = MakeDataFrameWithNumericColumns(length);
            BaseColumn stringColumn = new StringColumn("String", Enumerable.Range(0, length).Select(x => x.ToString()));
            df.InsertColumn(df.ColumnCount, stringColumn);
            return df;
        }

        static public DataFrame MakeDataFrameWithNumericColumns(int length)
        {
            BaseColumn byteColumn = new PrimitiveColumn<byte>("Byte", Enumerable.Range(0, length).Select(x => (byte)x));
            BaseColumn charColumn = new PrimitiveColumn<char>("Char", Enumerable.Range(0, length).Select(x => (char)(x + 65)));
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

            DataFrame dataFrame = new DataFrame(new List<BaseColumn> { byteColumn, charColumn, decimalColumn, doubleColumn, floatColumn, intColumn, longColumn, sbyteColumn, shortColumn, uintColumn, ulongColumn, ushortColumn});

            return dataFrame;
        }

        [Fact]
        public void TestIndexer()
        {
            Data.DataFrame dataFrame = MakeDataFrameWithTwoColumns(length: 10);
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
            Assert.Throws<ArgumentException>( () => dataFrame.InsertColumn(2, bigColumn));
            Assert.Throws<ArgumentException>( () => dataFrame.InsertColumn(2, repeatedName));
            Assert.Throws<ArgumentOutOfRangeException>( () => dataFrame.InsertColumn(10, repeatedName));

            Assert.Equal(2, dataFrame.ColumnCount);
            BaseColumn intColumnCopy = new PrimitiveColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            Assert.Throws<System.ArgumentException>(() => dataFrame.SetColumn(1, intColumnCopy));
            
            BaseColumn differentIntColumn = new PrimitiveColumn<int>("IntColumn1", Enumerable.Range(0, 10).Select(x => x));
            dataFrame.SetColumn(1, differentIntColumn);
            Assert.True(object.ReferenceEquals(differentIntColumn, dataFrame.Column(1)));

            dataFrame.RemoveColumn(1);
            Assert.Equal(1, dataFrame.ColumnCount);
            Assert.True(ReferenceEquals(intColumn, dataFrame.Column(0)));
        }

        [Fact]
        public void TestBinaryOperations()
        {
            DataFrame df = DataFrameTests.MakeDataFrameWithTwoColumns(10);
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
            Assert.Equal(true, df.GreaterThanOrEqual(5)[6, 0]);
            Assert.Equal(true, df.GreaterThanOrEqual(listOfInts)[6, 0]);
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
            Assert.Throws<System.NotSupportedException>(() => df.Add(true));

            var dataFrameColumn1 = new PrimitiveColumn<double>("Double1", Enumerable.Range(0, 10).Select(x => (double)x));
            df.SetColumn(0, dataFrameColumn1);
            // Double + comparison ops should throw
            Assert.Throws<System.NotSupportedException>(() => df.And(true));
        }

        [Fact]
        public void TestBinaryOperationsOnBoolColumn()
        {
            var df = new DataFrame();
            var dataFrameColumn1 = new PrimitiveColumn<bool>("Bool1", Enumerable.Range(0, 10).Select(x => true));
            df.InsertColumn(0, dataFrameColumn1);

            // bool + int should throw
            Assert.Throws<System.NotSupportedException>(() => df.Add(5));
            // Left shift should throw
            Assert.Throws<System.NotSupportedException>(() => df.LeftShift(5));

            // bool equals and And should work
            var newdf = df.Equals(true);
            Assert.Equal(true, newdf[4, 0]);

            newdf = df.And(true);
            Assert.Equal(true, newdf[4, 0]);

            newdf = df.Or(true);
            Assert.Equal(true, newdf[4, 0]);

            newdf = df.Xor(true);
            Assert.Equal(false, newdf[4, 0]);
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

            Assert.Throws<System.NotSupportedException>(() => df + true);
            Assert.Throws<System.NotSupportedException>(() => df & 5);

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
            DataFrame df = DataFrameTests.MakeDataFrameWithTwoColumns(10);

            df["Int3"] = df["Int1"] * 2 + df["Int2"];
            Assert.Equal(16, df["Int3"][2]);
        }

        [Fact]
        public void TestComputations()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(10);
            df["Int"][0] = -10;
            Assert.Equal(-10, df["Int"][0]);

            df["Int"].Abs();
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

            df["Double"][0] = 100.0;
            df["Double"].CumulativeMax();
            Assert.Equal(100.0, df["Double"][9]);

            df["Float"][0] = -10.0f;
            df["Float"].CumulativeMin();
            Assert.Equal(-10.0f, df["Float"][9]);

            df["Uint"].CumulativeProduct();
            Assert.Equal((uint)0, df["Uint"][9]);

            df["Ushort"].CumulativeSum();
            Assert.Equal((ushort)45, df["Ushort"][9]);

            Assert.Equal(100.0, df["Double"].Max());
            Assert.Equal(-10.0f, df["Float"].Min());
            Assert.Equal((uint)0, df["Uint"].Product());
            Assert.Equal((ushort)165, df["Ushort"].Sum());


            df["Double"][0] = 100.1;
            Assert.Equal(100.1, df["Double"][0]);
            df["Double"].Round();
            Assert.Equal(100.0, df["Double"][0]);
        }

        [Fact]
        public void TestSort()
        {
            DataFrame df = MakeDataFrameWithAllColumnTypes(20);
            df["Int"][0] = 100;
            df["Int"][19] = -1;
            df["Int"][5] = 2000;

            // Sort by "Int" in ascending order
            var sortedDf = df.Sort("Int");
            Assert.Equal(-1,   sortedDf["Int"][0]);
            Assert.Equal(100,  sortedDf["Int"][18]);
            Assert.Equal(2000, sortedDf["Int"][19]);

            // Sort by "Int" in descending order
            sortedDf = df.Sort("Int", false);
            Assert.Equal(-1,   sortedDf["Int"][19]);
            Assert.Equal(100,  sortedDf["Int"][1]);
            Assert.Equal(2000, sortedDf["Int"][0]);

            // Sort by "String" in ascending order
            sortedDf = df.Sort("String");
            Assert.Equal(100, sortedDf["Int"][0]);
            Assert.Equal(8, sortedDf["Int"][18]);
            Assert.Equal(9, sortedDf["Int"][19]);

            sortedDf = df.Sort("String", false);
            Assert.Equal(100, sortedDf["Int"][19]);
            Assert.Equal(8, sortedDf["Int"][1]);
            Assert.Equal(9, sortedDf["Int"][0]);
        }
    }
}
