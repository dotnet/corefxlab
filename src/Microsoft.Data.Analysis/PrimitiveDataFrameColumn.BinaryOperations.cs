
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveDataFrameColumn.BinaryOperations.tt. Do not modify directly

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Data.Analysis
{
    public partial class PrimitiveDataFrameColumn<T> : DataFrameColumn
        where T : unmanaged
    {
        public override DataFrameColumn Add(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Add<U>(U value, bool inPlace = false)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return Add(column, inPlace);
            }
            return AddImplementation(value, inPlace);
        }
        public override DataFrameColumn Subtract(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Subtract<U>(U value, bool inPlace = false)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return Subtract(column, inPlace);
            }
            return SubtractImplementation(value, inPlace);
        }
        public override DataFrameColumn Multiply(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Multiply<U>(U value, bool inPlace = false)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return Multiply(column, inPlace);
            }
            return MultiplyImplementation(value, inPlace);
        }
        public override DataFrameColumn Divide(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Divide<U>(U value, bool inPlace = false)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return Divide(column, inPlace);
            }
            return DivideImplementation(value, inPlace);
        }
        public override DataFrameColumn Modulo(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Modulo<U>(U value, bool inPlace = false)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return Modulo(column, inPlace);
            }
            return ModuloImplementation(value, inPlace);
        }
        public override DataFrameColumn And(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> And(bool value, bool inPlace = false)
        {
            return AndImplementation(value, inPlace);
        }
        public override DataFrameColumn Or(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> Or(bool value, bool inPlace = false)
        {
            return OrImplementation(value, inPlace);
        }
        public override DataFrameColumn Xor(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case ByteDataFrameColumn byteColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case CharDataFrameColumn charColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case DecimalDataFrameColumn decimalColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case DoubleDataFrameColumn doubleColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case FloatDataFrameColumn floatColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case IntDataFrameColumn intColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case LongDataFrameColumn longColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case SByteDataFrameColumn sbyteColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case ShortDataFrameColumn shortColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case UIntDataFrameColumn uintColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case ULongDataFrameColumn ulongColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case UShortDataFrameColumn ushortColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> Xor(bool value, bool inPlace = false)
        {
            return XorImplementation(value, inPlace);
        }
        public override DataFrameColumn LeftShift(int value, bool inPlace = false)
        {
            return LeftShiftImplementation(value, inPlace);
        }
        public override DataFrameColumn RightShift(int value, bool inPlace = false)
        {
            return RightShiftImplementation(value, inPlace);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseEquals(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseEquals<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseEquals(column);
            }
            return ElementwiseEqualsImplementation(value);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseNotEquals(column);
            }
            return ElementwiseNotEqualsImplementation(value);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqual(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqual<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseGreaterThanOrEqual(column);
            }
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqual(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqual<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseLessThanOrEqual(column);
            }
            return ElementwiseLessThanOrEqualImplementation(value);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThan(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThan<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseGreaterThan(column);
            }
            return ElementwiseGreaterThanImplementation(value);
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThan(DataFrameColumn column)
        {
            switch (column)
            {
                case BoolDataFrameColumn boolColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case ByteDataFrameColumn byteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case CharDataFrameColumn charColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case DecimalDataFrameColumn decimalColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case DoubleDataFrameColumn doubleColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case FloatDataFrameColumn floatColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case IntDataFrameColumn intColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case LongDataFrameColumn longColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case SByteDataFrameColumn sbyteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case ShortDataFrameColumn shortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case UIntDataFrameColumn uintColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case ULongDataFrameColumn ulongColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case UShortDataFrameColumn ushortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThan<U>(U value)
        {
            DataFrameColumn column = value as DataFrameColumn;
            if (column != null)
            {
                return ElementwiseLessThan(column);
            }
            return ElementwiseLessThanImplementation(value);
        }

        internal DataFrameColumn AddImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn AddImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn SubtractImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn SubtractImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn MultiplyImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn MultiplyImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn DivideImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn DivideImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn ModuloImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(column._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn ModuloImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(Unsafe.As<U, T>(ref value));
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn AndImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.And(column._columnContainer);
                    return retColumn;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> AndImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> typedColumn = this as PrimitiveDataFrameColumn<bool>;
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.And(Unsafe.As<U, bool>(ref value));
                    return retColumn as PrimitiveDataFrameColumn<bool>;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn OrImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Or(column._columnContainer);
                    return retColumn;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> OrImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> typedColumn = this as PrimitiveDataFrameColumn<bool>;
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Or(Unsafe.As<U, bool>(ref value));
                    return retColumn as PrimitiveDataFrameColumn<bool>;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn XorImplementation<U>(PrimitiveDataFrameColumn<U> column, bool inPlace)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Xor(column._columnContainer);
                    return retColumn;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> XorImplementation<U>(U value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> typedColumn = this as PrimitiveDataFrameColumn<bool>;
                    PrimitiveDataFrameColumn<bool> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Xor(Unsafe.As<U, bool>(ref value));
                    return retColumn as PrimitiveDataFrameColumn<bool>;
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type decimalType when decimalType == typeof(decimal):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn LeftShiftImplementation(int value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type byteType when byteType == typeof(byte):
                    PrimitiveDataFrameColumn<byte> byteColumn = this as PrimitiveDataFrameColumn<byte>;
                    PrimitiveDataFrameColumn<byte> newbyteColumn = inPlace ? byteColumn : byteColumn.Clone();
                    newbyteColumn._columnContainer.LeftShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveDataFrameColumn<char> charColumn = this as PrimitiveDataFrameColumn<char>;
                    PrimitiveDataFrameColumn<char> newcharColumn = inPlace ? charColumn : charColumn.Clone();
                    newcharColumn._columnContainer.LeftShift(value);
                    return newcharColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    throw new NotSupportedException();
                case Type doubleType when doubleType == typeof(double):
                    throw new NotSupportedException();
                case Type floatType when floatType == typeof(float):
                    throw new NotSupportedException();
                case Type intType when intType == typeof(int):
                    PrimitiveDataFrameColumn<int> intColumn = this as PrimitiveDataFrameColumn<int>;
                    PrimitiveDataFrameColumn<int> newintColumn = inPlace ? intColumn : intColumn.Clone();
                    newintColumn._columnContainer.LeftShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveDataFrameColumn<long> longColumn = this as PrimitiveDataFrameColumn<long>;
                    PrimitiveDataFrameColumn<long> newlongColumn = inPlace ? longColumn : longColumn.Clone();
                    newlongColumn._columnContainer.LeftShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveDataFrameColumn<sbyte> sbyteColumn = this as PrimitiveDataFrameColumn<sbyte>;
                    PrimitiveDataFrameColumn<sbyte> newsbyteColumn = inPlace ? sbyteColumn : sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.LeftShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveDataFrameColumn<short> shortColumn = this as PrimitiveDataFrameColumn<short>;
                    PrimitiveDataFrameColumn<short> newshortColumn = inPlace ? shortColumn : shortColumn.Clone();
                    newshortColumn._columnContainer.LeftShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveDataFrameColumn<uint> uintColumn = this as PrimitiveDataFrameColumn<uint>;
                    PrimitiveDataFrameColumn<uint> newuintColumn = inPlace ? uintColumn : uintColumn.Clone();
                    newuintColumn._columnContainer.LeftShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveDataFrameColumn<ulong> ulongColumn = this as PrimitiveDataFrameColumn<ulong>;
                    PrimitiveDataFrameColumn<ulong> newulongColumn = inPlace ? ulongColumn : ulongColumn.Clone();
                    newulongColumn._columnContainer.LeftShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveDataFrameColumn<ushort> ushortColumn = this as PrimitiveDataFrameColumn<ushort>;
                    PrimitiveDataFrameColumn<ushort> newushortColumn = inPlace ? ushortColumn : ushortColumn.Clone();
                    newushortColumn._columnContainer.LeftShift(value);
                    return newushortColumn;
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn RightShiftImplementation(int value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type byteType when byteType == typeof(byte):
                    PrimitiveDataFrameColumn<byte> byteColumn = this as PrimitiveDataFrameColumn<byte>;
                    PrimitiveDataFrameColumn<byte> newbyteColumn = inPlace ? byteColumn : byteColumn.Clone();
                    newbyteColumn._columnContainer.RightShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveDataFrameColumn<char> charColumn = this as PrimitiveDataFrameColumn<char>;
                    PrimitiveDataFrameColumn<char> newcharColumn = inPlace ? charColumn : charColumn.Clone();
                    newcharColumn._columnContainer.RightShift(value);
                    return newcharColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    throw new NotSupportedException();
                case Type doubleType when doubleType == typeof(double):
                    throw new NotSupportedException();
                case Type floatType when floatType == typeof(float):
                    throw new NotSupportedException();
                case Type intType when intType == typeof(int):
                    PrimitiveDataFrameColumn<int> intColumn = this as PrimitiveDataFrameColumn<int>;
                    PrimitiveDataFrameColumn<int> newintColumn = inPlace ? intColumn : intColumn.Clone();
                    newintColumn._columnContainer.RightShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveDataFrameColumn<long> longColumn = this as PrimitiveDataFrameColumn<long>;
                    PrimitiveDataFrameColumn<long> newlongColumn = inPlace ? longColumn : longColumn.Clone();
                    newlongColumn._columnContainer.RightShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveDataFrameColumn<sbyte> sbyteColumn = this as PrimitiveDataFrameColumn<sbyte>;
                    PrimitiveDataFrameColumn<sbyte> newsbyteColumn = inPlace ? sbyteColumn : sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.RightShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveDataFrameColumn<short> shortColumn = this as PrimitiveDataFrameColumn<short>;
                    PrimitiveDataFrameColumn<short> newshortColumn = inPlace ? shortColumn : shortColumn.Clone();
                    newshortColumn._columnContainer.RightShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveDataFrameColumn<uint> uintColumn = this as PrimitiveDataFrameColumn<uint>;
                    PrimitiveDataFrameColumn<uint> newuintColumn = inPlace ? uintColumn : uintColumn.Clone();
                    newuintColumn._columnContainer.RightShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveDataFrameColumn<ulong> ulongColumn = this as PrimitiveDataFrameColumn<ulong>;
                    PrimitiveDataFrameColumn<ulong> newulongColumn = inPlace ? ulongColumn : ulongColumn.Clone();
                    newulongColumn._columnContainer.RightShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveDataFrameColumn<ushort> ushortColumn = this as PrimitiveDataFrameColumn<ushort>;
                    PrimitiveDataFrameColumn<ushort> newushortColumn = inPlace ? ushortColumn : ushortColumn.Clone();
                    newushortColumn._columnContainer.RightShift(value);
                    return newushortColumn;
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseEqualsImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<U>)._columnContainer.ElementwiseEquals(column._columnContainer, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseEqualsImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<bool>)._columnContainer.ElementwiseEquals(Unsafe.As<U, bool>(ref value), retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseNotEqualsImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<U>)._columnContainer.ElementwiseNotEquals(column._columnContainer, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseNotEqualsImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<bool>)._columnContainer.ElementwiseNotEquals(Unsafe.As<U, bool>(ref value), retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqualImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqualImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqualImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqualImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanImplementation<U>(PrimitiveDataFrameColumn<U> column)
            where U : unmanaged
        {
            if (column.Length != Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanImplementation<U>(U value)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                case Type byteType when byteType == typeof(byte):
                case Type charType when charType == typeof(char):
                case Type doubleType when doubleType == typeof(double):
                case Type floatType when floatType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type sbyteType when sbyteType == typeof(sbyte):
                case Type shortType when shortType == typeof(short):
                case Type uintType when uintType == typeof(uint):
                case Type ulongType when ulongType == typeof(ulong):
                case Type ushortType when ushortType == typeof(ushort):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
                        return newColumn;
                    }
                    throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(U)));
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
