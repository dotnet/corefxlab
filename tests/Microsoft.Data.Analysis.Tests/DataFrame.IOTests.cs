﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using Apache.Arrow;
using Xunit;

namespace Microsoft.Data.Analysis.Tests
{
    public partial class DataFrameTests
    {
        internal static void VerifyColumnTypes(DataFrame df, bool testArrowStringColumn = false)
        {
            foreach (DataFrameColumn column in df.Columns)
            {
                switch (column.DataType)
                {
                    case Type boolType when boolType == typeof(bool):
                        Assert.NotNull(column as BooleanDataFrameColumn);
                        break;
                    case Type decimalType when decimalType == typeof(decimal):
                        Assert.NotNull(column as DecimalDataFrameColumn);
                        break;
                    case Type byteType when byteType == typeof(byte):
                        Assert.NotNull(column as ByteDataFrameColumn);
                        break;
                    case Type charType when charType == typeof(char):
                        Assert.NotNull(column as CharDataFrameColumn);
                        break;
                    case Type doubleType when doubleType == typeof(double):
                        Assert.NotNull(column as DoubleDataFrameColumn);
                        break;
                    case Type floatType when floatType == typeof(float):
                        Assert.NotNull(column as SingleDataFrameColumn);
                        break;
                    case Type intType when intType == typeof(int):
                        Assert.NotNull(column as Int32DataFrameColumn);
                        break;
                    case Type longType when longType == typeof(long):
                        Assert.NotNull(column as Int64DataFrameColumn);
                        break;
                    case Type sbyteType when sbyteType == typeof(sbyte):
                        Assert.NotNull(column as SByteDataFrameColumn);
                        break;
                    case Type shortType when shortType == typeof(short):
                        Assert.NotNull(column as Int16DataFrameColumn);
                        break;
                    case Type uintType when uintType == typeof(uint):
                        Assert.NotNull(column as UInt32DataFrameColumn);
                        break;
                    case Type ulongType when ulongType == typeof(ulong):
                        Assert.NotNull(column as UInt64DataFrameColumn);
                        break;
                    case Type ushortType when ushortType == typeof(ushort):
                        Assert.NotNull(column as UInt16DataFrameColumn);
                        break;
                    case Type stringType when stringType == typeof(string):
                        if (!testArrowStringColumn)
                        {
                            Assert.NotNull(column as StringDataFrameColumn);
                        }
                        else
                        {
                            Assert.NotNull(column as ArrowStringDataFrameColumn);
                        }
                        break;
                    default:
                        throw new NotImplementedException("Unit test has to be updated");
                }
            }
        }

        [Fact]
        public void TestReadCsvWithHeader()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount
CMT,1,1,1271,3.8,CRD,17.5
CMT,1,1,474,1.5,CRD,8
CMT,1,1,637,1.4,CRD,8.5
CMT,1,1,181,0.6,CSH,4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data));
            Assert.Equal(4, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["vendor_id"][3]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(7, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["vendor_id"][2]);
            VerifyColumnTypes(df);
        }

        [Fact]
        public void TestReadCsvNoHeader()
        {
            string data = @"CMT,1,1,1271,3.8,CRD,17.5
CMT,1,1,474,1.5,CRD,8
CMT,1,1,637,1.4,CRD,8.5
CMT,1,1,181,0.6,CSH,4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data), header: false);
            Assert.Equal(4, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["Column0"][3]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), header: false, numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(7, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["Column0"][2]);
            VerifyColumnTypes(df);
        }

        [Fact]
        public void TestReadCsvWithTypes()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount
