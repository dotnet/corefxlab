﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Microsoft.Data.Tests
{
    public class BufferTests
    {
        [Fact]
        public void TestNullCounts()
        {
            PrimitiveColumn<int> dataFrameColumn1 = new PrimitiveColumn<int>("Int1", Enumerable.Range(0, 10).Select(x => x));
            dataFrameColumn1.Append(null);
            Assert.Equal(1, dataFrameColumn1.NullCount);

            PrimitiveColumn<int> df2 = new PrimitiveColumn<int>("Int2");
            Assert.Equal(0, df2.NullCount);

            PrimitiveColumn<int> df3 = new PrimitiveColumn<int>("Int3", 10);
            Assert.Equal(0, df3.NullCount);

            // Test null counts with assignments on Primitive Columns
            df2.Append(null);
            df2.Append(1);
            Assert.Equal(1, df2.NullCount);
            df2[1] = 10;
            Assert.Equal(1, df2.NullCount);
            df2[1] = null;
            Assert.Equal(2, df2.NullCount);
            df2[1] = 5;
            Assert.Equal(1, df2.NullCount);
            df2[0] = null;
            Assert.Equal(1, df2.NullCount);

            // Test null counts with assignments on String Columns
            StringColumn strCol = new StringColumn("String", 0);
            Assert.Equal(0, strCol.NullCount);

            StringColumn strCol1 = new StringColumn("String1", 5);
            Assert.Equal(0, strCol1.NullCount);

            StringColumn strCol2 = new StringColumn("String", Enumerable.Range(0, 10).Select(x => x.ToString()));
            Assert.Equal(0, strCol2.NullCount);

            StringColumn strCol3 = new StringColumn("String", Enumerable.Range(0, 10).Select(x => (string)null));
            Assert.Equal(10, strCol3.NullCount);

            strCol.Append(null);
            Assert.Equal(1, strCol.NullCount);
            strCol.Append("foo");
            Assert.Equal(1, strCol.NullCount);
            strCol[1] = "bar";
            Assert.Equal(1, strCol.NullCount);
            strCol[1] = null;
            Assert.Equal(2, strCol.NullCount);
            strCol[1] = "foo";
            Assert.Equal(1, strCol.NullCount);
            strCol[0] = null;
            Assert.Equal(1, strCol.NullCount);
        }

        [Fact]
        public void TestValidity()
        {
            PrimitiveColumn<int> dataFrameColumn1 = new PrimitiveColumn<int>("Int1", Enumerable.Range(0, 10).Select(x => x));
            dataFrameColumn1.Append(null);
            Assert.False(dataFrameColumn1.IsValid(11));
            for (long i = 0; i < dataFrameColumn1.Length - 1; i++)
            {
                Assert.True(dataFrameColumn1.IsValid(i));
            }
        }
    }
}
