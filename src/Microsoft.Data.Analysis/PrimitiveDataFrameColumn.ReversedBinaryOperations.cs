
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveDataFrameColumn.ReversedBinaryOperations.tt. Do not modify directly

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Data.Analysis
{
    public partial class PrimitiveDataFrameColumn<T> : DataFrameColumn
        where T : unmanaged
    {

        public override DataFrameColumn ReverseAddValue<U>(U value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    throw new NotSupportedException();
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseAddValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> clonedDecimalColumn = CloneAsDecimalColumn();
                        clonedDecimalColumn._columnContainer.ReverseAddValue(DecimalConverter<U>.Instance.GetDecimal(value));
                        return clonedDecimalColumn;
                    }
                case PrimitiveDataFrameColumn<byte> byteColumn:
                case PrimitiveDataFrameColumn<char> charColumn:
                case PrimitiveDataFrameColumn<double> doubleColumn:
                case PrimitiveDataFrameColumn<float> floatColumn:
                case PrimitiveDataFrameColumn<int> intColumn:
                case PrimitiveDataFrameColumn<long> longColumn:
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                case PrimitiveDataFrameColumn<short> shortColumn:
                case PrimitiveDataFrameColumn<uint> uintColumn:
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseAddValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ReverseAddValue(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> clonedDoubleColumn = CloneAsDoubleColumn();
                            clonedDoubleColumn._columnContainer.ReverseAddValue(DoubleConverter<U>.Instance.GetDouble(value));
                            return clonedDoubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn ReverseSubtractValue<U>(U value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    throw new NotSupportedException();
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseSubtractValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> clonedDecimalColumn = CloneAsDecimalColumn();
                        clonedDecimalColumn._columnContainer.ReverseSubtractValue(DecimalConverter<U>.Instance.GetDecimal(value));
                        return clonedDecimalColumn;
                    }
                case PrimitiveDataFrameColumn<byte> byteColumn:
                case PrimitiveDataFrameColumn<char> charColumn:
                case PrimitiveDataFrameColumn<double> doubleColumn:
                case PrimitiveDataFrameColumn<float> floatColumn:
                case PrimitiveDataFrameColumn<int> intColumn:
                case PrimitiveDataFrameColumn<long> longColumn:
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                case PrimitiveDataFrameColumn<short> shortColumn:
                case PrimitiveDataFrameColumn<uint> uintColumn:
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseSubtractValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ReverseSubtractValue(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> clonedDoubleColumn = CloneAsDoubleColumn();
                            clonedDoubleColumn._columnContainer.ReverseSubtractValue(DoubleConverter<U>.Instance.GetDouble(value));
                            return clonedDoubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn ReverseMultiplyValue<U>(U value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    throw new NotSupportedException();
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseMultiplyValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> clonedDecimalColumn = CloneAsDecimalColumn();
                        clonedDecimalColumn._columnContainer.ReverseMultiplyValue(DecimalConverter<U>.Instance.GetDecimal(value));
                        return clonedDecimalColumn;
                    }
                case PrimitiveDataFrameColumn<byte> byteColumn:
                case PrimitiveDataFrameColumn<char> charColumn:
                case PrimitiveDataFrameColumn<double> doubleColumn:
                case PrimitiveDataFrameColumn<float> floatColumn:
                case PrimitiveDataFrameColumn<int> intColumn:
                case PrimitiveDataFrameColumn<long> longColumn:
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                case PrimitiveDataFrameColumn<short> shortColumn:
                case PrimitiveDataFrameColumn<uint> uintColumn:
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseMultiplyValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ReverseMultiplyValue(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> clonedDoubleColumn = CloneAsDoubleColumn();
                            clonedDoubleColumn._columnContainer.ReverseMultiplyValue(DoubleConverter<U>.Instance.GetDouble(value));
                            return clonedDoubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn ReverseDivideValue<U>(U value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    throw new NotSupportedException();
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseDivideValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> clonedDecimalColumn = CloneAsDecimalColumn();
                        clonedDecimalColumn._columnContainer.ReverseDivideValue(DecimalConverter<U>.Instance.GetDecimal(value));
                        return clonedDecimalColumn;
                    }
                case PrimitiveDataFrameColumn<byte> byteColumn:
                case PrimitiveDataFrameColumn<char> charColumn:
                case PrimitiveDataFrameColumn<double> doubleColumn:
                case PrimitiveDataFrameColumn<float> floatColumn:
                case PrimitiveDataFrameColumn<int> intColumn:
                case PrimitiveDataFrameColumn<long> longColumn:
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                case PrimitiveDataFrameColumn<short> shortColumn:
                case PrimitiveDataFrameColumn<uint> uintColumn:
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseDivideValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ReverseDivideValue(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> clonedDoubleColumn = CloneAsDoubleColumn();
                            clonedDoubleColumn._columnContainer.ReverseDivideValue(DoubleConverter<U>.Instance.GetDouble(value));
                            return clonedDoubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn ReverseModuloValue<U>(U value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    throw new NotSupportedException();
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseModuloValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> clonedDecimalColumn = CloneAsDecimalColumn();
                        clonedDecimalColumn._columnContainer.ReverseModuloValue(DecimalConverter<U>.Instance.GetDecimal(value));
                        return clonedDecimalColumn;
                    }
                case PrimitiveDataFrameColumn<byte> byteColumn:
                case PrimitiveDataFrameColumn<char> charColumn:
                case PrimitiveDataFrameColumn<double> doubleColumn:
                case PrimitiveDataFrameColumn<float> floatColumn:
                case PrimitiveDataFrameColumn<int> intColumn:
                case PrimitiveDataFrameColumn<long> longColumn:
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                case PrimitiveDataFrameColumn<short> shortColumn:
                case PrimitiveDataFrameColumn<uint> uintColumn:
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? this : Clone();
                        newColumn._columnContainer.ReverseModuloValue(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    else
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ReverseModuloValue(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> clonedDoubleColumn = CloneAsDoubleColumn();
                            clonedDoubleColumn._columnContainer.ReverseModuloValue(DoubleConverter<U>.Instance.GetDouble(value));
                            return clonedDoubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ReverseAndValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseAndValue(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        public override PrimitiveDataFrameColumn<bool> ReverseOrValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseOrValue(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        public override PrimitiveDataFrameColumn<bool> ReverseXorValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseXorValue(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        

    }
}