CMT,1,1,1271,3.8,CRD,17.5
CMT,1,1,474,1.5,CRD,8
CMT,1,1,637,1.4,CRD,8.5
,,,,,,
CMT,1,1,181,0.6,CSH,4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data), dataTypes: new Type[] { typeof(string), typeof(short), typeof(int), typeof(long), typeof(float), typeof(string), typeof(double) });
            Assert.Equal(5, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);

            Assert.True(typeof(string) == df.Columns[0].DataType);
            Assert.True(typeof(short) == df.Columns[1].DataType);
            Assert.True(typeof(int) == df.Columns[2].DataType);
            Assert.True(typeof(long) == df.Columns[3].DataType);
            Assert.True(typeof(float) == df.Columns[4].DataType);
            Assert.True(typeof(string) == df.Columns[5].DataType);
            Assert.True(typeof(double) == df.Columns[6].DataType);
            VerifyColumnTypes(df);

            foreach (var column in df.Columns)
            {
                if (column.DataType != typeof(string))
                {
                    Assert.Equal(1, column.NullCount);
                }
                else
                {
                    Assert.Equal(0, column.NullCount);
                }
            }
            var nullRow = df.Rows[3];
            Assert.Equal("", nullRow[0]);
            Assert.Null(nullRow[1]);
            Assert.Null(nullRow[2]);
            Assert.Null(nullRow[3]);
            Assert.Null(nullRow[4]);
            Assert.Equal("", nullRow[5]);
            Assert.Null(nullRow[6]);
        }

        [Fact]
        public void TestReadCsvWithPipeSeparator()
        {
            string data = @"vendor_id|rate_code|passenger_count|trip_time_in_secs|trip_distance|payment_type|fare_amount
CMT|1|1|1271|3.8|CRD|17.5
CMT|1|1|474|1.5|CRD|8
CMT|1|1|637|1.4|CRD|8.5
||||||
CMT|1|1|181|0.6|CSH|4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data), separator: '|');

            Assert.Equal(5, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["vendor_id"][4]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), separator: '|', numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(7, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["vendor_id"][2]);
            VerifyColumnTypes(df);

            var nullRow = df.Rows[3];
            Assert.Equal("", nullRow[0]);
            Assert.Null(nullRow[1]);
            Assert.Null(nullRow[2]);
            Assert.Null(nullRow[3]);
            Assert.Null(nullRow[4]);
            Assert.Equal("", nullRow[5]);
            Assert.Null(nullRow[6]);
        }

        [Fact]
        public void TestReadCsvWithSemicolonSeparator()
        {
            string data = @"vendor_id;rate_code;passenger_count;trip_time_in_secs;trip_distance;payment_type;fare_amount
CMT;1;1;1271;3.8;CRD;17.5
CMT;1;1;474;1.5;CRD;8
CMT;1;1;637;1.4;CRD;8.5
;;;;;;
CMT;1;1;181;0.6;CSH;4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data), separator: ';');

            Assert.Equal(5, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["vendor_id"][4]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), separator: ';', numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(7, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["vendor_id"][2]);
            VerifyColumnTypes(df);

            var nullRow = df.Rows[3];
            Assert.Equal("", nullRow[0]);
            Assert.Null(nullRow[1]);
            Assert.Null(nullRow[2]);
            Assert.Null(nullRow[3]);
            Assert.Null(nullRow[4]);
            Assert.Equal("", nullRow[5]);
            Assert.Null(nullRow[6]);
        }

        [Fact]
        public void TestReadCsvWithExtraColumnInHeader()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount,extra
CMT,1,1,1271,3.8,CRD,17.5
CMT,1,1,474,1.5,CRD,8
CMT,1,1,637,1.4,CRD,8.5
CMT,1,1,181,0.6,CSH,4.5";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }
            DataFrame df = DataFrame.LoadCsv(GetStream(data));

            Assert.Equal(4, df.Rows.Count);
            Assert.Equal(7, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["vendor_id"][3]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(7, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["vendor_id"][2]);
            VerifyColumnTypes(df);
        }

        [Fact]
        public void TestReadCsvWithExtraColumnInRow()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount
CMT,1,1,1271,3.8,CRD,17.5,0
CMT,1,1,474,1.5,CRD,8,0
CMT,1,1,637,1.4,CRD,8.5,0
CMT,1,1,181,0.6,CSH,4.5,0";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }

            Assert.Throws<IndexOutOfRangeException>(() => DataFrame.LoadCsv(GetStream(data)));
        }

        [Fact]
        public void TestReadCsvWithLessColumnsInRow()
        {
            string data = @"vendor_id,rate_code,passenger_count,trip_time_in_secs,trip_distance,payment_type,fare_amount
CMT,1,1,1271,3.8,CRD
CMT,1,1,474,1.5,CRD
CMT,1,1,637,1.4,CRD
CMT,1,1,181,0.6,CSH";

            Stream GetStream(string streamData)
            {
                return new MemoryStream(Encoding.Default.GetBytes(streamData));
            }

            DataFrame df = DataFrame.LoadCsv(GetStream(data));
            Assert.Equal(4, df.Rows.Count);
            Assert.Equal(6, df.Columns.Count);
            Assert.Equal("CMT", df.Columns["vendor_id"][3]);
            VerifyColumnTypes(df);

            DataFrame reducedRows = DataFrame.LoadCsv(GetStream(data), numberOfRowsToRead: 3);
            Assert.Equal(3, reducedRows.Rows.Count);
            Assert.Equal(6, reducedRows.Columns.Count);
            Assert.Equal("CMT", reducedRows.Columns["vendor_id"][2]);
            VerifyColumnTypes(df);

        }
    }
}
