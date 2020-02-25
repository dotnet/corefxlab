﻿

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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static IntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column, inPlace: true);
        }
        public static LongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
        }
        public static ShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(column, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Add(column, inPlace);
        }
        public static LongDataFrameColumn Add(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
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
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static IntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column, inPlace: true);
        }
        public static LongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
        }
        public static SByteDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Add(column, inPlace);
        }
        public static ShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(column, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(column, inPlace: true);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static IntDataFrameColumn Add(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column, inPlace: true);
        }
        public static LongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
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
            return (UIntDataFrameColumn)uintColumn.Add(column, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Add(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static LongDataFrameColumn Add(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
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
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
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
            return (DecimalDataFrameColumn)decimalColumn.Add(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(column, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(column, inPlace: true);
        }
        public static IntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(column, inPlace: true);
        }
        public static LongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(column, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(column, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(column, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Add(column, inPlace);
        }
        public static ByteDataFrameColumn Add(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Add(value, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static ShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(value, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(value, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(value, inPlace: true);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Add(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Add(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Add(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Add(value, inPlace);
        }
        public static LongDataFrameColumn Add(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Add(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Add(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Add(value, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Add(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static SByteDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Add(value, inPlace);
        }
        public static ShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Add(value, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(value, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Add(value, inPlace: true);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Add(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn Add(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Add(value, inPlace);
        }
        public static UIntDataFrameColumn Add(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(value, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Add(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Add(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Add(value, inPlace);
        }
        public static ULongDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static UIntDataFrameColumn Add(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace);
        }
        public static ULongDataFrameColumn Add(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Add(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Add(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Add(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Add(value, inPlace: true);
        }
        public static FloatDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Add(value, inPlace: true);
        }
        public static IntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Add(value, inPlace: true);
        }
        public static LongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Add(value, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Add(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Add(value, inPlace: true);
        }
        public static ULongDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Add(value, inPlace: true);
        }
        public static UShortDataFrameColumn Add(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Add(value, inPlace);
        }
        public static ByteDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.ReverseAdd(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseAdd(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseAdd(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseAdd(value, inPlace: true);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseAdd(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseAdd(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.ReverseAdd(value, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseAdd(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static SByteDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.ReverseAdd(value, inPlace);
        }
        public static ShortDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseAdd(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseAdd(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseAdd(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.ReverseAdd(value, inPlace);
        }
        public static UIntDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseAdd(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseAdd(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseAdd(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseAdd(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseAdd(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseAdd(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseAdd(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseAdd(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseAdd(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseAdd(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseAdd(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseAdd(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.ReverseAdd(value, inPlace);
        }
        public static ByteDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Subtract(column, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(column, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(column, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(column, inPlace: true);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(otherdecimalColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(otherdoubleColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(otherfloatColumn, inPlace);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(otherintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Subtract(column, inPlace);
        }
        public static LongDataFrameColumn Subtract(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(otherintColumn, inPlace);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(otherintColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(otherintColumn, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static SByteDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Subtract(column, inPlace);
        }
        public static ShortDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(column, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(column, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(column, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(othershortColumn, inPlace);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Subtract(column, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(column, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Subtract(column, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(otheruintColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(otherulongColumn, inPlace);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(otherushortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(column, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(column, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(column, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(column, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(column, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(column, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Subtract(column, inPlace);
        }
        public static ByteDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Subtract(value, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(value, inPlace: true);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Subtract(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Subtract(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Subtract(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Subtract(value, inPlace);
        }
        public static LongDataFrameColumn Subtract(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Subtract(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Subtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Subtract(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static SByteDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Subtract(value, inPlace);
        }
        public static ShortDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Subtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Subtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Subtract(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Subtract(value, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Subtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Subtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Subtract(value, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn Subtract(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace);
        }
        public static ULongDataFrameColumn Subtract(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Subtract(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Subtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Subtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Subtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Subtract(value, inPlace: true);
        }
        public static IntDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Subtract(value, inPlace: true);
        }
        public static LongDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Subtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Subtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Subtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Subtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn Subtract(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Subtract(value, inPlace);
        }
        public static ByteDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.ReverseSubtract(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseSubtract(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseSubtract(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.ReverseSubtract(value, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseSubtract(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static SByteDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.ReverseSubtract(value, inPlace);
        }
        public static ShortDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseSubtract(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.ReverseSubtract(value, inPlace);
        }
        public static UIntDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseSubtract(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseSubtract(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseSubtract(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseSubtract(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseSubtract(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseSubtract(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseSubtract(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseSubtract(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseSubtract(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.ReverseSubtract(value, inPlace);
        }
        public static ByteDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Multiply(column, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(column, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(column, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(column, inPlace: true);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(otherdecimalColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(otherdoubleColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(otherfloatColumn, inPlace);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(otherintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Multiply(column, inPlace);
        }
        public static LongDataFrameColumn Multiply(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(otherintColumn, inPlace);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(otherintColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(otherintColumn, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static SByteDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Multiply(column, inPlace);
        }
        public static ShortDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(column, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(column, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(column, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(othershortColumn, inPlace);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Multiply(column, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(column, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Multiply(column, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(otheruintColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(otherulongColumn, inPlace);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(otherushortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(column, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(column, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(column, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(column, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(column, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(column, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Multiply(column, inPlace);
        }
        public static ByteDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Multiply(value, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(value, inPlace: true);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Multiply(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Multiply(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Multiply(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Multiply(value, inPlace);
        }
        public static LongDataFrameColumn Multiply(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Multiply(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Multiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Multiply(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static SByteDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Multiply(value, inPlace);
        }
        public static ShortDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Multiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Multiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Multiply(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Multiply(value, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Multiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Multiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Multiply(value, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn Multiply(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace);
        }
        public static ULongDataFrameColumn Multiply(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Multiply(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Multiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Multiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Multiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Multiply(value, inPlace: true);
        }
        public static IntDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Multiply(value, inPlace: true);
        }
        public static LongDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Multiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Multiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Multiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Multiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn Multiply(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Multiply(value, inPlace);
        }
        public static ByteDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.ReverseMultiply(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseMultiply(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseMultiply(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.ReverseMultiply(value, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseMultiply(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static SByteDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.ReverseMultiply(value, inPlace);
        }
        public static ShortDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseMultiply(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.ReverseMultiply(value, inPlace);
        }
        public static UIntDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseMultiply(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseMultiply(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseMultiply(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseMultiply(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseMultiply(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseMultiply(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseMultiply(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseMultiply(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseMultiply(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.ReverseMultiply(value, inPlace);
        }
        public static ByteDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Divide(column, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(column, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(column, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(column, inPlace: true);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(otherdecimalColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(otherdoubleColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(otherfloatColumn, inPlace);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(otherintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Divide(column, inPlace);
        }
        public static LongDataFrameColumn Divide(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(otherintColumn, inPlace);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(otherintColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(otherintColumn, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static SByteDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Divide(column, inPlace);
        }
        public static ShortDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(column, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(column, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(column, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(othershortColumn, inPlace);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Divide(column, inPlace);
        }
        public static UIntDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(column, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Divide(column, inPlace);
        }
        public static ULongDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(otheruintColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(otherulongColumn, inPlace);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(otherushortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(column, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(column, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(column, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(column, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(column, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(column, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Divide(column, inPlace);
        }
        public static ByteDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Divide(value, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(value, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(value, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(value, inPlace: true);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Divide(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Divide(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Divide(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Divide(value, inPlace);
        }
        public static LongDataFrameColumn Divide(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Divide(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Divide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Divide(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static SByteDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Divide(value, inPlace);
        }
        public static ShortDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Divide(value, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(value, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Divide(value, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Divide(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Divide(value, inPlace);
        }
        public static UIntDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(value, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Divide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Divide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Divide(value, inPlace);
        }
        public static ULongDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static UIntDataFrameColumn Divide(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace);
        }
        public static ULongDataFrameColumn Divide(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Divide(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Divide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Divide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Divide(value, inPlace: true);
        }
        public static FloatDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Divide(value, inPlace: true);
        }
        public static IntDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Divide(value, inPlace: true);
        }
        public static LongDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Divide(value, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Divide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Divide(value, inPlace: true);
        }
        public static ULongDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Divide(value, inPlace: true);
        }
        public static UShortDataFrameColumn Divide(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Divide(value, inPlace);
        }
        public static ByteDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.ReverseDivide(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseDivide(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseDivide(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseDivide(value, inPlace: true);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseDivide(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseDivide(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.ReverseDivide(value, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseDivide(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static SByteDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.ReverseDivide(value, inPlace);
        }
        public static ShortDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseDivide(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseDivide(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseDivide(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.ReverseDivide(value, inPlace);
        }
        public static UIntDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseDivide(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseDivide(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseDivide(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseDivide(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseDivide(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseDivide(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseDivide(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseDivide(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseDivide(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseDivide(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseDivide(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseDivide(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.ReverseDivide(value, inPlace);
        }
        public static ByteDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Modulo(column, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(column, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(column, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(column, inPlace: true);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(otherdecimalColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(otherdoubleColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(otherfloatColumn, inPlace);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(otherintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Modulo(column, inPlace);
        }
        public static LongDataFrameColumn Modulo(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(otherintColumn, inPlace);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(otherintColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(otherintColumn, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, LongDataFrameColumn column, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(otherlongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static SByteDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Modulo(column, inPlace);
        }
        public static ShortDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(column, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(column, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(column, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(othershortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(othershortColumn, inPlace);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Modulo(column, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(column, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(otheruintColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(otheruintColumn, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Modulo(column, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(otheruintColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(otherulongColumn, inPlace);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(otherushortColumn, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(column, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(column, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(column, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(column, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(column, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(otherushortColumn, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(column, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(column, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Modulo(column, inPlace);
        }
        public static ByteDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.Modulo(value, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(value, inPlace: true);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn Modulo(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn Modulo(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.Modulo(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.Modulo(value, inPlace);
        }
        public static LongDataFrameColumn Modulo(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Modulo(convertedValue, inPlace);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.Modulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn Modulo(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static SByteDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.Modulo(value, inPlace);
        }
        public static ShortDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.Modulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.Modulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.Modulo(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.Modulo(value, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Modulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Modulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.Modulo(value, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn Modulo(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace);
        }
        public static ULongDataFrameColumn Modulo(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.Modulo(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Modulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.Modulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.Modulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.Modulo(value, inPlace: true);
        }
        public static IntDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.Modulo(value, inPlace: true);
        }
        public static LongDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.Modulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.Modulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.Modulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.Modulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn Modulo(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.Modulo(value, inPlace);
        }
        public static ByteDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, byte value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)byteColumn.ReverseModulo(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = byteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = byteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = byteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, int value)
        {
            IntDataFrameColumn intColumn = byteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, long value)
        {
            LongDataFrameColumn longColumn = byteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = byteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseModulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = byteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = byteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseModulo(this ByteDataFrameColumn byteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = byteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseModulo(value, inPlace: true);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, decimal value, bool inPlace = false)
        {
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DecimalDataFrameColumn decimalColumn, ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = doubleColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, double value, bool inPlace = false)
        {
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DoubleDataFrameColumn ReverseModulo(this DoubleDataFrameColumn doubleColumn, ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = floatColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = floatColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, float value, bool inPlace = false)
        {
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static FloatDataFrameColumn ReverseModulo(this FloatDataFrameColumn floatColumn, ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = intColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = intColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, float value)
        {
            FloatDataFrameColumn floatColumn = intColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)intColumn.ReverseModulo(value, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, long value)
        {
            LongDataFrameColumn longColumn = intColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static IntDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = intColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this IntDataFrameColumn intColumn, ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return (IntDataFrameColumn)intColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = longColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = longColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, float value)
        {
            FloatDataFrameColumn floatColumn = longColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, long value, bool inPlace = false)
        {
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static LongDataFrameColumn ReverseModulo(this LongDataFrameColumn longColumn, ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return (LongDataFrameColumn)longColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = sbyteColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = sbyteColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, float value)
        {
            FloatDataFrameColumn floatColumn = sbyteColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, int value)
        {
            IntDataFrameColumn intColumn = sbyteColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, long value)
        {
            LongDataFrameColumn longColumn = sbyteColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static SByteDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, sbyte value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)sbyteColumn.ReverseModulo(value, inPlace);
        }
        public static ShortDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, short value)
        {
            ShortDataFrameColumn shortColumn = sbyteColumn.CloneAsShortColumn();
            return (ShortDataFrameColumn)shortColumn.ReverseModulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = sbyteColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = sbyteColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseModulo(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            UShortDataFrameColumn ushortColumn = sbyteColumn.CloneAsUShortColumn();
            return (UShortDataFrameColumn)ushortColumn.ReverseModulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = shortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = shortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = shortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, int value)
        {
            IntDataFrameColumn intColumn = shortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, long value)
        {
            LongDataFrameColumn longColumn = shortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static ShortDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return (ShortDataFrameColumn)shortColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ShortDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, short value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)shortColumn.ReverseModulo(value, inPlace);
        }
        public static UIntDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = shortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseModulo(this ShortDataFrameColumn shortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = shortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = uintColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = uintColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, float value)
        {
            FloatDataFrameColumn floatColumn = uintColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, long value)
        {
            LongDataFrameColumn longColumn = uintColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, uint value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = uintColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static UIntDataFrameColumn ReverseModulo(this UIntDataFrameColumn uintColumn, ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ulongColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ulongColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ulongColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, ulong value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace);
        }
        public static ULongDataFrameColumn ReverseModulo(this ULongDataFrameColumn ulongColumn, ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static UShortDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static DecimalDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            DecimalDataFrameColumn decimalColumn = ushortColumn.CloneAsDecimalColumn();
            return (DecimalDataFrameColumn)decimalColumn.ReverseModulo(value, inPlace: true);
        }
        public static DoubleDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, double value)
        {
            DoubleDataFrameColumn doubleColumn = ushortColumn.CloneAsDoubleColumn();
            return (DoubleDataFrameColumn)doubleColumn.ReverseModulo(value, inPlace: true);
        }
        public static FloatDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, float value)
        {
            FloatDataFrameColumn floatColumn = ushortColumn.CloneAsFloatColumn();
            return (FloatDataFrameColumn)floatColumn.ReverseModulo(value, inPlace: true);
        }
        public static IntDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, int value)
        {
            IntDataFrameColumn intColumn = ushortColumn.CloneAsIntColumn();
            return (IntDataFrameColumn)intColumn.ReverseModulo(value, inPlace: true);
        }
        public static LongDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, long value)
        {
            LongDataFrameColumn longColumn = ushortColumn.CloneAsLongColumn();
            return (LongDataFrameColumn)longColumn.ReverseModulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return (UShortDataFrameColumn)ushortColumn.ReverseModulo(convertedValue, inPlace);
        }
        public static UIntDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, uint value)
        {
            UIntDataFrameColumn uintColumn = ushortColumn.CloneAsUIntColumn();
            return (UIntDataFrameColumn)uintColumn.ReverseModulo(value, inPlace: true);
        }
        public static ULongDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            ULongDataFrameColumn ulongColumn = ushortColumn.CloneAsULongColumn();
            return (ULongDataFrameColumn)ulongColumn.ReverseModulo(value, inPlace: true);
        }
        public static UShortDataFrameColumn ReverseModulo(this UShortDataFrameColumn ushortColumn, ushort value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)ushortColumn.ReverseModulo(value, inPlace);
        }
        public static BoolDataFrameColumn And(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.And(column, inPlace);
        }
        public static BoolDataFrameColumn And(this BoolDataFrameColumn boolColumn, bool value, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.And(value, inPlace);
        }
        public static BoolDataFrameColumn Or(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.Or(column, inPlace);
        }
        public static BoolDataFrameColumn Or(this BoolDataFrameColumn boolColumn, bool value, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.Or(value, inPlace);
        }
        public static BoolDataFrameColumn Xor(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.Xor(column, inPlace);
        }
        public static BoolDataFrameColumn Xor(this BoolDataFrameColumn boolColumn, bool value, bool inPlace = false)
        {
            return (BoolDataFrameColumn)boolColumn.Xor(value, inPlace);
        }
    }

    public static class ComparisonOperations
    {
        public static BoolDataFrameColumn ElementwiseEquals(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseEquals(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(column);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseNotEquals(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseNotEquals(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThanOrEqual(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThanOrEqual(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThanOrEqual(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(column);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseGreaterThan(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseGreaterThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this BoolDataFrameColumn boolColumn, BoolDataFrameColumn column)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, ByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, DecimalDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, DoubleDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, FloatDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, IntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, LongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, SByteDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, ShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, UIntDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, ULongDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, UShortDataFrameColumn column)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(column);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this BoolDataFrameColumn boolColumn, bool value)
        {
            return (BoolDataFrameColumn)boolColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, byte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, decimal value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, double value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, float value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, int value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, long value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, short value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, uint value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, ulong value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ByteDataFrameColumn byteColumn, ushort value)
        {
            return (BoolDataFrameColumn)byteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, byte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, decimal value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, double value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, float value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, int value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, long value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, sbyte value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, short value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, uint value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, ulong value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DecimalDataFrameColumn decimalColumn, ushort value)
        {
            return (BoolDataFrameColumn)decimalColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, byte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, decimal value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, double value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, float value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, int value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, long value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, sbyte value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, short value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, uint value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, ulong value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this DoubleDataFrameColumn doubleColumn, ushort value)
        {
            return (BoolDataFrameColumn)doubleColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, byte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, decimal value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, double value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, float value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, int value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, long value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, sbyte value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, short value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, uint value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, ulong value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this FloatDataFrameColumn floatColumn, ushort value)
        {
            return (BoolDataFrameColumn)floatColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, byte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, decimal value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, double value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, float value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, int value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, long value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, sbyte value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, short value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, uint value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, ulong value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this IntDataFrameColumn intColumn, ushort value)
        {
            return (BoolDataFrameColumn)intColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, byte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, decimal value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, double value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, float value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, int value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, long value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, sbyte value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, short value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, uint value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, ulong value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this LongDataFrameColumn longColumn, ushort value)
        {
            return (BoolDataFrameColumn)longColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, byte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, decimal value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, double value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, float value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, int value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, long value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, sbyte value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, short value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, uint value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, ulong value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this SByteDataFrameColumn sbyteColumn, ushort value)
        {
            return (BoolDataFrameColumn)sbyteColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, byte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, decimal value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, double value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, float value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, int value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, long value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, short value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, uint value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, ulong value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ShortDataFrameColumn shortColumn, ushort value)
        {
            return (BoolDataFrameColumn)shortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, byte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, decimal value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, double value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, float value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, int value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, long value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, sbyte value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, short value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, uint value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, ulong value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UIntDataFrameColumn uintColumn, ushort value)
        {
            return (BoolDataFrameColumn)uintColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, byte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, decimal value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, double value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, float value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, int value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, long value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, short value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, uint value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, ulong value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this ULongDataFrameColumn ulongColumn, ushort value)
        {
            return (BoolDataFrameColumn)ulongColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, byte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, decimal value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, double value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, float value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, int value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, long value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, sbyte value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, short value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, uint value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, ulong value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
        public static BoolDataFrameColumn ElementwiseLessThan(this UShortDataFrameColumn ushortColumn, ushort value)
        {
            return (BoolDataFrameColumn)ushortColumn.ElementwiseLessThan(value);
        }
    }

    public static class BinaryShiftOperations
    {
        public static ByteDataFrameColumn LeftShift(this ByteDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static CharDataFrameColumn LeftShift(this CharDataFrameColumn column, int value, bool inPlace = false)
        {
            return (CharDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static IntDataFrameColumn LeftShift(this IntDataFrameColumn column, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static LongDataFrameColumn LeftShift(this LongDataFrameColumn column, int value, bool inPlace = false)
        {
            return (LongDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static SByteDataFrameColumn LeftShift(this SByteDataFrameColumn column, int value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static ShortDataFrameColumn LeftShift(this ShortDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static UIntDataFrameColumn LeftShift(this UIntDataFrameColumn column, int value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static ULongDataFrameColumn LeftShift(this ULongDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static UShortDataFrameColumn LeftShift(this UShortDataFrameColumn column, int value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)column.LeftShift(value, inPlace);
        }
        public static ByteDataFrameColumn RightShift(this ByteDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ByteDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static CharDataFrameColumn RightShift(this CharDataFrameColumn column, int value, bool inPlace = false)
        {
            return (CharDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static IntDataFrameColumn RightShift(this IntDataFrameColumn column, int value, bool inPlace = false)
        {
            return (IntDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static LongDataFrameColumn RightShift(this LongDataFrameColumn column, int value, bool inPlace = false)
        {
            return (LongDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static SByteDataFrameColumn RightShift(this SByteDataFrameColumn column, int value, bool inPlace = false)
        {
            return (SByteDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static ShortDataFrameColumn RightShift(this ShortDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ShortDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static UIntDataFrameColumn RightShift(this UIntDataFrameColumn column, int value, bool inPlace = false)
        {
            return (UIntDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static ULongDataFrameColumn RightShift(this ULongDataFrameColumn column, int value, bool inPlace = false)
        {
            return (ULongDataFrameColumn)column.RightShift(value, inPlace);
        }
        public static UShortDataFrameColumn RightShift(this UShortDataFrameColumn column, int value, bool inPlace = false)
        {
            return (UShortDataFrameColumn)column.RightShift(value, inPlace);
        }
    }
}
