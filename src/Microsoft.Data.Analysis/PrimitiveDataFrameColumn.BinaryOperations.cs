
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return AddImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return AddImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return AddImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return AddImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return AddImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return AddImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return AddImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return AddImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return AddImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return AddImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return AddImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return AddImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return SubtractImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return MultiplyImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return DivideImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return ModuloImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return AndImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return AndImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return AndImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return AndImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return AndImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return AndImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return AndImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return AndImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return AndImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return AndImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return AndImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return AndImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return OrImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return OrImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return OrImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return OrImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return OrImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return OrImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return OrImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return OrImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return OrImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return OrImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return OrImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return OrImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return XorImplementation(column as PrimitiveDataFrameColumn<bool>, inPlace);
                case Type byteType when byteType == typeof(byte):
                    return XorImplementation(column as PrimitiveDataFrameColumn<byte>, inPlace);
                case Type charType when charType == typeof(char):
                    return XorImplementation(column as PrimitiveDataFrameColumn<char>, inPlace);
                case Type decimalType when decimalType == typeof(decimal):
                    return XorImplementation(column as PrimitiveDataFrameColumn<decimal>, inPlace);
                case Type doubleType when doubleType == typeof(double):
                    return XorImplementation(column as PrimitiveDataFrameColumn<double>, inPlace);
                case Type floatType when floatType == typeof(float):
                    return XorImplementation(column as PrimitiveDataFrameColumn<float>, inPlace);
                case Type intType when intType == typeof(int):
                    return XorImplementation(column as PrimitiveDataFrameColumn<int>, inPlace);
                case Type longType when longType == typeof(long):
                    return XorImplementation(column as PrimitiveDataFrameColumn<long>, inPlace);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return XorImplementation(column as PrimitiveDataFrameColumn<sbyte>, inPlace);
                case Type shortType when shortType == typeof(short):
                    return XorImplementation(column as PrimitiveDataFrameColumn<short>, inPlace);
                case Type uintType when uintType == typeof(uint):
                    return XorImplementation(column as PrimitiveDataFrameColumn<uint>, inPlace);
                case Type ulongType when ulongType == typeof(ulong):
                    return XorImplementation(column as PrimitiveDataFrameColumn<ulong>, inPlace);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseNotEqualsImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseGreaterThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseLessThanOrEqualImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseGreaterThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
            switch (column.DataType)
            {
                case Type boolType when boolType == typeof(bool):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<bool>);
                case Type byteType when byteType == typeof(byte):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<byte>);
                case Type charType when charType == typeof(char):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<char>);
                case Type decimalType when decimalType == typeof(decimal):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<decimal>);
                case Type doubleType when doubleType == typeof(double):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<double>);
                case Type floatType when floatType == typeof(float):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<float>);
                case Type intType when intType == typeof(int):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<int>);
                case Type longType when longType == typeof(long):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<long>);
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<sbyte>);
                case Type shortType when shortType == typeof(short):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<short>);
                case Type uintType when uintType == typeof(uint):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<uint>);
                case Type ulongType when ulongType == typeof(ulong):
                    return ElementwiseLessThanImplementation(column as PrimitiveDataFrameColumn<ulong>);
                case Type ushortType when ushortType == typeof(ushort):
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Add(Unsafe.As<U, T>(ref value));
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Subtract(Unsafe.As<U, T>(ref value));
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Multiply(Unsafe.As<U, T>(ref value));
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Divide(Unsafe.As<U, T>(ref value));
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<T> newColumn = inPlace ? primitiveColumn : primitiveColumn.Clone();
                        newColumn._columnContainer.Modulo(Unsafe.As<U, T>(ref value));
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseNotEquals(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThanOrEqual(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseGreaterThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
                        PrimitiveDataFrameColumn<T> primitiveColumn = this;
                        PrimitiveDataFrameColumn<bool> newColumn = CloneAsBoolColumn();
                        primitiveColumn._columnContainer.ElementwiseLessThan(Unsafe.As<U, T>(ref value), newColumn._columnContainer);
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
