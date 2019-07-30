// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Data.Tests
{
    public partial class DataFrameTests
    {
        public static DataFrame MakeDataFrameWithTwoColumns(int length)
        {
            BaseColumn dataFrameColumn1 = new PrimitiveColumn<int>("Int1", Enumerable.Range(0, length).Select(x => x));
            BaseColumn dataFrameColumn2 = new PrimitiveColumn<int>("Int2", Enumerable.Range(10, length).Select(x => x));
            dataFrameColumn1[length / 2] = null;
            dataFrameColumn2[length / 2] = null;
            Data.DataFrame dataFrame = new Data.DataFrame();
            dataFrame.InsertColumn(0, dataFrameColumn1);
            dataFrame.InsertColumn(1, dataFrameColumn2);
            return dataFrame;
        }

        public static DataFrame MakeDataFrameWithAllColumnTypes(int length, bool withNulls = true)
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
            return df;
        }

        public static DataFrame MakeDataFrameWithNumericColumns(int length, bool withNulls = true)
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

            DataFrame dataFrame = new DataFrame(new List<BaseColumn> { byteColumn, charColumn, decimalColumn, doubleColumn, floatColumn, intColumn, longColumn, sbyteColumn, shortColumn, uintColumn, ulongColumn, ushortColumn });

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

            // Test the computation results
            df["Double"][0] = 100.0;
            df["Double"].CumulativeMax();
            Assert.Equal(100.0, df["Double"][9]);

            df["Float"][0] = -10.0f;
            df["Float"].CumulativeMin();
            Assert.Equal(-10.0f, df["Float"][9]);

            df["Uint"].CumulativeProduct();
            Assert.Equal((uint)0, df["Uint"][9]);

            df["Ushort"].CumulativeSum();
            Assert.Equal((ushort)40, df["Ushort"][9]);

            Assert.Equal(100.0, df["Double"].Max());
            Assert.Equal(-10.0f, df["Float"].Min());
            Assert.Equal((uint)0, df["Uint"].Product());
            Assert.Equal((ushort)140, df["Ushort"].Sum());

            df["Double"][0] = 100.1;
            Assert.Equal(100.1, df["Double"][0]);
            df["Double"].Round();
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
            DataFrame df = MakeDataFrameWithAllColumnTypes(20);
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

        [Fact]
        public void TestPrimitiveColumnSort()
        {
            // Primitive Column Sort
            PrimitiveColumn<int> intColumn = new PrimitiveColumn<int>("Int", 0);
            Assert.Equal(0, intColumn.NullCount);
            intColumn.AppendMany(null, 5);
            Assert.Equal(5, intColumn.NullCount);

            // Should handle all nulls
            PrimitiveColumn<int> sortedIntColumn = intColumn.Sort() as PrimitiveColumn<int>;
            Assert.Equal(5, sortedIntColumn.NullCount);
            Assert.Null(sortedIntColumn[0]);

            for (int i = 0; i < 5; i++)
            {
                intColumn.Append(i);
            }
            Assert.Equal(5, intColumn.NullCount);

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

        [Fact]
        public void TestJoin()
        {
            DataFrame left = MakeDataFrameWithAllColumnTypes(10);
            DataFrame right = MakeDataFrameWithAllColumnTypes(5);

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
            right = MakeDataFrameWithAllColumnTypes(15);
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
        }

        [Fact]
        public void TestGoupByDifferentColumnTypes()
        {
            void GroupCountAndAssert(DataFrame df)
            {
                DataFrame grouped = df.GroupBy("Column1").Count();
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

            df = MakeDataFrame<int , bool>(10, false);
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
    }
}
