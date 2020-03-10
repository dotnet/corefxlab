

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameColumn.BinaryOperationImplementations.ExplodedColumns.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn AddImplementation(ByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn AddImplementation(DecimalDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn AddImplementation(DoubleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn AddImplementation(SingleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn AddImplementation(Int32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn AddImplementation(Int64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn AddImplementation(SByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn AddImplementation(Int16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn AddImplementation(UInt32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn AddImplementation(UInt64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn AddImplementation(UInt16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn AddImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn AddImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn AddImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn AddImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn AddImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn AddImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn AddImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn AddImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn AddImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn AddImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn AddImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ReverseAddImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ReverseAddImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ReverseAddImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ReverseAddImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ReverseAddImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ReverseAddImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ReverseAddImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ReverseAddImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ReverseAddImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ReverseAddImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ReverseAddImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn SubtractImplementation(ByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn SubtractImplementation(DecimalDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn SubtractImplementation(DoubleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn SubtractImplementation(SingleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn SubtractImplementation(Int32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn SubtractImplementation(Int64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn SubtractImplementation(SByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn SubtractImplementation(Int16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn SubtractImplementation(UInt32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn SubtractImplementation(UInt64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn SubtractImplementation(UInt16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn SubtractImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn SubtractImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn SubtractImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn SubtractImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn SubtractImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn SubtractImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn SubtractImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn SubtractImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn SubtractImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn SubtractImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn SubtractImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ReverseSubtractImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ReverseSubtractImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ReverseSubtractImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ReverseSubtractImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ReverseSubtractImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ReverseSubtractImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ReverseSubtractImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ReverseSubtractImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ReverseSubtractImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ReverseSubtractImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ReverseSubtractImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn MultiplyImplementation(ByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn MultiplyImplementation(DecimalDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn MultiplyImplementation(DoubleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn MultiplyImplementation(SingleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn MultiplyImplementation(Int32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn MultiplyImplementation(Int64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn MultiplyImplementation(SByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn MultiplyImplementation(Int16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn MultiplyImplementation(UInt32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn MultiplyImplementation(UInt64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn MultiplyImplementation(UInt16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn MultiplyImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn MultiplyImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn MultiplyImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn MultiplyImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn MultiplyImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn MultiplyImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn MultiplyImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn MultiplyImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn MultiplyImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn MultiplyImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn MultiplyImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ReverseMultiplyImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ReverseMultiplyImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ReverseMultiplyImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ReverseMultiplyImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ReverseMultiplyImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ReverseMultiplyImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ReverseMultiplyImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ReverseMultiplyImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ReverseMultiplyImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ReverseMultiplyImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ReverseMultiplyImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn DivideImplementation(ByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn DivideImplementation(DecimalDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn DivideImplementation(DoubleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn DivideImplementation(SingleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn DivideImplementation(Int32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn DivideImplementation(Int64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn DivideImplementation(SByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn DivideImplementation(Int16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn DivideImplementation(UInt32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn DivideImplementation(UInt64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn DivideImplementation(UInt16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn DivideImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn DivideImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn DivideImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn DivideImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn DivideImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn DivideImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn DivideImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn DivideImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn DivideImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn DivideImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn DivideImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ReverseDivideImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ReverseDivideImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ReverseDivideImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ReverseDivideImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ReverseDivideImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ReverseDivideImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ReverseDivideImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ReverseDivideImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ReverseDivideImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ReverseDivideImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ReverseDivideImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ModuloImplementation(ByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ModuloImplementation(DecimalDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ModuloImplementation(DoubleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ModuloImplementation(SingleDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ModuloImplementation(Int32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ModuloImplementation(Int64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ModuloImplementation(SByteDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ModuloImplementation(Int16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ModuloImplementation(UInt32DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ModuloImplementation(UInt64DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ModuloImplementation(UInt16DataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ModuloImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ModuloImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ModuloImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ModuloImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ModuloImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ModuloImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ModuloImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ModuloImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ModuloImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ModuloImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ModuloImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        internal ByteDataFrameColumn ReverseModuloImplementation(byte value, bool inPlace = false)
        {
            ByteDataFrameColumn newColumn = inPlace ? this : CloneAsByteColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        internal DecimalDataFrameColumn ReverseModuloImplementation(decimal value, bool inPlace = false)
        {
            DecimalDataFrameColumn newColumn = inPlace ? this : CloneAsDecimalColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        internal DoubleDataFrameColumn ReverseModuloImplementation(double value, bool inPlace = false)
        {
            DoubleDataFrameColumn newColumn = inPlace ? this : CloneAsDoubleColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        internal SingleDataFrameColumn ReverseModuloImplementation(float value, bool inPlace = false)
        {
            SingleDataFrameColumn newColumn = inPlace ? this : CloneAsSingleColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        internal Int32DataFrameColumn ReverseModuloImplementation(int value, bool inPlace = false)
        {
            Int32DataFrameColumn newColumn = inPlace ? this : CloneAsInt32Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        internal Int64DataFrameColumn ReverseModuloImplementation(long value, bool inPlace = false)
        {
            Int64DataFrameColumn newColumn = inPlace ? this : CloneAsInt64Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        internal SByteDataFrameColumn ReverseModuloImplementation(sbyte value, bool inPlace = false)
        {
            SByteDataFrameColumn newColumn = inPlace ? this : CloneAsSByteColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        internal Int16DataFrameColumn ReverseModuloImplementation(short value, bool inPlace = false)
        {
            Int16DataFrameColumn newColumn = inPlace ? this : CloneAsInt16Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        internal UInt32DataFrameColumn ReverseModuloImplementation(uint value, bool inPlace = false)
        {
            UInt32DataFrameColumn newColumn = inPlace ? this : CloneAsUInt32Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        internal UInt64DataFrameColumn ReverseModuloImplementation(ulong value, bool inPlace = false)
        {
            UInt64DataFrameColumn newColumn = inPlace ? this : CloneAsUInt64Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        internal UInt16DataFrameColumn ReverseModuloImplementation(ushort value, bool inPlace = false)
        {
            UInt16DataFrameColumn newColumn = inPlace ? this : CloneAsUInt16Column();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseEqualsImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseNotEqualsImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanOrEqualImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseGreaterThanImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(BooleanDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(ByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(DecimalDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(DoubleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(SingleDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(Int32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(Int64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(SByteDataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(Int16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(UInt32DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(UInt64DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(UInt16DataFrameColumn column)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BooleanDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(bool value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(byte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(decimal value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(double value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SingleDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(float value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(int value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(long value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(sbyte value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class Int16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(short value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt32DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(uint value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt64DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(ulong value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UInt16DataFrameColumn
    {
        public BooleanDataFrameColumn ElementwiseLessThanImplementation(ushort value)
        {
            BooleanDataFrameColumn newColumn = CloneAsBooleanColumn();
            _columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
            return newColumn;
        }
    }
}
