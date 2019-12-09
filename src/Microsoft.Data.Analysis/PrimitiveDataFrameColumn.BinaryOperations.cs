
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
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return AddImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Add<U>(U value, bool inPlace = false)
        {
            if (typeof(U) == typeof(bool))
            {
                return AddImplementation(Unsafe.As<U, bool>(ref value), inPlace);
            }
            if (typeof(U) == typeof(byte))
            {
                return AddImplementation(Unsafe.As<U, byte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(char))
            {
                return AddImplementation(Unsafe.As<U, char>(ref value), inPlace);
            }
            if (typeof(U) == typeof(decimal))
            {
                return AddImplementation(Unsafe.As<U, decimal>(ref value), inPlace);
            }
            if (typeof(U) == typeof(double))
            {
                return AddImplementation(Unsafe.As<U, double>(ref value), inPlace);
            }
            if (typeof(U) == typeof(float))
            {
                return AddImplementation(Unsafe.As<U, float>(ref value), inPlace);
            }
            if (typeof(U) == typeof(int))
            {
                return AddImplementation(Unsafe.As<U, int>(ref value), inPlace);
            }
            if (typeof(U) == typeof(long))
            {
                return AddImplementation(Unsafe.As<U, long>(ref value), inPlace);
            }
            if (typeof(U) == typeof(sbyte))
            {
                return AddImplementation(Unsafe.As<U, sbyte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(short))
            {
                return AddImplementation(Unsafe.As<U, short>(ref value), inPlace);
            }
            if (typeof(U) == typeof(uint))
            {
                return AddImplementation(Unsafe.As<U, uint>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ulong))
            {
                return AddImplementation(Unsafe.As<U, ulong>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ushort))
            {
                return AddImplementation(Unsafe.As<U, ushort>(ref value), inPlace);
            }
            throw new NotImplementedException();
        }
        public override DataFrameColumn Subtract(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Subtract<U>(U value, bool inPlace = false)
        {
            if (typeof(U) == typeof(bool))
            {
                return SubtractImplementation(Unsafe.As<U, bool>(ref value), inPlace);
            }
            if (typeof(U) == typeof(byte))
            {
                return SubtractImplementation(Unsafe.As<U, byte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(char))
            {
                return SubtractImplementation(Unsafe.As<U, char>(ref value), inPlace);
            }
            if (typeof(U) == typeof(decimal))
            {
                return SubtractImplementation(Unsafe.As<U, decimal>(ref value), inPlace);
            }
            if (typeof(U) == typeof(double))
            {
                return SubtractImplementation(Unsafe.As<U, double>(ref value), inPlace);
            }
            if (typeof(U) == typeof(float))
            {
                return SubtractImplementation(Unsafe.As<U, float>(ref value), inPlace);
            }
            if (typeof(U) == typeof(int))
            {
                return SubtractImplementation(Unsafe.As<U, int>(ref value), inPlace);
            }
            if (typeof(U) == typeof(long))
            {
                return SubtractImplementation(Unsafe.As<U, long>(ref value), inPlace);
            }
            if (typeof(U) == typeof(sbyte))
            {
                return SubtractImplementation(Unsafe.As<U, sbyte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(short))
            {
                return SubtractImplementation(Unsafe.As<U, short>(ref value), inPlace);
            }
            if (typeof(U) == typeof(uint))
            {
                return SubtractImplementation(Unsafe.As<U, uint>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ulong))
            {
                return SubtractImplementation(Unsafe.As<U, ulong>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ushort))
            {
                return SubtractImplementation(Unsafe.As<U, ushort>(ref value), inPlace);
            }
            throw new NotImplementedException();
        }
        public override DataFrameColumn Multiply(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Multiply<U>(U value, bool inPlace = false)
        {
            if (typeof(U) == typeof(bool))
            {
                return MultiplyImplementation(Unsafe.As<U, bool>(ref value), inPlace);
            }
            if (typeof(U) == typeof(byte))
            {
                return MultiplyImplementation(Unsafe.As<U, byte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(char))
            {
                return MultiplyImplementation(Unsafe.As<U, char>(ref value), inPlace);
            }
            if (typeof(U) == typeof(decimal))
            {
                return MultiplyImplementation(Unsafe.As<U, decimal>(ref value), inPlace);
            }
            if (typeof(U) == typeof(double))
            {
                return MultiplyImplementation(Unsafe.As<U, double>(ref value), inPlace);
            }
            if (typeof(U) == typeof(float))
            {
                return MultiplyImplementation(Unsafe.As<U, float>(ref value), inPlace);
            }
            if (typeof(U) == typeof(int))
            {
                return MultiplyImplementation(Unsafe.As<U, int>(ref value), inPlace);
            }
            if (typeof(U) == typeof(long))
            {
                return MultiplyImplementation(Unsafe.As<U, long>(ref value), inPlace);
            }
            if (typeof(U) == typeof(sbyte))
            {
                return MultiplyImplementation(Unsafe.As<U, sbyte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(short))
            {
                return MultiplyImplementation(Unsafe.As<U, short>(ref value), inPlace);
            }
            if (typeof(U) == typeof(uint))
            {
                return MultiplyImplementation(Unsafe.As<U, uint>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ulong))
            {
                return MultiplyImplementation(Unsafe.As<U, ulong>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ushort))
            {
                return MultiplyImplementation(Unsafe.As<U, ushort>(ref value), inPlace);
            }
            throw new NotImplementedException();
        }
        public override DataFrameColumn Divide(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return DivideImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Divide<U>(U value, bool inPlace = false)
        {
            if (typeof(U) == typeof(bool))
            {
                return DivideImplementation(Unsafe.As<U, bool>(ref value), inPlace);
            }
            if (typeof(U) == typeof(byte))
            {
                return DivideImplementation(Unsafe.As<U, byte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(char))
            {
                return DivideImplementation(Unsafe.As<U, char>(ref value), inPlace);
            }
            if (typeof(U) == typeof(decimal))
            {
                return DivideImplementation(Unsafe.As<U, decimal>(ref value), inPlace);
            }
            if (typeof(U) == typeof(double))
            {
                return DivideImplementation(Unsafe.As<U, double>(ref value), inPlace);
            }
            if (typeof(U) == typeof(float))
            {
                return DivideImplementation(Unsafe.As<U, float>(ref value), inPlace);
            }
            if (typeof(U) == typeof(int))
            {
                return DivideImplementation(Unsafe.As<U, int>(ref value), inPlace);
            }
            if (typeof(U) == typeof(long))
            {
                return DivideImplementation(Unsafe.As<U, long>(ref value), inPlace);
            }
            if (typeof(U) == typeof(sbyte))
            {
                return DivideImplementation(Unsafe.As<U, sbyte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(short))
            {
                return DivideImplementation(Unsafe.As<U, short>(ref value), inPlace);
            }
            if (typeof(U) == typeof(uint))
            {
                return DivideImplementation(Unsafe.As<U, uint>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ulong))
            {
                return DivideImplementation(Unsafe.As<U, ulong>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ushort))
            {
                return DivideImplementation(Unsafe.As<U, ushort>(ref value), inPlace);
            }
            throw new NotImplementedException();
        }
        public override DataFrameColumn Modulo(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<ushort>, inPlace);
                default:
                    throw new NotSupportedException();
            }
        }
        public override DataFrameColumn Modulo<U>(U value, bool inPlace = false)
        {
            if (typeof(U) == typeof(bool))
            {
                return ModuloImplementation(Unsafe.As<U, bool>(ref value), inPlace);
            }
            if (typeof(U) == typeof(byte))
            {
                return ModuloImplementation(Unsafe.As<U, byte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(char))
            {
                return ModuloImplementation(Unsafe.As<U, char>(ref value), inPlace);
            }
            if (typeof(U) == typeof(decimal))
            {
                return ModuloImplementation(Unsafe.As<U, decimal>(ref value), inPlace);
            }
            if (typeof(U) == typeof(double))
            {
                return ModuloImplementation(Unsafe.As<U, double>(ref value), inPlace);
            }
            if (typeof(U) == typeof(float))
            {
                return ModuloImplementation(Unsafe.As<U, float>(ref value), inPlace);
            }
            if (typeof(U) == typeof(int))
            {
                return ModuloImplementation(Unsafe.As<U, int>(ref value), inPlace);
            }
            if (typeof(U) == typeof(long))
            {
                return ModuloImplementation(Unsafe.As<U, long>(ref value), inPlace);
            }
            if (typeof(U) == typeof(sbyte))
            {
                return ModuloImplementation(Unsafe.As<U, sbyte>(ref value), inPlace);
            }
            if (typeof(U) == typeof(short))
            {
                return ModuloImplementation(Unsafe.As<U, short>(ref value), inPlace);
            }
            if (typeof(U) == typeof(uint))
            {
                return ModuloImplementation(Unsafe.As<U, uint>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ulong))
            {
                return ModuloImplementation(Unsafe.As<U, ulong>(ref value), inPlace);
            }
            if (typeof(U) == typeof(ushort))
            {
                return ModuloImplementation(Unsafe.As<U, ushort>(ref value), inPlace);
            }
            throw new NotImplementedException();
        }
        public override DataFrameColumn And(DataFrameColumn column, bool inPlace = false)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return AndImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
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
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return OrImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
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
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return XorImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
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
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseEquals<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseEqualsImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals(DataFrameColumn column)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseNotEqualsImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqual(DataFrameColumn column)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqual<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseGreaterThanOrEqualImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqual(DataFrameColumn column)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqual<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseLessThanOrEqualImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThan(DataFrameColumn column)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseGreaterThan<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseGreaterThanImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThan(DataFrameColumn column)
        {
            switch (column)
            {
                case PrimitiveDataFrameColumn<bool> boolColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case PrimitiveDataFrameColumn<byte> byteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case PrimitiveDataFrameColumn<char> charColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case PrimitiveDataFrameColumn<decimal> decimalColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case PrimitiveDataFrameColumn<double> doubleColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case PrimitiveDataFrameColumn<float> floatColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case PrimitiveDataFrameColumn<int> intColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case PrimitiveDataFrameColumn<long> longColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case PrimitiveDataFrameColumn<sbyte> sbyteColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case PrimitiveDataFrameColumn<short> shortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case PrimitiveDataFrameColumn<uint> uintColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case PrimitiveDataFrameColumn<ulong> ulongColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case PrimitiveDataFrameColumn<ushort> ushortColumn:
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<ushort>);
                default:
                    throw new NotSupportedException();
            }
        }
        public override PrimitiveDataFrameColumn<bool> ElementwiseLessThan<U>(U value)
        {
            switch (typeof(U))
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, bool>(ref value));
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, byte>(ref value));
                case Type charType when charType == typeof(char):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, char>(ref value));
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, decimal>(ref value));
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, double>(ref value));
                case Type floatType when floatType == typeof(float):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, float>(ref value));
                case Type intType when intType == typeof(int):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, int>(ref value));
                case Type longType when longType == typeof(long):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, long>(ref value));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, sbyte>(ref value));
                case Type shortType when shortType == typeof(short):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, short>(ref value));
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, uint>(ref value));
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, ulong>(ref value));
                case Type ushortType when ushortType == typeof(ushort):
                    return ElementwiseLessThanImplementation(Unsafe.As<U, ushort>(ref value));
                default:
                    throw new NotImplementedException();
            }
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
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Add((column as PrimitiveDataFrameColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Add(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn AddImplementation<U>(U value, bool inPlace)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(value);
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
                            decimalColumn._columnContainer.Add(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Add(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
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
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Subtract((column as PrimitiveDataFrameColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Subtract(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn SubtractImplementation<U>(U value, bool inPlace)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(value);
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
                            decimalColumn._columnContainer.Subtract(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Subtract(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
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
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Multiply((column as PrimitiveDataFrameColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Multiply(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn MultiplyImplementation<U>(U value, bool inPlace)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(value);
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
                            decimalColumn._columnContainer.Multiply(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Multiply(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
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
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Divide((column as PrimitiveDataFrameColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Divide(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn DivideImplementation<U>(U value, bool inPlace)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(value);
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
                            decimalColumn._columnContainer.Divide(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Divide(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
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
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(column._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedColumnValueType, typeof(T)), nameof(column));
                        }
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.Modulo((column as PrimitiveDataFrameColumn<decimal>)._columnContainer);
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Modulo(column.CloneAsDoubleColumn()._columnContainer);
                            return doubleColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal DataFrameColumn ModuloImplementation<U>(U value, bool inPlace)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(value);
                        return newColumn;
                    }
                    else 
                    {
                        if (inPlace)
                        {
                            throw new ArgumentException(string.Format(Strings.MismatchedValueType, typeof(T)), nameof(value));
                        }
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<U> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(value);
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
                            decimalColumn._columnContainer.Modulo(DecimalConverter<U>.Instance.GetDecimal(value));
                            return decimalColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.Modulo(DoubleConverter<U>.Instance.GetDouble(value));
                            return doubleColumn;
                        }
                    }
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
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.And(value);
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
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Or(value);
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
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<U> typedColumn = this as PrimitiveDataFrameColumn<U>;
                    PrimitiveDataFrameColumn<U> retColumn = inPlace ? typedColumn : typedColumn.Clone();
                    retColumn._columnContainer.Xor(value);
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseEquals((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseEqualsImplementation<U>(U value)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<U>)._columnContainer.ElementwiseEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseNotEquals((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseNotEquals(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseNotEqualsImplementation<U>(U value)
            where U : unmanaged
        {
            switch (typeof(T))
            {
                case Type boolType when boolType == typeof(bool):
                    if (typeof(U) != typeof(bool))
                    {
                        throw new NotSupportedException();
                    }
                    PrimitiveDataFrameColumn<bool> retColumn = CloneAsBoolColumn();
                    (this as PrimitiveDataFrameColumn<U>)._columnContainer.ElementwiseNotEquals(value, retColumn._columnContainer);
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
                        primitiveColumn._columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseNotEquals(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseNotEquals(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanOrEqualImplementation<U>(U value)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThanOrEqual((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThanOrEqual(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanOrEqualImplementation<U>(U value)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThanOrEqual(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThanOrEqual(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThan((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseGreaterThanImplementation<U>(U value)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseGreaterThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseGreaterThan(DoubleConverter<U>.Instance.GetDouble(value), newColumn._columnContainer);
                            return newColumn;
                        }
                    }
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
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(column._columnContainer, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThan((column as PrimitiveDataFrameColumn<decimal>)._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
                            doubleColumn._columnContainer.ElementwiseLessThan(column.CloneAsDoubleColumn()._columnContainer, newColumn._columnContainer);
                            return newColumn;
                        }
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        internal PrimitiveDataFrameColumn<bool> ElementwiseLessThanImplementation<U>(U value)
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
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
                        PrimitiveDataFrameColumn<U> primitiveColumn = this as PrimitiveDataFrameColumn<U>;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(value, newColumn._columnContainer);
                        return newColumn;
                    }
                    else 
                    {
                        if (typeof(U) == typeof(decimal))
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<decimal> decimalColumn = CloneAsDecimalColumn();
                            decimalColumn._columnContainer.ElementwiseLessThan(DecimalConverter<U>.Instance.GetDecimal(value), newColumn._columnContainer);
                            return newColumn;
                        }
                        else
                        {
                            PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                            PrimitiveDataFrameColumn<double> doubleColumn = CloneAsDoubleColumn();
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
