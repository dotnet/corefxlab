// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Microsoft.Data.Tests
{
    public partial class DataFrameTests
    {
        [Fact]
        public void TestReadCsv()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount
CMT,1,1,1271,3.8,CRD,17.5
CMT,1,1,474,1.5,CRD,8
CMT,1,1,637,1.4,CRD,8.5
CMT,1,1,181,0.6,CSH,4.5";

            Stream GetStream(string data)
            {
                return new MemoryStream(Encoding.Default.GetBytes(data));
            }
            DataFrame df = DataFrame.ReadStream(() => new StreamReader(GetStream(data)));
            Assert.Equal(4, df.RowCount);
            Assert.Equal(7, df.ColumnCount);
            Assert.Equal("CMT", df["vendor_id"][3]);
        }
    }
}
