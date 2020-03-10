﻿

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
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Add(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Add(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Add(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
        public DecimalDataFrameColumn Add(SingleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(Int64DataFrameColumn column, bool inPlace = false)
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
        public DecimalDataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return AddImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Add(SingleDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(Int64DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return AddImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(Int64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(UInt64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return AddImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return AddImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return AddImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
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
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Add(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Add(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Add(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return AddImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Add(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return AddImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return AddImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(Int32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column, bool inPlace = false)
        {
            return AddImplementation(column, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return AddImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return AddImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Add(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Add(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Add(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Add(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Add(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return AddImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Add(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Add(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(UInt16DataFrameColumn column, bool inPlace = false)
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
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Add(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Add(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Add(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Add(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(float value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Add(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(int value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Add(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(long value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Add(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Add(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Add(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Add(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Add(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Add(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Add(short value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Add(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(uint value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Add(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Add(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Add(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Add(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Add(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Add(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return AddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Add(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Add(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.AddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Add(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn ReverseAdd(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseAdd(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn ReverseAdd(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseAdd(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value, bool inPlace = false)
        {
            return ReverseAddImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseAdd(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseAdd(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseAdd(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseAdd(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseAdd(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseAdd(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseAdd(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseAddImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseAdd(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseAdd(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseAddImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseAdd(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Subtract(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
        public DecimalDataFrameColumn Subtract(SingleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(Int64DataFrameColumn column, bool inPlace = false)
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
        public DecimalDataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return SubtractImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Subtract(SingleDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(Int64DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return SubtractImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(Int64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(UInt64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return SubtractImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return SubtractImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return SubtractImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
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
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Subtract(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return SubtractImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return SubtractImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return SubtractImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(Int32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column, bool inPlace = false)
        {
            return SubtractImplementation(column, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return SubtractImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return SubtractImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return SubtractImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(UInt16DataFrameColumn column, bool inPlace = false)
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
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Subtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Subtract(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(int value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Subtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Subtract(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Subtract(short value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Subtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Subtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Subtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Subtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Subtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return SubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Subtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Subtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.SubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Subtract(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn ReverseSubtract(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseSubtract(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn ReverseSubtract(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseSubtract(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value, bool inPlace = false)
        {
            return ReverseSubtractImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseSubtract(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseSubtract(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseSubtract(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseSubtract(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseSubtract(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseSubtract(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseSubtract(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseSubtractImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseSubtract(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseSubtract(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseSubtractImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseSubtract(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Multiply(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
        public DecimalDataFrameColumn Multiply(SingleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(Int64DataFrameColumn column, bool inPlace = false)
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
        public DecimalDataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return MultiplyImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Multiply(SingleDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(Int64DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return MultiplyImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(Int64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(UInt64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return MultiplyImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return MultiplyImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return MultiplyImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
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
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Multiply(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return MultiplyImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return MultiplyImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return MultiplyImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(Int32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column, bool inPlace = false)
        {
            return MultiplyImplementation(column, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return MultiplyImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return MultiplyImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return MultiplyImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(UInt16DataFrameColumn column, bool inPlace = false)
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
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Multiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Multiply(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(int value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Multiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Multiply(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Multiply(short value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Multiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Multiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Multiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Multiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Multiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return MultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Multiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Multiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.MultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Multiply(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn ReverseMultiply(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseMultiply(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn ReverseMultiply(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseMultiply(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value, bool inPlace = false)
        {
            return ReverseMultiplyImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseMultiply(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseMultiply(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseMultiply(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseMultiply(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseMultiply(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseMultiply(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseMultiply(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseMultiplyImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseMultiply(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseMultiply(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseMultiplyImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseMultiply(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Divide(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Divide(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
        public DecimalDataFrameColumn Divide(SingleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(Int64DataFrameColumn column, bool inPlace = false)
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
        public DecimalDataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return DivideImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Divide(SingleDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(Int64DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return DivideImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(Int64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(UInt64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return DivideImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return DivideImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return DivideImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
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
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Divide(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Divide(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return DivideImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return DivideImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return DivideImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(Int32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column, bool inPlace = false)
        {
            return DivideImplementation(column, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return DivideImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return DivideImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Divide(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Divide(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Divide(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return DivideImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(UInt16DataFrameColumn column, bool inPlace = false)
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
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Divide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Divide(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Divide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Divide(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(int value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Divide(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Divide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Divide(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Divide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Divide(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Divide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Divide(short value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(uint value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Divide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Divide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Divide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Divide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Divide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return DivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Divide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Divide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.DivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Divide(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn ReverseDivide(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseDivide(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn ReverseDivide(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseDivide(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value, bool inPlace = false)
        {
            return ReverseDivideImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseDivide(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseDivide(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseDivide(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseDivide(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseDivide(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseDivide(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseDivide(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseDivideImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseDivide(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseDivide(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseDivideImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseDivide(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Modulo(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
        public DecimalDataFrameColumn Modulo(SingleDataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(Int64DataFrameColumn column, bool inPlace = false)
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
        public DecimalDataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ModuloImplementation(otherdecimalColumn, inPlace);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Modulo(SingleDataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(Int64DataFrameColumn column, bool inPlace = false)
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
        public DoubleDataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(UInt64DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ModuloImplementation(otherdoubleColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(Int64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(UInt64DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ModuloImplementation(otherfloatColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ModuloImplementation(otherintColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ModuloImplementation(otherlongColumn, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
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
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Modulo(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ModuloImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ModuloImplementation(othershortColumn, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ModuloImplementation(otheruintColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(Int32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(Int16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt32DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column, bool inPlace = false)
        {
            return ModuloImplementation(column, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ModuloImplementation(otherulongColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(ByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ModuloImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(SByteDataFrameColumn column, bool inPlace = false)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ModuloImplementation(otherushortColumn, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(column, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(UInt16DataFrameColumn column, bool inPlace = false)
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
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn Modulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn Modulo(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(int value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn Modulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn Modulo(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn Modulo(short value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn Modulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn Modulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn Modulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn Modulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn Modulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn Modulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn Modulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn Modulo(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public Int16DataFrameColumn ReverseModulo(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseModulo(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
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
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            float convertedValue = (float)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            int convertedValue = (int)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            long convertedValue = (long)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
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
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
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
        public Int16DataFrameColumn ReverseModulo(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public UInt16DataFrameColumn ReverseModulo(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            short convertedValue = (short)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public Int16DataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            uint convertedValue = (uint)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(int value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(short value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(uint value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            ulong convertedValue = (ulong)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseModulo(byte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DecimalDataFrameColumn ReverseModulo(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public DoubleDataFrameColumn ReverseModulo(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public SingleDataFrameColumn ReverseModulo(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int32DataFrameColumn ReverseModulo(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public Int64DataFrameColumn ReverseModulo(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseModulo(sbyte value, bool inPlace = false)
        {
            ushort convertedValue = (ushort)value;
            return ReverseModuloImplementation(convertedValue, inPlace);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt32DataFrameColumn ReverseModulo(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt64DataFrameColumn ReverseModulo(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ReverseModuloImplementation(value, inPlace: true);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public UInt16DataFrameColumn ReverseModulo(ushort value, bool inPlace = false)
        {
            return ReverseModuloImplementation(value, inPlace);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn And(BooleanDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.And(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public new BooleanDataFrameColumn And(bool value, bool inPlace = false)
        {
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.And(value);
            return retColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn Or(BooleanDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.Or(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public new BooleanDataFrameColumn Or(bool value, bool inPlace = false)
        {
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.Or(value);
            return retColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn Xor(BooleanDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.Xor(column._columnContainer);
            return retColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public new BooleanDataFrameColumn Xor(bool value, bool inPlace = false)
        {
            BooleanDataFrameColumn retColumn = inPlace ? this : CloneAsBooleanColumn();
            retColumn._columnContainer.Xor(value);
            return retColumn;
        }
    }

    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(BooleanDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseEqualsImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseEqualsImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseEqualsImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseEqualsImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(UInt16DataFrameColumn column)
        {
            return ElementwiseEqualsImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(bool value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseEqualsImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseEqualsImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseEqualsImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseEqualsImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseEqualsImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseEqualsImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEquals(ushort value)
        {
            return ElementwiseEqualsImplementation(value);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(BooleanDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseNotEqualsImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseNotEqualsImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseNotEqualsImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseNotEqualsImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseNotEqualsImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseNotEqualsImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseNotEqualsImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseNotEqualsImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseNotEqualsImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseNotEqualsImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseNotEqualsImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(UInt16DataFrameColumn column)
        {
            return ElementwiseNotEqualsImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(bool value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseNotEqualsImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseNotEqualsImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseNotEqualsImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseNotEqualsImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseNotEqualsImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseNotEqualsImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseNotEqualsImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseNotEqualsImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseNotEqualsImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseNotEqualsImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseNotEqualsImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEquals(ushort value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(BooleanDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseGreaterThanOrEqualImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(UInt16DataFrameColumn column)
        {
            return ElementwiseGreaterThanOrEqualImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(bool value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseGreaterThanOrEqualImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqual(ushort value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(BooleanDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseLessThanOrEqualImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanOrEqualImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseLessThanOrEqualImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanOrEqualImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanOrEqualImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanOrEqualImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanOrEqualImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(UInt16DataFrameColumn column)
        {
            return ElementwiseLessThanOrEqualImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(bool value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseLessThanOrEqualImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanOrEqualImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanOrEqualImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanOrEqualImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanOrEqualImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanOrEqualImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseLessThanOrEqualImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanOrEqualImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanOrEqualImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanOrEqualImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanOrEqualImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqual(ushort value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(BooleanDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseGreaterThanImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseGreaterThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseGreaterThanImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseGreaterThanImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseGreaterThanImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseGreaterThanImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseGreaterThanImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseGreaterThanImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseGreaterThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseGreaterThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseGreaterThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(UInt16DataFrameColumn column)
        {
            return ElementwiseGreaterThanImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(bool value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseGreaterThanImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseGreaterThanImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseGreaterThanImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseGreaterThanImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseGreaterThanImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseGreaterThanImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseGreaterThanImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseGreaterThanImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseGreaterThanImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseGreaterThanImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseGreaterThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThan(ushort value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(BooleanDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            ByteDataFrameColumn otherbyteColumn = column.CloneAsByteColumn();
            return ElementwiseLessThanImplementation(otherbyteColumn);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            DecimalDataFrameColumn otherdecimalColumn = column.CloneAsDecimalColumn();
            return ElementwiseLessThanImplementation(otherdecimalColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            DoubleDataFrameColumn otherdoubleColumn = column.CloneAsDoubleColumn();
            return ElementwiseLessThanImplementation(otherdoubleColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            SingleDataFrameColumn otherfloatColumn = column.CloneAsSingleColumn();
            return ElementwiseLessThanImplementation(otherfloatColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            Int32DataFrameColumn otherintColumn = column.CloneAsInt32Column();
            return ElementwiseLessThanImplementation(otherintColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            Int64DataFrameColumn otherlongColumn = column.CloneAsInt64Column();
            return ElementwiseLessThanImplementation(otherlongColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            SByteDataFrameColumn othersbyteColumn = column.CloneAsSByteColumn();
            return ElementwiseLessThanImplementation(othersbyteColumn);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            Int16DataFrameColumn othershortColumn = column.CloneAsInt16Column();
            return ElementwiseLessThanImplementation(othershortColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            UInt32DataFrameColumn otheruintColumn = column.CloneAsUInt32Column();
            return ElementwiseLessThanImplementation(otheruintColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            UInt64DataFrameColumn otherulongColumn = column.CloneAsUInt64Column();
            return ElementwiseLessThanImplementation(otherulongColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DecimalDataFrameColumn column)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(DoubleDataFrameColumn column)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SingleDataFrameColumn column)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int32DataFrameColumn column)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int64DataFrameColumn column)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(SByteDataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(Int16DataFrameColumn column)
        {
            UInt16DataFrameColumn otherushortColumn = column.CloneAsUInt16Column();
            return ElementwiseLessThanImplementation(otherushortColumn);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt32DataFrameColumn column)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt64DataFrameColumn column)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(column);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(UInt16DataFrameColumn column)
        {
            return ElementwiseLessThanImplementation(column);
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(bool value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            byte otherbyteValue = (byte)value;
            return ElementwiseLessThanImplementation(otherbyteValue);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            decimal otherdecimalValue = (decimal)value;
            return ElementwiseLessThanImplementation(otherdecimalValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            double otherdoubleValue = (double)value;
            return ElementwiseLessThanImplementation(otherdoubleValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            float otherfloatValue = (float)value;
            return ElementwiseLessThanImplementation(otherfloatValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            int otherintValue = (int)value;
            return ElementwiseLessThanImplementation(otherintValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            long otherlongValue = (long)value;
            return ElementwiseLessThanImplementation(otherlongValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            sbyte othersbyteValue = (sbyte)value;
            return ElementwiseLessThanImplementation(othersbyteValue);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            Int16DataFrameColumn shortColumn = CloneAsInt16Column();
            return shortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            UInt16DataFrameColumn ushortColumn = CloneAsUInt16Column();
            return ushortColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            short othershortValue = (short)value;
            return ElementwiseLessThanImplementation(othershortValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            uint otheruintValue = (uint)value;
            return ElementwiseLessThanImplementation(otheruintValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            ulong otherulongValue = (ulong)value;
            return ElementwiseLessThanImplementation(otherulongValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(byte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(decimal value)
        {
            DecimalDataFrameColumn decimalColumn = CloneAsDecimalColumn();
            return decimalColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(double value)
        {
            DoubleDataFrameColumn doubleColumn = CloneAsDoubleColumn();
            return doubleColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(float value)
        {
            SingleDataFrameColumn floatColumn = CloneAsSingleColumn();
            return floatColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(int value)
        {
            Int32DataFrameColumn intColumn = CloneAsInt32Column();
            return intColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(long value)
        {
            Int64DataFrameColumn longColumn = CloneAsInt64Column();
            return longColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(sbyte value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(short value)
        {
            ushort otherushortValue = (ushort)value;
            return ElementwiseLessThanImplementation(otherushortValue);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(uint value)
        {
            UInt32DataFrameColumn uintColumn = CloneAsUInt32Column();
            return uintColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ulong value)
        {
            UInt64DataFrameColumn ulongColumn = CloneAsUInt64Column();
            return ulongColumn.ElementwiseLessThanImplementation(value);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThan(ushort value)
        {
            return ElementwiseLessThanImplementation(value);
        }
    }

    public partial class ByteDataFrameColumn
    {
        public new ByteDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<byte>)base.LeftShift(value, inPlace);
            return new ByteDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class CharDataFrameColumn
    {
        public new CharDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<char>)base.LeftShift(value, inPlace);
            return new CharDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public new Int32DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<int>)base.LeftShift(value, inPlace);
            return new Int32DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public new Int64DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<long>)base.LeftShift(value, inPlace);
            return new Int64DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public new SByteDataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<sbyte>)base.LeftShift(value, inPlace);
            return new SByteDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public new Int16DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<short>)base.LeftShift(value, inPlace);
            return new Int16DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public new UInt32DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<uint>)base.LeftShift(value, inPlace);
            return new UInt32DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public new UInt64DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ulong>)base.LeftShift(value, inPlace);
            return new UInt64DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public new UInt16DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ushort>)base.LeftShift(value, inPlace);
            return new UInt16DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class ByteDataFrameColumn
    {
        public new ByteDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<byte>)base.RightShift(value, inPlace);
            return new ByteDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class CharDataFrameColumn
    {
        public new CharDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<char>)base.RightShift(value, inPlace);
            return new CharDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int32DataFrameColumn
    {
        public new Int32DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<int>)base.RightShift(value, inPlace);
            return new Int32DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int64DataFrameColumn
    {
        public new Int64DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<long>)base.RightShift(value, inPlace);
            return new Int64DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class SByteDataFrameColumn
    {
        public new SByteDataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<sbyte>)base.RightShift(value, inPlace);
            return new SByteDataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class Int16DataFrameColumn
    {
        public new Int16DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<short>)base.RightShift(value, inPlace);
            return new Int16DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public new UInt32DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<uint>)base.RightShift(value, inPlace);
            return new UInt32DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public new UInt64DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ulong>)base.RightShift(value, inPlace);
            return new UInt64DataFrameColumn(result.Name, result._columnContainer);
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public new UInt16DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            var result = (PrimitiveDataFrameColumn<ushort>)base.RightShift(value, inPlace);
            return new UInt16DataFrameColumn(result.Name, result._columnContainer);
        }
    }
}
