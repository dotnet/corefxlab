

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameColumn.BinaryOperations.ExplodedColumns.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    public static class BinaryOperations
    {
        public static ByteDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Add(column, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static ShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(column);
        }
        public static UIntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column);
        }
        public static ULongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static UShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(column);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(otherdecimalColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(otherdoubleColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(otherfloatColumn, inPlace);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(otherintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Add(column, inPlace);
        }
        public static LongDataFrameColumn Add(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(otherintColumn, inPlace);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(otherintColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(otherintColumn, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Add(column, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static SByteDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Add(column, inPlace);
        }
        public static ShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(column);
        }
        public static UIntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column);
        }
        public static ULongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static UShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(column);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(othershortColumn, inPlace);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Add(column, inPlace);
        }
        public static UIntDataFrameColumn Add(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column);
        }
        public static ULongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Add(column, inPlace);
        }
        public static ULongDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(otheruintColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(otherulongColumn, inPlace);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(otherushortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column);
        }
        public static DoubleDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column);
        }
        public static FloatDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column);
        }
        public static IntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column);
        }
        public static LongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column);
        }
        public static ULongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Add(column, inPlace);
        }
    }
}
