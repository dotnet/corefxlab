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
        private class ArrayComparer :
            IArrowArrayVisitor<Int8Array>,
            IArrowArrayVisitor<Int16Array>,
            IArrowArrayVisitor<Int32Array>,
            IArrowArrayVisitor<Int64Array>,
            IArrowArrayVisitor<UInt8Array>,
            IArrowArrayVisitor<UInt16Array>,
            IArrowArrayVisitor<UInt32Array>,
            IArrowArrayVisitor<UInt64Array>,
            IArrowArrayVisitor<FloatArray>,
            IArrowArrayVisitor<DoubleArray>,
            IArrowArrayVisitor<BooleanArray>,
            IArrowArrayVisitor<TimestampArray>,
            IArrowArrayVisitor<Date32Array>,
            IArrowArrayVisitor<Date64Array>,
            IArrowArrayVisitor<ListArray>,
            IArrowArrayVisitor<StringArray>,
            IArrowArrayVisitor<BinaryArray>
        {
            private readonly IArrowArray _expectedArray;

            public ArrayComparer(IArrowArray expectedArray)
            {
                _expectedArray = expectedArray;
            }

            public void Visit(Int8Array array) => CompareArrays(array);
            public void Visit(Int16Array array) => CompareArrays(array);
            public void Visit(Int32Array array) => CompareArrays(array);
            public void Visit(Int64Array array) => CompareArrays(array);
            public void Visit(UInt8Array array) => CompareArrays(array);
            public void Visit(UInt16Array array) => CompareArrays(array);
            public void Visit(UInt32Array array) => CompareArrays(array);
            public void Visit(UInt64Array array) => CompareArrays(array);
            public void Visit(FloatArray array) => CompareArrays(array);
            public void Visit(DoubleArray array) => CompareArrays(array);
            public void Visit(BooleanArray array) => CompareArrays(array);
            public void Visit(TimestampArray array) => CompareArrays(array);
            public void Visit(Date32Array array) => CompareArrays(array);
            public void Visit(Date64Array array) => CompareArrays(array);
            public void Visit(ListArray array) => throw new NotImplementedException();
            public void Visit(StringArray array) => CompareArrays(array);
            public void Visit(BinaryArray array) => throw new NotImplementedException();
            public void Visit(IArrowArray array) => throw new NotImplementedException();

            private void CompareArrays<T>(PrimitiveArray<T> actualArray)
                where T : struct, IEquatable<T>
            {
                Assert.IsAssignableFrom<PrimitiveArray<T>>(_expectedArray);
                PrimitiveArray<T> expectedArray = (PrimitiveArray<T>)_expectedArray;

                Assert.Equal(expectedArray.Length, actualArray.Length);
                Assert.Equal(expectedArray.NullCount, actualArray.NullCount);
                Assert.Equal(expectedArray.Offset, actualArray.Offset);

                Assert.True(expectedArray.NullBitmapBuffer.Span.SequenceEqual(actualArray.NullBitmapBuffer.Span));
                Assert.True(expectedArray.Values.Slice(0, expectedArray.Length).SequenceEqual(actualArray.Values.Slice(0, actualArray.Length)));
            }

            private void CompareArrays(BooleanArray actualArray)
            {
                Assert.IsAssignableFrom<BooleanArray>(_expectedArray);
                BooleanArray expectedArray = (BooleanArray)_expectedArray;

                Assert.Equal(expectedArray.Length, actualArray.Length);
                Assert.Equal(expectedArray.NullCount, actualArray.NullCount);
                Assert.Equal(expectedArray.Offset, actualArray.Offset);

                Assert.True(expectedArray.NullBitmapBuffer.Span.SequenceEqual(actualArray.NullBitmapBuffer.Span));
                int booleanByteCount = BitUtility.ByteCount(expectedArray.Length);
                Assert.True(expectedArray.Values.Slice(0, booleanByteCount).SequenceEqual(actualArray.Values.Slice(0, booleanByteCount)));
            }

            private void CompareArrays(StringArray actualArray)
            {
                Assert.IsAssignableFrom<StringArray>(_expectedArray);
                StringArray expectedArray = (StringArray)_expectedArray;

                Assert.Equal(expectedArray.Length, actualArray.Length);
                Assert.Equal(expectedArray.NullCount, actualArray.NullCount);
                Assert.Equal(expectedArray.Offset, actualArray.Offset);

                Assert.True(expectedArray.NullBitmapBuffer.Span.SequenceEqual(actualArray.NullBitmapBuffer.Span));
                Assert.True(expectedArray.Values.Slice(0, expectedArray.Length).SequenceEqual(actualArray.Values.Slice(0, actualArray.Length)));
            }
        }

        internal class FieldComparer
        {
            public static bool Equals(Field f1, Field f2)
            {
                if (ReferenceEquals(f1, f2))
                {
                    return true;
                }
                if (f2 != null && f1 != null && f1.Name == f2.Name && f1.IsNullable == f2.IsNullable &&
                    f1.DataType.TypeId == f2.DataType.TypeId && f1.HasMetadata == f2.HasMetadata)
                {
                    if (f1.HasMetadata && f2.HasMetadata)
                    {
                        return f1.Metadata.Keys.Count() == f2.Metadata.Keys.Count() &&
                               f1.Metadata.Keys.All(k => f2.Metadata.ContainsKey(k) && f1.Metadata[k] == f2.Metadata[k]) &&
                               f2.Metadata.Keys.All(k => f1.Metadata.ContainsKey(k) && f2.Metadata[k] == f1.Metadata[k]);
                    }
                    return true;
                }
                return false;
            }
        }

        internal class SchemaComparer
        {
            public static bool Equals(Schema s1, Schema s2)
            {
                if (ReferenceEquals(s1, s2))
                {
                    return true;
                }
                if (s2 == null || s1 == null || s1.HasMetadata != s2.HasMetadata || s1.Fields.Count != s2.Fields.Count)
                {
                    return false;
                }

                if (!s1.Fields.Keys.All(k => s2.Fields.ContainsKey(k) && FieldComparer.Equals(s1.Fields[k], s2.Fields[k])) ||
                    !s2.Fields.Keys.All(k => s1.Fields.ContainsKey(k) && FieldComparer.Equals(s2.Fields[k], s1.Fields[k])))
                {
                    return false;
                }

                if (s1.HasMetadata && s2.HasMetadata)
                {
                    return s1.Metadata.Keys.Count() == s2.Metadata.Keys.Count() &&
                           s1.Metadata.Keys.All(k => s2.Metadata.ContainsKey(k) && s1.Metadata[k] == s2.Metadata[k]) &&
                           s2.Metadata.Keys.All(k => s1.Metadata.ContainsKey(k) && s2.Metadata[k] == s1.Metadata[k]);
                }
                return true;
            }
        }

        internal static void CompareBatches(RecordBatch expectedBatch, RecordBatch actualBatch)
        {
            Assert.True(SchemaComparer.Equals(expectedBatch.Schema, actualBatch.Schema));
            Assert.Equal(expectedBatch.Length, actualBatch.Length);
            Assert.Equal(expectedBatch.ColumnCount, actualBatch.ColumnCount);

            for (int i = 0; i < expectedBatch.ColumnCount; i++)
            {
                IArrowArray expectedArray = expectedBatch.Arrays.ElementAt(i);
                IArrowArray actualArray = actualBatch.Arrays.ElementAt(i);

                actualArray.Accept(new ArrayComparer(expectedArray));
            }
        }

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
                CompareBatches(originalBatch, batch);
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
                    CompareBatches(recordBatch, readRecordBatch);
                    readRecordBatch = reader.ReadNextRecordBatch();
                }
            }
            Assert.True(foundARecordBatch);
        }
    }
}
