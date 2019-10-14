
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
        public override BaseColumn And(bool value, bool inPlace = false)
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
        public override BaseColumn Or(bool value, bool inPlace = false)
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
        public override BaseColumn Xor(bool value, bool inPlace = false)
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
        public override PrimitiveColumn<bool> PairwiseEquals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseEqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseEquals<U>(U value)
        {
            return PairwiseEqualsImplementation(value);
        }
        public override PrimitiveColumn<bool> PairwiseNotEquals(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseNotEqualsImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseNotEquals<U>(U value)
        {
            return PairwiseNotEqualsImplementation(value);
        }
        public override PrimitiveColumn<bool> PairwiseGreaterThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseGreaterThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseGreaterThanOrEqual<U>(U value)
        {
            return PairwiseGreaterThanOrEqualImplementation(value);
        }
        public override PrimitiveColumn<bool> PairwiseLessThanOrEqual(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseLessThanOrEqualImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseLessThanOrEqual<U>(U value)
        {
            return PairwiseLessThanOrEqualImplementation(value);
        }
        public override PrimitiveColumn<bool> PairwiseGreaterThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseGreaterThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseGreaterThan<U>(U value)
        {
            return PairwiseGreaterThanImplementation(value);
        }
        public override PrimitiveColumn<bool> PairwiseLessThan(BaseColumn column)
        {
            switch (column)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<bool>);
                case PrimitiveColumn<byte> byteColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<byte>);
                case PrimitiveColumn<char> charColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<char>);
                case PrimitiveColumn<decimal> decimalColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<decimal>);
                case PrimitiveColumn<double> doubleColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<double>);
                case PrimitiveColumn<float> floatColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<float>);
                case PrimitiveColumn<int> intColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<int>);
                case PrimitiveColumn<long> longColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<long>);
                case PrimitiveColumn<sbyte> sbyteColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<sbyte>);
                case PrimitiveColumn<short> shortColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<short>);
                case PrimitiveColumn<uint> uintColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<uint>);
                case PrimitiveColumn<ulong> ulongColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<ulong>);
                case PrimitiveColumn<ushort> ushortColumn:
                    return PairwiseLessThanImplementation(column as PrimitiveColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveColumn<bool> PairwiseLessThan<U>(U value)
        {
            return PairwiseLessThanImplementation(value);
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
                    PrimitiveColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
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
        internal PrimitiveColumn<bool> PairwiseEqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    (this as PrimitiveColumn<U>)._columnContainer.PairwiseEquals(column._columnContainer, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseEquals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseEquals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseEqualsImplementation<U>(U value)
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
                    (this as PrimitiveColumn<U>)._columnContainer.PairwiseEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseNotEqualsImplementation<U>(PrimitiveColumn<U> column)
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
                    (this as PrimitiveColumn<U>)._columnContainer.PairwiseNotEquals(column._columnContainer, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseNotEquals(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseNotEquals((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseNotEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseNotEqualsImplementation<U>(U value)
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
                    (this as PrimitiveColumn<U>)._columnContainer.PairwiseNotEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseNotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseNotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseNotEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseGreaterThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.PairwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseGreaterThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseGreaterThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseGreaterThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseGreaterThanOrEqualImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.PairwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseGreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseGreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseGreaterThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseLessThanOrEqualImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.PairwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseLessThanOrEqual(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseLessThanOrEqual((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseLessThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseLessThanOrEqualImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.PairwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseLessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseLessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseLessThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseGreaterThanImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.PairwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseGreaterThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseGreaterThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseGreaterThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseGreaterThanImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.PairwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseGreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseGreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseGreaterThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseLessThanImplementation<U>(PrimitiveColumn<U> column)
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
                        primitiveColumn._columnContainer.PairwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseLessThan(column.CloneAsDecimalColumn()._columnContainer, newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseLessThan((column as PrimitiveColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseLessThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveColumn<bool> PairwiseLessThanImplementation<U>(U value)
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
                        primitiveColumn._columnContainer.PairwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                        decimalColumn._columnContainer.PairwiseLessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
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
                        primitiveColumn._columnContainer.PairwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.PairwiseLessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.PairwiseLessThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
