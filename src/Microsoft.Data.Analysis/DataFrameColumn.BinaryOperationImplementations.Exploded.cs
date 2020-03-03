

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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn AddImplementation(FloatDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn AddImplementation(IntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn AddImplementation(LongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn AddImplementation(ShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn AddImplementation(UIntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn AddImplementation(ULongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Add(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn AddImplementation(UShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn AddImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn AddImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn AddImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn AddImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn AddImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn AddImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Add(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn AddImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ReverseAddImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ReverseAddImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ReverseAddImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ReverseAddImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ReverseAddImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ReverseAddImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.ReverseAdd(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ReverseAddImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn SubtractImplementation(FloatDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn SubtractImplementation(IntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn SubtractImplementation(LongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn SubtractImplementation(ShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn SubtractImplementation(UIntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn SubtractImplementation(ULongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Subtract(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn SubtractImplementation(UShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn SubtractImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn SubtractImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn SubtractImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn SubtractImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn SubtractImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn SubtractImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Subtract(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn SubtractImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ReverseSubtractImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ReverseSubtractImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ReverseSubtractImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ReverseSubtractImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ReverseSubtractImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ReverseSubtractImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.ReverseSubtract(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ReverseSubtractImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn MultiplyImplementation(FloatDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn MultiplyImplementation(IntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn MultiplyImplementation(LongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn MultiplyImplementation(ShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn MultiplyImplementation(UIntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn MultiplyImplementation(ULongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Multiply(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn MultiplyImplementation(UShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn MultiplyImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn MultiplyImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn MultiplyImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn MultiplyImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn MultiplyImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn MultiplyImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Multiply(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn MultiplyImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ReverseMultiplyImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ReverseMultiplyImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ReverseMultiplyImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ReverseMultiplyImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ReverseMultiplyImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ReverseMultiplyImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.ReverseMultiply(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ReverseMultiplyImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn DivideImplementation(FloatDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn DivideImplementation(IntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn DivideImplementation(LongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn DivideImplementation(ShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn DivideImplementation(UIntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn DivideImplementation(ULongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Divide(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn DivideImplementation(UShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn DivideImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn DivideImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn DivideImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn DivideImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn DivideImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn DivideImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Divide(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn DivideImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ReverseDivideImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ReverseDivideImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ReverseDivideImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ReverseDivideImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ReverseDivideImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ReverseDivideImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.ReverseDivide(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ReverseDivideImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ModuloImplementation(FloatDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ModuloImplementation(IntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ModuloImplementation(LongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ModuloImplementation(ShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ModuloImplementation(UIntDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ModuloImplementation(ULongDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Modulo(column._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ModuloImplementation(UShortDataFrameColumn column, bool inPlace = false)
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ModuloImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ModuloImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ModuloImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ModuloImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ModuloImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ModuloImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.Modulo(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ModuloImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
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
    public partial class FloatDataFrameColumn
    {
        internal FloatDataFrameColumn ReverseModuloImplementation(float value, bool inPlace = false)
        {
            FloatDataFrameColumn newColumn = inPlace ? this : CloneAsFloatColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        internal IntDataFrameColumn ReverseModuloImplementation(int value, bool inPlace = false)
        {
            IntDataFrameColumn newColumn = inPlace ? this : CloneAsIntColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        internal LongDataFrameColumn ReverseModuloImplementation(long value, bool inPlace = false)
        {
            LongDataFrameColumn newColumn = inPlace ? this : CloneAsLongColumn();
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
    public partial class ShortDataFrameColumn
    {
        internal ShortDataFrameColumn ReverseModuloImplementation(short value, bool inPlace = false)
        {
            ShortDataFrameColumn newColumn = inPlace ? this : CloneAsShortColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        internal UIntDataFrameColumn ReverseModuloImplementation(uint value, bool inPlace = false)
        {
            UIntDataFrameColumn newColumn = inPlace ? this : CloneAsUIntColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        internal ULongDataFrameColumn ReverseModuloImplementation(ulong value, bool inPlace = false)
        {
            ULongDataFrameColumn newColumn = inPlace ? this : CloneAsULongColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        internal UShortDataFrameColumn ReverseModuloImplementation(ushort value, bool inPlace = false)
        {
            UShortDataFrameColumn newColumn = inPlace ? this : CloneAsUShortColumn();
            newColumn._columnContainer.ReverseModulo(value);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseEqualsImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseNotEqualsImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanOrEqualImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanOrEqualImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseGreaterThanImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(BoolDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(ByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(DecimalDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(DoubleDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(FloatDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(IntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(LongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(SByteDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(ShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(UIntDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(ULongDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(UShortDataFrameColumn column)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class BoolDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(bool value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(byte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DecimalDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(decimal value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class DoubleDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(double value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class FloatDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(float value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class IntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(int value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class LongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(long value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class SByteDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(sbyte value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(short value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UIntDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(uint value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class ULongDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(ulong value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
    public partial class UShortDataFrameColumn
    {
        public BoolDataFrameColumn ElementwiseLessThanImplementation(ushort value)
        {
            BoolDataFrameColumn newColumn = CloneAsBoolColumn();
            _columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
            return newColumn;
        }
    }
}
