

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameColumn.BinaryOperationAPIs.ExplodedColumns.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Add(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Add(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Add(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Add(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Add(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Add(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Add(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Add(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return AddImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Add(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return AddImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Add(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return AddImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Add(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Add(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Add(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return AddImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Add(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Add(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(UShortDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Add(byte value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Add(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Add(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Add(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Add(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(int value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(long value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Add(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Add(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Add(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Add(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Add(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Add(short value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Add(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(uint value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Add(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Add(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Add(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Add(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Add(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseAdd(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseAdd(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseAdd(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseAdd(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseAdd(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseAdd(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseAdd(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseAdd(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseAdd(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Subtract(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Subtract(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return SubtractImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Subtract(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return SubtractImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return SubtractImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Subtract(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Subtract(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return SubtractImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(UShortDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Subtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Subtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Subtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Subtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Subtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Subtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Subtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Subtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseSubtract(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseSubtract(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseSubtract(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseSubtract(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseSubtract(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseSubtract(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseSubtract(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseSubtract(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseSubtract(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Multiply(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Multiply(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return MultiplyImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Multiply(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return MultiplyImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return MultiplyImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Multiply(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Multiply(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return MultiplyImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(UShortDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Multiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Multiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Multiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Multiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Multiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Multiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Multiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Multiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseMultiply(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseMultiply(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseMultiply(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseMultiply(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseMultiply(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseMultiply(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseMultiply(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseMultiply(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseMultiply(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Divide(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Divide(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Divide(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Divide(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Divide(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Divide(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return DivideImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Divide(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return DivideImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return DivideImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Divide(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Divide(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Divide(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return DivideImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Divide(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(UShortDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Divide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Divide(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Divide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Divide(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(int value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Divide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Divide(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Divide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Divide(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Divide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Divide(short value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Divide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Divide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Divide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Divide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Divide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Divide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseDivide(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseDivide(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseDivide(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseDivide(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseDivide(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseDivide(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseDivide(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseDivide(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseDivide(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Modulo(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DoubleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(FloatDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(LongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(ULongDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(FloatDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(LongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(ULongDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(LongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(ULongDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Modulo(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ModuloImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Modulo(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ModuloImplementation(othershortColumn, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(IntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(UIntDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ModuloImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Modulo(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Modulo(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ModuloImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(UShortDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn Modulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn Modulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn Modulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn Modulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn Modulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn Modulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn Modulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn Modulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ByteDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseModulo(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseModulo(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(double value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(float value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            decimal convertedValue = (decimal)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(float value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            double convertedValue = (double)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class IntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class IntDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class LongDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public SByteDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ShortDataFrameColumn ReverseModulo(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UShortDataFrameColumn ReverseModulo(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ShortDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public FloatDataFrameColumn ReverseModulo(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public IntDataFrameColumn ReverseModulo(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public LongDataFrameColumn ReverseModulo(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UIntDataFrameColumn ReverseModulo(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public ULongDataFrameColumn ReverseModulo(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public UShortDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn And(BoolDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.And(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public new BoolDataFrameColumn And(bool value, bool inPlace = false)
        {
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.And(value);
            return retColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn Or(BoolDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.Or(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public new BoolDataFrameColumn Or(bool value, bool inPlace = false)
        {
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.Or(value);
            return retColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn Xor(BoolDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.Xor(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public new BoolDataFrameColumn Xor(bool value, bool inPlace = false)
        {
            BoolDataFrameColumn retColumn = inPlace ? this : CloneAsBoolColumn();
            retColumn._columnContainer.Xor(value);
            return retColumn;
        }
    }

    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(BoolDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseEqualsImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseEqualsImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(UShortDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(bool value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseEqualsImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseEqualsImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEquals(ushort value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(BoolDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseNotEqualsImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseNotEqualsImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(UShortDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(bool value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseNotEqualsImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseNotEqualsImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(BoolDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseGreaterThanOrEqualImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(UShortDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(bool value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseGreaterThanOrEqualImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(BoolDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseLessThanOrEqualImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseLessThanOrEqualImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(UShortDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(bool value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseLessThanOrEqualImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseLessThanOrEqualImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(BoolDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseGreaterThanImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseGreaterThanImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(UShortDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(bool value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseGreaterThanImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseGreaterThanImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(BoolDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseLessThanImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            FloatDataFrameColumn otherfloatColumn = column.CloneAsFloatColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            IntDataFrameColumn otherintColumn = column.CloneAsIntColumn();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            LongDataFrameColumn otherlongColumn = column.CloneAsLongColumn();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseLessThanImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            ShortDataFrameColumn othershortColumn = column.CloneAsShortColumn();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            UIntDataFrameColumn otheruintColumn = column.CloneAsUIntColumn();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            ULongDataFrameColumn otherulongColumn = column.CloneAsULongColumn();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(FloatDataFrameColumn column)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(IntDataFrameColumn column)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(LongDataFrameColumn column)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ShortDataFrameColumn column)
        {
            UShortDataFrameColumn otherushortColumn = column.CloneAsUShortColumn();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UIntDataFrameColumn column)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ULongDataFrameColumn column)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(UShortDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(bool value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseLessThanImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseLessThanImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            ShortDataFrameColumn shortColumn = CloneAsShortColumn();
            return shortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            UShortDataFrameColumn ushortColumn = CloneAsUShortColumn();
            return ushortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(float value)
        {
            FloatDataFrameColumn floatColumn = CloneAsFloatColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(int value)
        {
            IntDataFrameColumn intColumn = CloneAsIntColumn();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(long value)
        {
            LongDataFrameColumn longColumn = CloneAsLongColumn();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(uint value)
        {
            UIntDataFrameColumn uintColumn = CloneAsUIntColumn();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ulong value)
        {
            ULongDataFrameColumn ulongColumn = CloneAsULongColumn();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThan(ushort value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }

    public partial class ByteDataFrameColumn
    {
        public new ByteDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<byte>)base.LeftShift(value, inPlace);
            return new ByteDataFrameColumn(result);
        }
    }
    public partial class CharDataFrameColumn
    {
        public new CharDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<char>)base.LeftShift(value, inPlace);
            return new CharDataFrameColumn(result);
        }
    }
    public partial class IntDataFrameColumn
    {
        public new IntDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<int>)base.LeftShift(value, inPlace);
            return new IntDataFrameColumn(result);
        }
    }
    public partial class LongDataFrameColumn
    {
        public new LongDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<long>)base.LeftShift(value, inPlace);
            return new LongDataFrameColumn(result);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public new SByteDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<sbyte>)base.LeftShift(value, inPlace);
            return new SByteDataFrameColumn(result);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public new ShortDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<short>)base.LeftShift(value, inPlace);
            return new ShortDataFrameColumn(result);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public new UIntDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<uint>)base.LeftShift(value, inPlace);
            return new UIntDataFrameColumn(result);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public new ULongDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ulong>)base.LeftShift(value, inPlace);
            return new ULongDataFrameColumn(result);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public new UShortDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ushort>)base.LeftShift(value, inPlace);
            return new UShortDataFrameColumn(result);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public new ByteDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<byte>)base.RightShift(value, inPlace);
            return new ByteDataFrameColumn(result);
        }
    }
    public partial class CharDataFrameColumn
    {
        public new CharDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<char>)base.RightShift(value, inPlace);
            return new CharDataFrameColumn(result);
        }
    }
    public partial class IntDataFrameColumn
    {
        public new IntDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<int>)base.RightShift(value, inPlace);
            return new IntDataFrameColumn(result);
        }
    }
    public partial class LongDataFrameColumn
    {
        public new LongDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<long>)base.RightShift(value, inPlace);
            return new LongDataFrameColumn(result);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public new SByteDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<sbyte>)base.RightShift(value, inPlace);
            return new SByteDataFrameColumn(result);
        }
    }
    public partial class ShortDataFrameColumn
    {
        public new ShortDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<short>)base.RightShift(value, inPlace);
            return new ShortDataFrameColumn(result);
        }
    }
    public partial class UIntDataFrameColumn
    {
        public new UIntDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<uint>)base.RightShift(value, inPlace);
            return new UIntDataFrameColumn(result);
        }
    }
    public partial class ULongDataFrameColumn
    {
        public new ULongDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ulong>)base.RightShift(value, inPlace);
            return new ULongDataFrameColumn(result);
        }
    }
    public partial class UShortDataFrameColumn
    {
        public new UShortDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ushort>)base.RightShift(value, inPlace);
            return new UShortDataFrameColumn(result);
        }
    }
}
