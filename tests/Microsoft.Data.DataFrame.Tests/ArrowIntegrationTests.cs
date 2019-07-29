// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apache.Arrow;
using Apache.Arrow.Ipc;
using Xunit;

namespace Microsoft.Data.Tests
{
    public class ArrowIntegrationTests
    {
        [Fact]
        public void TestArrowIntegration()
        {
            RecordBatch originalBatch = new RecordBatch.Builder()
                .Append("Column1", false, col => col.Int32(array => array.AppendRange(Enumerable.Range(0, 10))))
                .Append("Column2", true, new Int32Array(
                    valueBuffer: new ArrowBuffer.Builder<int>().AppendRange(Enumerable.Range(0, 10)).Build(),
                    nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0xfd).Append(0xff).Build(),
                    length: 10,
                    nullCount: 1,
                    offset: 0))
                .Append("Column3", true, new Int32Array(
                    valueBuffer: new ArrowBuffer.Builder<int>().AppendRange(Enumerable.Range(0, 10)).Build(),
                    nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0x00).Append(0x00).Build(),
                    length: 10,
                    nullCount: 10,
                    offset: 0))
                .Append("NullableBooleanColumn", true, new BooleanArray(
                    valueBuffer: new ArrowBuffer.Builder<byte>().Append(0xfd).Append(0xff).Build(),
                    nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0xed).Append(0xff).Build(),
                    length: 10,
                    nullCount: 2,
                    offset: 0))
                .Append("StringColumn", false, new StringArray.Builder().AppendRange(Enumerable.Range(0, 10).Select(x => x.ToString())).Build())
                .Build();

            DataFrame df = new DataFrame(originalBatch);

            IEnumerable<RecordBatch> recordBatches = df.AsArrowRecordBatches();

            foreach (RecordBatch batch in recordBatches)
            {
                RecordBatchComparer.CompareBatches(originalBatch, batch);
            }
        }

        [Fact]
        public void TestEmptyDataFrameRecordBatch()
        {
            PrimitiveColumn<int> ageColumn = new PrimitiveColumn<int>("Age");
            PrimitiveColumn<int> lengthColumn = new PrimitiveColumn<int>("CharCount");
            DataFrame df = new DataFrame(new List<BaseColumn>() { ageColumn, lengthColumn });

            IEnumerable<RecordBatch> recordBatches = df.AsArrowRecordBatches();
            bool foundARecordBatch = false;
            foreach (RecordBatch recordBatch in recordBatches)
            {
                foundARecordBatch = true;
                MemoryStream stream = new MemoryStream();
                ArrowStreamWriter writer = new ArrowStreamWriter(stream, recordBatch.Schema);
                writer.WriteRecordBatchAsync(recordBatch).GetAwaiter().GetResult();

                stream.Position = 0;
                ArrowStreamReader reader = new ArrowStreamReader(stream);
                RecordBatch readRecordBatch = reader.ReadNextRecordBatch();
                while (readRecordBatch != null)
                {
                    RecordBatchComparer.CompareBatches(recordBatch, readRecordBatch);
                    readRecordBatch = reader.ReadNextRecordBatch();
                }
            }
            Assert.True(foundARecordBatch);
        }
    }
}
