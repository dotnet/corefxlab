// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Collections.Generic;
using Microsoft.Data;
using Xunit;

namespace Microsoft.Data.Tests
{
    public class DataFrameTests
    {
        static public DataFrame MakeTestTableWithTwoColumns(int length)
        {
            BaseDataFrameColumn dataFrameColumn1 = new PrimitiveDataFrameColumn<int>("Int1", Enumerable.Range(0, length).Select(x => x));
            BaseDataFrameColumn dataFrameColumn2 = new PrimitiveDataFrameColumn<int>("Int2", Enumerable.Range(10, length).Select(x => x));
            Data.DataFrame dataFrame = new Data.DataFrame();
            dataFrame.InsertColumn(0, dataFrameColumn1);
            dataFrame.InsertColumn(1, dataFrameColumn2);
            return dataFrame;
        }
        [Fact]
        public void TestIndexer()
        {
            Data.DataFrame dataFrame = MakeTestTableWithTwoColumns(length: 10);
            var foo = dataFrame[0, 0];
            Assert.Equal(0, dataFrame[0, 0]);
            Assert.Equal(11, dataFrame[1, 1]);
            Assert.Equal(2, dataFrame.Columns().Count);
            Assert.Equal("Int1", dataFrame.Columns()[0]);

            var headList = dataFrame.Head(5);
            Assert.Equal(14, (int)headList[4][1]);

            var tailList = dataFrame.Tail(5);
            Assert.Equal(19, (int)tailList[4][1]);

            dataFrame[2, 1] = 1000;
            Assert.Equal(1000, dataFrame[2, 1]);

            var row = dataFrame[4];
            Assert.Equal(14, (int)row[1]);

            var column = dataFrame["Int2"] as PrimitiveDataFrameColumn<int>;
            Assert.Equal(1000, (int)column[2]);

            Assert.Throws<System.ArgumentException>(() => dataFrame["Int5"]);
        }

        [Fact]
        public void ColumnAndTableCreationTest()
        {
            BaseDataFrameColumn intColumn = new PrimitiveDataFrameColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            BaseDataFrameColumn floatColumn = new PrimitiveDataFrameColumn<float>("FloatColumn", Enumerable.Range(0, 10).Select(x => (float)x));
            DataFrame dataFrame = new DataFrame();
            dataFrame.InsertColumn(0, intColumn);
            dataFrame.InsertColumn(1, floatColumn);
            Assert.Equal(10, dataFrame.RowCount);
            Assert.Equal(2, dataFrame.ColumnCount);
            Assert.Equal(10, dataFrame.Column(0).Length);
            Assert.Equal("IntColumn", dataFrame.Column(0).Name);
            Assert.Equal(10, dataFrame.Column(1).Length);
            Assert.Equal("FloatColumn", dataFrame.Column(1).Name);
            
            BaseDataFrameColumn bigColumn = new PrimitiveDataFrameColumn<float>("BigColumn", Enumerable.Range(0, 11).Select(x => (float)x));
            BaseDataFrameColumn repeatedName = new PrimitiveDataFrameColumn<float>("FloatColumn", Enumerable.Range(0, 10).Select(x => (float)x));
            Assert.Throws<System.ArgumentException>( () => dataFrame.InsertColumn(2, bigColumn));
            Assert.Throws<System.ArgumentException>( () => dataFrame.InsertColumn(2, repeatedName));
            Assert.Throws<System.ArgumentException>( () => dataFrame.InsertColumn(10, repeatedName));

            Assert.Equal(2, dataFrame.ColumnCount);
            BaseDataFrameColumn intColumnCopy = new PrimitiveDataFrameColumn<int>("IntColumn", Enumerable.Range(0, 10).Select(x => x));
            Assert.Throws<System.ArgumentException>(() => dataFrame.SetColumn(1, intColumnCopy));
            
            BaseDataFrameColumn differentIntColumn = new PrimitiveDataFrameColumn<int>("IntColumn1", Enumerable.Range(0, 10).Select(x => x));
            dataFrame.SetColumn(1, differentIntColumn);
            Assert.True(differentIntColumn == dataFrame.Column(1));

            dataFrame.RemoveColumn(1);
            Assert.Equal(1, dataFrame.ColumnCount);
            Assert.True(intColumn == dataFrame.Column(0));
        }

        [Fact]
        public void TestBinaryOperations()
        {
            DataFrame df = DataFrameTests.MakeTestTableWithTwoColumns(10);
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
            Assert.Equal(5, df.And(5)[5, 0]);
            Assert.Equal(5, df.And(listOfInts)[5, 0]);
            Assert.Equal(5, df.Or(5)[5, 0]);
            Assert.Equal(5, df.Or(listOfInts)[5, 0]);
            Assert.Equal(0, df.Xor(5)[5, 0]);
            Assert.Equal(0, df.Xor(listOfInts)[5, 0]);
            Assert.Equal(2, df.LeftShift<int>(1)[1, 0]);
            Assert.Equal(1, df.RightShift<int>(1)[2, 0]);
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
        // TODO: Add tests for other column types
    }
}
