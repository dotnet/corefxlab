
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumn.BinaryOperations.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    public partial class PrimitiveColumn<T> : BaseColumn
        where T : unmanaged
    {
        public override BaseColumn Add(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return AddImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return AddImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return AddImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return AddImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return AddImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return AddImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return AddImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return AddImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return AddImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return AddImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return AddImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return AddImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return AddImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Add<U>(U value, bool inPlace = false)
        {
            return AddImplementation(value, inPlace);
        }
        public override BaseColumn Subtract(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return SubtractImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return SubtractImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return SubtractImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return SubtractImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return SubtractImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return SubtractImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return SubtractImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return SubtractImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return SubtractImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return SubtractImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return SubtractImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return SubtractImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return SubtractImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Subtract<U>(U value, bool inPlace = false)
        {
            return SubtractImplementation(value, inPlace);
        }
        public override BaseColumn Multiply(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return MultiplyImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Multiply<U>(U value, bool inPlace = false)
        {
            return MultiplyImplementation(value, inPlace);
        }
        public override BaseColumn Divide(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return DivideImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return DivideImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return DivideImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return DivideImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return DivideImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return DivideImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return DivideImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return DivideImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return DivideImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return DivideImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return DivideImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return DivideImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return DivideImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Divide<U>(U value, bool inPlace = false)
        {
            return DivideImplementation(value, inPlace);
        }
        public override BaseColumn Modulo(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ModuloImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return ModuloImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return ModuloImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ModuloImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return ModuloImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return ModuloImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return ModuloImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return ModuloImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ModuloImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return ModuloImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return ModuloImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ModuloImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ModuloImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Modulo<U>(U value, bool inPlace = false)
        {
            return ModuloImplementation(value, inPlace);
        }
        public override BaseColumn And(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return AndImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return AndImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return AndImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return AndImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return AndImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return AndImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return AndImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return AndImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return AndImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return AndImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return AndImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return AndImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return AndImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn And<U>(U value, bool inPlace = false)
        {
            return AndImplementation(value, inPlace);
        }
        public override BaseColumn Or(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return OrImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return OrImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return OrImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return OrImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return OrImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return OrImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return OrImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return OrImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return OrImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return OrImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return OrImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return OrImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return OrImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Or<U>(U value, bool inPlace = false)
        {
            return OrImplementation(value, inPlace);
        }
        public override BaseColumn Xor(BaseColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return XorImplementation(column as PrimitiveColumn<bool>, inPlace);
                case PrimitiveColumn<byte> byteColumn:
                    return XorImplementation(column as PrimitiveColumn<byte>, inPlace);
                case PrimitiveColumn<char> charColumn:
                    return XorImplementation(column as PrimitiveColumn<char>, inPlace);
                case PrimitiveColumn<decimal> decimalColumn:
                    return XorImplementation(column as PrimitiveColumn<decimal>, inPlace);
                case PrimitiveColumn<double> doubleColumn:
                    return XorImplementation(column as PrimitiveColumn<double>, inPlace);
                case PrimitiveColumn<float> floatColumn:
                    return XorImplementation(column as PrimitiveColumn<float>, inPlace);
                case PrimitiveColumn<int> intColumn:
                    return XorImplementation(column as PrimitiveColumn<int>, inPlace);
                case PrimitiveColumn<long> longColumn:
                    return XorImplementation(column as PrimitiveColumn<long>, inPlace);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return XorImplementation(column as PrimitiveColumn<sbyte>, inPlace);
                case PrimitiveColumn<short> shortColumn:
                    return XorImplementation(column as PrimitiveColumn<short>, inPlace);
                case PrimitiveColumn<uint> uintColumn:
                    return XorImplementation(column as PrimitiveColumn<uint>, inPlace);
                case PrimitiveColumn<ulong> ulongColumn:
                    return XorImplementation(column as PrimitiveColumn<ulong>, inPlace);
                case PrimitiveColumn<ushort> ushortColumn:
                    return XorImplementation(column as PrimitiveColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Xor<U>(U value, bool inPlace = false)
        {
            return XorImplementation(value, inPlace);
        }
        public override BaseColumn LeftShift(int value, bool inPlace = false)
        {
            return LeftShiftImplementation(value, inPlace);
        }
        public override BaseColumn RightShift(int value, bool inPlace = false)
        {
            return RightShiftImplementation(value, inPlace);
        }
        public override BaseColumn Equals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return EqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return EqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return EqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return EqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return EqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return EqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return EqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return EqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return EqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return EqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return EqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return EqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return EqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn Equals<U>(U value)
        {
            return EqualsImplementation(value);
        }
        public override BaseColumn NotEquals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return NotEqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn NotEquals<U>(U value)
        {
            return NotEqualsImplementation(value);
        }
        public override BaseColumn GreaterThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return GreaterThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn GreaterThanOrEqual<U>(U value)
        {
            return GreaterThanOrEqualImplementation(value);
        }
        public override BaseColumn LessThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return LessThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn LessThanOrEqual<U>(U value)
        {
            return LessThanOrEqualImplementation(value);
        }
        public override BaseColumn GreaterThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return GreaterThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn GreaterThan<U>(U value)
        {
            return GreaterThanImplementation(value);
        }
        public override BaseColumn LessThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return LessThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return LessThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return LessThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return LessThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return LessThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return LessThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return LessThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return LessThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return LessThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return LessThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return LessThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return LessThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return LessThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override BaseColumn LessThan<U>(U value)
        {
            return LessThanImplementation(value);
        }

        internal BaseColumn AddImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Add(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Add(column.CloneAsDecimalColumn()._columnContainer);
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Add(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Add((column as PrimitiveColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Add(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn AddImplementation<U>(U value, bool inPlace)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Add(value);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Add(DecimalConverter<U>.Instance.GetDecimal(value));
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Add(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Add(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Add(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn SubtractImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Subtract(column.CloneAsDecimalColumn()._columnContainer);
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Subtract((column as PrimitiveColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Subtract(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn SubtractImplementation<U>(U value, bool inPlace)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(value);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Subtract(DecimalConverter<U>.Instance.GetDecimal(value));
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Subtract(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Subtract(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn MultiplyImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Multiply(column.CloneAsDecimalColumn()._columnContainer);
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Multiply((column as PrimitiveColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Multiply(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn MultiplyImplementation<U>(U value, bool inPlace)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(value);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Multiply(DecimalConverter<U>.Instance.GetDecimal(value));
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Multiply(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Multiply(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn DivideImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Divide(column.CloneAsDecimalColumn()._columnContainer);
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Divide((column as PrimitiveColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Divide(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn DivideImplementation<U>(U value, bool inPlace)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(value);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Divide(DecimalConverter<U>.Instance.GetDecimal(value));
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Divide(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Divide(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn ModuloImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Modulo(column.CloneAsDecimalColumn()._columnContainer);
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Modulo((column as PrimitiveColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Modulo(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn ModuloImplementation<U>(U value, bool inPlace)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(value);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Modulo(DecimalConverter<U>.Instance.GetDecimal(value));
                        return decimalColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<U> newColumn;
                        if (inPlace)
                            newColumn = primitiveColumn;
                        else
                            newColumn = primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Modulo(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Modulo(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn AndImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
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
        internal BaseColumn AndImplementation<U>(U value, bool inPlace)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
                    retColumn._columnContainer.And(value);
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
        internal BaseColumn OrImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
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
        internal BaseColumn OrImplementation<U>(U value, bool inPlace)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
                    retColumn._columnContainer.Or(value);
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
        internal BaseColumn XorImplementation<U>(PrimitiveColumn<U> column, bool inPlace)
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
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
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
        internal BaseColumn XorImplementation<U>(U value, bool inPlace)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveColumn<U> typedColumn = this as PrimitiveColumn<U>;
                    PrimitiveColumn<U> retColumn;
                    if (inPlace)
                        retColumn = typedColumn;
                    else
                        retColumn = typedColumn.Clone();
                    retColumn._columnContainer.Xor(value);
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
        internal BaseColumn LeftShiftImplementation(int value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type byteType when byteType == typeof(byte):
                    PrimitiveColumn<byte> byteColumn = this as PrimitiveColumn<byte>;
                    PrimitiveColumn<byte> newbyteColumn;
                    if (inPlace)
                        newbyteColumn = byteColumn;
                    else
                        newbyteColumn = byteColumn.Clone();
                    newbyteColumn._columnContainer.LeftShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveColumn<char> charColumn = this as PrimitiveColumn<char>;
                    PrimitiveColumn<char> newcharColumn;
                    if (inPlace)
                        newcharColumn = charColumn;
                    else
                        newcharColumn = charColumn.Clone();
                    newcharColumn._columnContainer.LeftShift(value);
                    return newcharColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    throw new NotSupportedException();
                case Type doubleType when doubleType == typeof(double):
                    throw new NotSupportedException();
                case Type floatType when floatType == typeof(float):
                    throw new NotSupportedException();
                case Type intType when intType == typeof(int):
                    PrimitiveColumn<int> intColumn = this as PrimitiveColumn<int>;
                    PrimitiveColumn<int> newintColumn;
                    if (inPlace)
                        newintColumn = intColumn;
                    else
                        newintColumn = intColumn.Clone();
                    newintColumn._columnContainer.LeftShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveColumn<long> longColumn = this as PrimitiveColumn<long>;
                    PrimitiveColumn<long> newlongColumn;
                    if (inPlace)
                        newlongColumn = longColumn;
                    else
                        newlongColumn = longColumn.Clone();
                    newlongColumn._columnContainer.LeftShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveColumn<sbyte> sbyteColumn = this as PrimitiveColumn<sbyte>;
                    PrimitiveColumn<sbyte> newsbyteColumn;
                    if (inPlace)
                        newsbyteColumn = sbyteColumn;
                    else
                        newsbyteColumn = sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.LeftShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveColumn<short> shortColumn = this as PrimitiveColumn<short>;
                    PrimitiveColumn<short> newshortColumn;
                    if (inPlace)
                        newshortColumn = shortColumn;
                    else
                        newshortColumn = shortColumn.Clone();
                    newshortColumn._columnContainer.LeftShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveColumn<uint> uintColumn = this as PrimitiveColumn<uint>;
                    PrimitiveColumn<uint> newuintColumn;
                    if (inPlace)
                        newuintColumn = uintColumn;
                    else
                        newuintColumn = uintColumn.Clone();
                    newuintColumn._columnContainer.LeftShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveColumn<ulong> ulongColumn = this as PrimitiveColumn<ulong>;
                    PrimitiveColumn<ulong> newulongColumn;
                    if (inPlace)
                        newulongColumn = ulongColumn;
                    else
                        newulongColumn = ulongColumn.Clone();
                    newulongColumn._columnContainer.LeftShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveColumn<ushort> ushortColumn = this as PrimitiveColumn<ushort>;
                    PrimitiveColumn<ushort> newushortColumn;
                    if (inPlace)
                        newushortColumn = ushortColumn;
                    else
                        newushortColumn = ushortColumn.Clone();
                    newushortColumn._columnContainer.LeftShift(value);
                    return newushortColumn;
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn RightShiftImplementation(int value, bool inPlace)
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    throw new NotSupportedException();
                case Type byteType when byteType == typeof(byte):
                    PrimitiveColumn<byte> byteColumn = this as PrimitiveColumn<byte>;
                    PrimitiveColumn<byte> newbyteColumn;
                    if (inPlace)
                        newbyteColumn = byteColumn;
                    else
                        newbyteColumn = byteColumn.Clone();
                    newbyteColumn._columnContainer.RightShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveColumn<char> charColumn = this as PrimitiveColumn<char>;
                    PrimitiveColumn<char> newcharColumn;
                    if (inPlace)
                        newcharColumn = charColumn;
                    else
                        newcharColumn = charColumn.Clone();
                    newcharColumn._columnContainer.RightShift(value);
                    return newcharColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    throw new NotSupportedException();
                case Type doubleType when doubleType == typeof(double):
                    throw new NotSupportedException();
                case Type floatType when floatType == typeof(float):
                    throw new NotSupportedException();
                case Type intType when intType == typeof(int):
                    PrimitiveColumn<int> intColumn = this as PrimitiveColumn<int>;
                    PrimitiveColumn<int> newintColumn;
                    if (inPlace)
                        newintColumn = intColumn;
                    else
                        newintColumn = intColumn.Clone();
                    newintColumn._columnContainer.RightShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveColumn<long> longColumn = this as PrimitiveColumn<long>;
                    PrimitiveColumn<long> newlongColumn;
                    if (inPlace)
                        newlongColumn = longColumn;
                    else
                        newlongColumn = longColumn.Clone();
                    newlongColumn._columnContainer.RightShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveColumn<sbyte> sbyteColumn = this as PrimitiveColumn<sbyte>;
                    PrimitiveColumn<sbyte> newsbyteColumn;
                    if (inPlace)
                        newsbyteColumn = sbyteColumn;
                    else
                        newsbyteColumn = sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.RightShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveColumn<short> shortColumn = this as PrimitiveColumn<short>;
                    PrimitiveColumn<short> newshortColumn;
                    if (inPlace)
                        newshortColumn = shortColumn;
                    else
                        newshortColumn = shortColumn.Clone();
                    newshortColumn._columnContainer.RightShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveColumn<uint> uintColumn = this as PrimitiveColumn<uint>;
                    PrimitiveColumn<uint> newuintColumn;
                    if (inPlace)
                        newuintColumn = uintColumn;
                    else
                        newuintColumn = uintColumn.Clone();
                    newuintColumn._columnContainer.RightShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveColumn<ulong> ulongColumn = this as PrimitiveColumn<ulong>;
                    PrimitiveColumn<ulong> newulongColumn;
                    if (inPlace)
                        newulongColumn = ulongColumn;
                    else
                        newulongColumn = ulongColumn.Clone();
                    newulongColumn._columnContainer.RightShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveColumn<ushort> ushortColumn = this as PrimitiveColumn<ushort>;
                    PrimitiveColumn<ushort> newushortColumn;
                    if (inPlace)
                        newushortColumn = ushortColumn;
                    else
                        newushortColumn = ushortColumn.Clone();
                    newushortColumn._columnContainer.RightShift(value);
                    return newushortColumn;
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn EqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    PrimitiveColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveColumn<U>)._columnContainer.Equals(column._columnContainer, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.Equals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Equals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.Equals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Equals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Equals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn EqualsImplementation<U>(U value)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveColumn<U>)._columnContainer.Equals(value, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.Equals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.Equals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.Equals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Equals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Equals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn NotEqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    PrimitiveColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveColumn<U>)._columnContainer.NotEquals(column._columnContainer, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.NotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.NotEquals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.NotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.NotEquals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.NotEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn NotEqualsImplementation<U>(U value)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveColumn<U>)._columnContainer.NotEquals(value, retColumn._columnContainer);
                    return retColumn;
                case Type decimalType when decimalType == typeof(decimal):
                    if (typeof(U) == typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    if (typeof(U) == typeof(T))
                    {
                        // No conversions
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.NotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.NotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.NotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.NotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.NotEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn GreaterThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.GreaterThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.GreaterThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.GreaterThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn GreaterThanOrEqualImplementation<U>(U value)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.GreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.GreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.GreaterThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn LessThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.LessThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.LessThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.LessThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn LessThanOrEqualImplementation<U>(U value)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.LessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.LessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.LessThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn GreaterThanImplementation<U>(PrimitiveColumn<U> column)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.GreaterThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.GreaterThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.GreaterThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn GreaterThanImplementation<U>(U value)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.GreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.GreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.GreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.GreaterThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn LessThanImplementation<U>(PrimitiveColumn<U> column)
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.LessThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.LessThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.LessThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal BaseColumn LessThanImplementation<U>(U value)
            where U : unmanaged
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.LessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                        return newColumn;
                    }
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
                        PrimitiveColumn<U> primitiveColumn = this as PrimitiveColumn<U>;
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.LessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.LessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.LessThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
