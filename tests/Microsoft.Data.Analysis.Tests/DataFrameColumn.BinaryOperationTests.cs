

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameColumn.BinaryOperationTests.tt. Do not modify directly

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Data.Analysis.Tests
{
    public partial class DataFrameColumnTests
    {
        [Fact]
        public void AddByteDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            ByteDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)(2 * x));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddDecimalDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            DecimalDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)(2 * x));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddDoubleDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            DoubleDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (double)(2 * x));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddFloatDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            FloatDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (float)(2 * x));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            IntDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (int)(2 * x));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddLongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            LongDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (long)(2 * x));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            ShortDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (short)(2 * x));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddUIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            UIntDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)(2 * x));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddULongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            ULongDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)(2 * x));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddUShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            UShortDataFrameColumn columnResult = column + otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)(2 * x));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x + (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x + (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x + (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x + (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x + (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x + (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x + (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x + (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x + (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void AddUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = column + value;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x + (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x + (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x + (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x + (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x + (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x + (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x + (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x + (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x + (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x + (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseAddUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = value + column;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x + (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractByteDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            ByteDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)0);
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractDecimalDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            DecimalDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)0);
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractDoubleDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            DoubleDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (double)0);
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractFloatDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            FloatDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (float)0);
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            IntDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (int)0);
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractLongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            LongDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (long)0);
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            ShortDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (short)0);
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractUIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            UIntDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)0);
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractULongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            ULongDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)0);
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractUShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            UShortDataFrameColumn columnResult = column - otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)0);
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x - (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x - (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x - (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x - (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x - (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x - (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x - (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x - (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x - (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void SubtractUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = column - value;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x - (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)value - (byte)x));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)value - (decimal)x));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)value - (double)x));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)value - (float)x));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)value - (int)x));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)value - (long)x));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)value - (short)x));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)value - (uint)x));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)value - (ulong)x));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseSubtractUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = value - column;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)value - (ushort)x));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyByteDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            ByteDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)(x * x));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyDecimalDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            DecimalDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)(x * x));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyDoubleDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            DoubleDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (double)(x * x));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyFloatDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            FloatDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (float)(x * x));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            IntDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (int)(x * x));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyLongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            LongDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (long)(x * x));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            ShortDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (short)(x * x));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyUIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            UIntDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)(x * x));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyULongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            ULongDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)(x * x));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyUShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            UShortDataFrameColumn columnResult = column * otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)(x * x));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x * (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x * (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x * (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x * (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x * (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x * (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x * (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x * (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x * (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void MultiplyUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = column * value;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x * (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x * (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x * (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x * (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x * (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x * (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x * (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x * (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x * (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x * (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseMultiplyUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = value * column;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x * (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideByteDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            ByteDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)(1));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideDecimalDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            DecimalDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)(1));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideDoubleDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            DoubleDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (double)(1));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideFloatDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            FloatDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (float)(1));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            IntDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (int)(1));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideLongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            LongDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (long)(1));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            ShortDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (short)(1));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideUIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            UIntDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)(1));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideULongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            ULongDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)(1));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideUShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            UShortDataFrameColumn columnResult = column / otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)(1));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x / (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x / (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x / (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x / (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x / (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x / (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x / (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x / (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x / (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void DivideUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = column / value;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x / (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)value / (byte)x));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)value / (decimal)x));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)value / (double)x));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)value / (float)x));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)value / (int)x));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)value / (long)x));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)value / (short)x));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)value / (uint)x));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)value / (ulong)x));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseDivideUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = value / column;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)value / (ushort)x));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloByteDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            ByteDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)(0));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloDecimalDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            DecimalDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)(0));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloDoubleDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            DoubleDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (double)(0));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloFloatDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            FloatDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (float)(0));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            IntDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (int)(0));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloLongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            LongDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (long)(0));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            ShortDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (short)(0));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloUIntDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            UIntDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)(0));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloULongDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            ULongDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)(0));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloUShortDataFrameColumnToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            UShortDataFrameColumn columnResult = column % otherColumn;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)(0));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)x % (byte)value));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)x % (decimal)value));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)x % (double)value));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)x % (float)value));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)x % (int)value));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)x % (long)value));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)x % (short)value));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)x % (uint)value));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)x % (ulong)value));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ModuloUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = column % value;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)x % (ushort)value));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloByteToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = (byte)5;
            ByteDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (byte)((byte)value % (byte)x));
            var verifyColumn = new ByteDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloDecimalToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = (byte)5;
            DecimalDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (decimal)((decimal)value % (decimal)x));
            var verifyColumn = new DecimalDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloDoubleToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = (byte)5;
            DoubleDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (double)((double)value % (double)x));
            var verifyColumn = new DoubleDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloFloatToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = (byte)5;
            FloatDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (float)((float)value % (float)x));
            var verifyColumn = new FloatDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = (byte)5;
            IntDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (int)((int)value % (int)x));
            var verifyColumn = new IntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloLongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = (byte)5;
            LongDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (long)((long)value % (long)x));
            var verifyColumn = new LongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = (byte)5;
            ShortDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (short)((short)value % (short)x));
            var verifyColumn = new ShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloUIntToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = (byte)5;
            UIntDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (uint)((uint)value % (uint)x));
            var verifyColumn = new UIntDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloULongToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = (byte)5;
            ULongDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (ulong)((ulong)value % (ulong)x));
            var verifyColumn = new ULongDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ReverseModuloUShortToByteDataFrameColumn()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = (byte)5;
            UShortDataFrameColumn columnResult = value % column;
            var verify = Enumerable.Range(1, 10).Select(x => (ushort)((ushort)value % (ushort)x));
            var verifyColumn = new UShortDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToDecimal()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToDouble()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToFloat()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToLong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToSByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (sbyte)x);
            SByteDataFrameColumn otherColumn = new SByteDataFrameColumn("SByte", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToUInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToULong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToUShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarDecimal()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarDouble()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarFloat()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarLong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarSByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            sbyte value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarUInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarULong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseEqualsBoolToScalarUShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn otherColumn = new ByteDataFrameColumn("Byte", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToDecimal()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (decimal)x);
            DecimalDataFrameColumn otherColumn = new DecimalDataFrameColumn("Decimal", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToDouble()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (double)x);
            DoubleDataFrameColumn otherColumn = new DoubleDataFrameColumn("Double", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToFloat()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (float)x);
            FloatDataFrameColumn otherColumn = new FloatDataFrameColumn("Float", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (int)x);
            IntDataFrameColumn otherColumn = new IntDataFrameColumn("Int", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToLong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (long)x);
            LongDataFrameColumn otherColumn = new LongDataFrameColumn("Long", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToSByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (sbyte)x);
            SByteDataFrameColumn otherColumn = new SByteDataFrameColumn("SByte", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (short)x);
            ShortDataFrameColumn otherColumn = new ShortDataFrameColumn("Short", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToUInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (uint)x);
            UIntDataFrameColumn otherColumn = new UIntDataFrameColumn("UInt", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToULong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ulong)x);
            ULongDataFrameColumn otherColumn = new ULongDataFrameColumn("ULong", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToUShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            var otherColumnEnumerable = Enumerable.Range(1, 10).Select(x => (ushort)x);
            UShortDataFrameColumn otherColumn = new UShortDataFrameColumn("UShort", otherColumnEnumerable);
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(otherColumn);
            var verify = Enumerable.Range(1, 10).Select(x => true);
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());

            // If this is equals, change thisx to false
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            byte value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarDecimal()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            decimal value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarDouble()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            double value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarFloat()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            float value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            int value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarLong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            long value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarSByte()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            sbyte value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            short value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarUInt()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            uint value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarULong()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ulong value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
        [Fact]
        public void ElementwiseNotEqualsBoolToScalarUShort()
        {
            var columnEnumerable = Enumerable.Range(1, 10).Select(x => (byte)x);
            ByteDataFrameColumn column = new ByteDataFrameColumn("Byte", columnEnumerable);
            ushort value = 100;
            BoolDataFrameColumn columnResult = column.ElementwiseNotEquals(value);
            var verify = Enumerable.Range(1, 10).Select(x => (bool)(false));
            var verifyColumn = new BoolDataFrameColumn("Verify", verify);
            Assert.Equal(columnResult.Length, verify.Count());
            Assert.True(columnResult.ElementwiseNotEquals(verifyColumn).All());
        }
    }
}
