// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xunit;

namespace Microsoft.Data.Analysis.IO.Tests
{
    public partial class DataFrameIOTests
    {
        [Fact]
        public void TestReadCsvInvariantCulture()
        {
            string data = @"date,value
                18-2-2001,18.4
                29-9-2010,22.411
                1-11-2007,812.34";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }

            DataFrame df = DataFrameIO.LoadCsv(() => GetStream(data), SetTextFieldParserProperties, dataTypes: new Type[] {typeof(string), typeof(double)});

            Assert.Equal(3, df.Rows.Count);
            Assert.Equal(2, df.Columns.Count);
            Assert.Equal(Convert.ToDouble(18.4), df.Columns["value"][0]);
            VerifyColumnTypes(df);
        }

        [Fact]
        public void TestReadCsvCurrentCulture()
        {
            string data = @"date;value
                18-2-2001;18,4
                29-9-2010;22,411
                1-11-2007;812,34";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }

            DataFrame df = DataFrameIO.LoadCsv(() => GetStream(data), SetTextFieldParserProperties,
                separator: ';', dataTypes: new Type[] { typeof(string), typeof(double) },
                cultureInfo: CultureInfo.CurrentCulture); 

            Assert.Equal(3, df.Rows.Count);
            Assert.Equal(2, df.Columns.Count);
            Assert.Equal(Convert.ToDouble(18.4), df.Columns["value"][0]);
            VerifyColumnTypes(df);
        }

    }
}
