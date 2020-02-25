
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

        public DataFrameColumn ReverseAddValue<U>(U value, bool inPlace = false)
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
                        newColumn._columnContainer.ReverseAdd(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
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
                        newColumn._columnContainer.ReverseAdd(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                default:
                    throw new NotSupportedException();
            }
        }
        public DataFrameColumn ReverseSubtractValue<U>(U value, bool inPlace = false)
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
                        newColumn._columnContainer.ReverseSubtract(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
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
                        newColumn._columnContainer.ReverseSubtract(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                default:
                    throw new NotSupportedException();
            }
        }
        public DataFrameColumn ReverseMultiplyValue<U>(U value, bool inPlace = false)
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
                        newColumn._columnContainer.ReverseMultiply(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
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
                        newColumn._columnContainer.ReverseMultiply(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                default:
                    throw new NotSupportedException();
            }
        }
        public DataFrameColumn ReverseDivideValue<U>(U value, bool inPlace = false)
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
                        newColumn._columnContainer.ReverseDivide(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
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
                        newColumn._columnContainer.ReverseDivide(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                default:
                    throw new NotSupportedException();
            }
        }
        public DataFrameColumn ReverseModuloValue<U>(U value, bool inPlace = false)
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
                        newColumn._columnContainer.ReverseModulo(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
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
                        newColumn._columnContainer.ReverseModulo(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                default:
                    throw new NotSupportedException();
            }
        }
        public PrimitiveDataFrameColumn<bool> ReverseAndValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseAnd(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        public PrimitiveDataFrameColumn<bool> ReverseOrValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseOr(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        public PrimitiveDataFrameColumn<bool> ReverseXorValue(bool value, bool inPlace = false)
        {
            switch (this)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? boolColumn : boolColumn.Clone();
                    retColumn._columnContainer.ReverseXor(value);
                    return retColumn;
                default:
                    throw new NotSupportedException();
                    
            }
        }
        

    }
}
