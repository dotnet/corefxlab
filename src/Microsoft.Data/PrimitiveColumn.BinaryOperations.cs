
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
        public override PrimitiveColumn<bool> And(bool value, bool inPlace = false)
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
        public override PrimitiveColumn<bool> Or(bool value, bool inPlace = false)
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
        public override PrimitiveColumn<bool> Xor(bool value, bool inPlace = false)
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
        public override PrimitiveColumn<bool> ElementwiseEquals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseEquals<U>(U value)
        {
            return ElementwiseEqualsImplementation(value);
        }
        public override PrimitiveColumn<bool> ElementwiseNotEquals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseNotEquals<U>(U value)
        {
            return ElementwiseNotEqualsImplementation(value);
        }
        public override PrimitiveColumn<bool> ElementwiseGreaterThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseGreaterThanOrEqual<U>(U value)
        {
            return ElementwiseGreaterThanOrEqualImplementation(value);
        }
        public override PrimitiveColumn<bool> ElementwiseLessThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseLessThanOrEqual<U>(U value)
        {
            return ElementwiseLessThanOrEqualImplementation(value);
        }
        public override PrimitiveColumn<bool> ElementwiseGreaterThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseGreaterThan<U>(U value)
        {
            return ElementwiseGreaterThanImplementation(value);
        }
        public override PrimitiveColumn<bool> ElementwiseLessThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> ElementwiseLessThan<U>(U value)
        {
            return ElementwiseLessThanImplementation(value);
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                        PrimitiveColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
        internal PrimitiveColumn<bool> AndImplementation<U>(U value, bool inPlace)
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.And(value);
                    return retColumn as PrimitiveColumn<bool>;
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
        internal PrimitiveColumn<bool> OrImplementation<U>(U value, bool inPlace)
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Or(value);
                    return retColumn as PrimitiveColumn<bool>;
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
        internal PrimitiveColumn<bool> XorImplementation<U>(U value, bool inPlace)
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Xor(value);
                    return retColumn as PrimitiveColumn<bool>;
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
                    PrimitiveColumn<byte> newbyteColumn = inPlace ? byteColumn : byteColumn.Clone();
                    newbyteColumn._columnContainer.LeftShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveColumn<char> charColumn = this as PrimitiveColumn<char>;
                    PrimitiveColumn<char> newcharColumn = inPlace ? charColumn : charColumn.Clone();
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
                    PrimitiveColumn<int> newintColumn = inPlace ? intColumn : intColumn.Clone();
                    newintColumn._columnContainer.LeftShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveColumn<long> longColumn = this as PrimitiveColumn<long>;
                    PrimitiveColumn<long> newlongColumn = inPlace ? longColumn : longColumn.Clone();
                    newlongColumn._columnContainer.LeftShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveColumn<sbyte> sbyteColumn = this as PrimitiveColumn<sbyte>;
                    PrimitiveColumn<sbyte> newsbyteColumn = inPlace ? sbyteColumn : sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.LeftShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveColumn<short> shortColumn = this as PrimitiveColumn<short>;
                    PrimitiveColumn<short> newshortColumn = inPlace ? shortColumn : shortColumn.Clone();
                    newshortColumn._columnContainer.LeftShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveColumn<uint> uintColumn = this as PrimitiveColumn<uint>;
                    PrimitiveColumn<uint> newuintColumn = inPlace ? uintColumn : uintColumn.Clone();
                    newuintColumn._columnContainer.LeftShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveColumn<ulong> ulongColumn = this as PrimitiveColumn<ulong>;
                    PrimitiveColumn<ulong> newulongColumn = inPlace ? ulongColumn : ulongColumn.Clone();
                    newulongColumn._columnContainer.LeftShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveColumn<ushort> ushortColumn = this as PrimitiveColumn<ushort>;
                    PrimitiveColumn<ushort> newushortColumn = inPlace ? ushortColumn : ushortColumn.Clone();
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
                    PrimitiveColumn<byte> newbyteColumn = inPlace ? byteColumn : byteColumn.Clone();
                    newbyteColumn._columnContainer.RightShift(value);
                    return newbyteColumn;
                case Type charType when charType == typeof(char):
                    PrimitiveColumn<char> charColumn = this as PrimitiveColumn<char>;
                    PrimitiveColumn<char> newcharColumn = inPlace ? charColumn : charColumn.Clone();
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
                    PrimitiveColumn<int> newintColumn = inPlace ? intColumn : intColumn.Clone();
                    newintColumn._columnContainer.RightShift(value);
                    return newintColumn;
                case Type longType when longType == typeof(long):
                    PrimitiveColumn<long> longColumn = this as PrimitiveColumn<long>;
                    PrimitiveColumn<long> newlongColumn = inPlace ? longColumn : longColumn.Clone();
                    newlongColumn._columnContainer.RightShift(value);
                    return newlongColumn;
                case Type sbyteType when sbyteType == typeof(sbyte):
                    PrimitiveColumn<sbyte> sbyteColumn = this as PrimitiveColumn<sbyte>;
                    PrimitiveColumn<sbyte> newsbyteColumn = inPlace ? sbyteColumn : sbyteColumn.Clone();
                    newsbyteColumn._columnContainer.RightShift(value);
                    return newsbyteColumn;
                case Type shortType when shortType == typeof(short):
                    PrimitiveColumn<short> shortColumn = this as PrimitiveColumn<short>;
                    PrimitiveColumn<short> newshortColumn = inPlace ? shortColumn : shortColumn.Clone();
                    newshortColumn._columnContainer.RightShift(value);
                    return newshortColumn;
                case Type uintType when uintType == typeof(uint):
                    PrimitiveColumn<uint> uintColumn = this as PrimitiveColumn<uint>;
                    PrimitiveColumn<uint> newuintColumn = inPlace ? uintColumn : uintColumn.Clone();
                    newuintColumn._columnContainer.RightShift(value);
                    return newuintColumn;
                case Type ulongType when ulongType == typeof(ulong):
                    PrimitiveColumn<ulong> ulongColumn = this as PrimitiveColumn<ulong>;
                    PrimitiveColumn<ulong> newulongColumn = inPlace ? ulongColumn : ulongColumn.Clone();
                    newulongColumn._columnContainer.RightShift(value);
                    return newulongColumn;
                case Type ushortType when ushortType == typeof(ushort):
                    PrimitiveColumn<ushort> ushortColumn = this as PrimitiveColumn<ushort>;
                    PrimitiveColumn<ushort> newushortColumn = inPlace ? ushortColumn : ushortColumn.Clone();
                    newushortColumn._columnContainer.RightShift(value);
                    return newushortColumn;
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseEqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    (this as PrimitiveColumn<U>)._columnContainer.ElementwiseEquals(column._columnContainer, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseEquals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseEquals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseEqualsImplementation<U>(U value)
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
                    (this as PrimitiveColumn<U>)._columnContainer.ElementwiseEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseNotEqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    (this as PrimitiveColumn<U>)._columnContainer.ElementwiseNotEquals(column._columnContainer, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseNotEquals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseNotEquals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseNotEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseNotEqualsImplementation<U>(U value)
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
                    (this as PrimitiveColumn<U>)._columnContainer.ElementwiseNotEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseNotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseNotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseNotEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseGreaterThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseGreaterThanOrEqualImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseLessThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseLessThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseLessThanOrEqualImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseLessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseGreaterThanImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseGreaterThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseGreaterThanImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseGreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseLessThanImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseLessThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> ElementwiseLessThanImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.ElementwiseLessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
