
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from BaseColumn.BinaryOperators.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    public abstract partial class BaseColumn
    {
        public static BaseColumn operator +(BaseColumn left, BaseColumn right)
        {
            return left.Add(right);
        }

        public static BaseColumn operator +(BaseColumn column, bool value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(bool value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, byte value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(byte value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, char value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(char value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, decimal value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(decimal value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, double value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(double value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, float value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(float value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, int value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(int value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, long value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(long value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, sbyte value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(sbyte value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, short value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(short value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, uint value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(uint value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, ulong value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(ulong value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator +(BaseColumn column, ushort value)
        {
            return column.Add(value, inPlace: false);
        }

        public static BaseColumn operator +(ushort value, BaseColumn column)
        {
            return column.Add(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn left, BaseColumn right)
        {
            return left.Subtract(right);
        }

        public static BaseColumn operator -(BaseColumn column, bool value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(bool value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, byte value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(byte value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, char value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(char value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, decimal value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(decimal value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, double value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(double value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, float value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(float value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, int value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(int value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, long value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(long value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, sbyte value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(sbyte value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, short value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(short value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, uint value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(uint value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, ulong value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(ulong value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator -(BaseColumn column, ushort value)
        {
            return column.Subtract(value, inPlace: false);
        }

        public static BaseColumn operator -(ushort value, BaseColumn column)
        {
            return column.Subtract(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn left, BaseColumn right)
        {
            return left.Multiply(right);
        }

        public static BaseColumn operator *(BaseColumn column, bool value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(bool value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, byte value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(byte value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, char value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(char value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, decimal value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(decimal value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, double value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(double value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, float value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(float value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, int value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(int value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, long value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(long value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, sbyte value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(sbyte value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, short value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(short value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, uint value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(uint value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, ulong value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(ulong value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator *(BaseColumn column, ushort value)
        {
            return column.Multiply(value, inPlace: false);
        }

        public static BaseColumn operator *(ushort value, BaseColumn column)
        {
            return column.Multiply(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn left, BaseColumn right)
        {
            return left.Divide(right);
        }

        public static BaseColumn operator /(BaseColumn column, bool value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(bool value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, byte value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(byte value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, char value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(char value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, decimal value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(decimal value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, double value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(double value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, float value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(float value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, int value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(int value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, long value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(long value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, sbyte value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(sbyte value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, short value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(short value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, uint value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(uint value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, ulong value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(ulong value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator /(BaseColumn column, ushort value)
        {
            return column.Divide(value, inPlace: false);
        }

        public static BaseColumn operator /(ushort value, BaseColumn column)
        {
            return column.Divide(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn left, BaseColumn right)
        {
            return left.Modulo(right);
        }

        public static BaseColumn operator %(BaseColumn column, bool value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(bool value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, byte value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(byte value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, char value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(char value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, decimal value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(decimal value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, double value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(double value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, float value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(float value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, int value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(int value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, long value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(long value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, sbyte value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(sbyte value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, short value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(short value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, uint value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(uint value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, ulong value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(ulong value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator %(BaseColumn column, ushort value)
        {
            return column.Modulo(value, inPlace: false);
        }

        public static BaseColumn operator %(ushort value, BaseColumn column)
        {
            return column.Modulo(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn left, BaseColumn right)
        {
            return left.And(right);
        }

        public static BaseColumn operator &(BaseColumn column, bool value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(bool value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, byte value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(byte value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, char value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(char value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, decimal value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(decimal value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, double value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(double value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, float value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(float value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, int value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(int value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, long value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(long value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, sbyte value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(sbyte value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, short value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(short value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, uint value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(uint value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, ulong value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(ulong value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator &(BaseColumn column, ushort value)
        {
            return column.And(value, inPlace: false);
        }

        public static BaseColumn operator &(ushort value, BaseColumn column)
        {
            return column.And(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn left, BaseColumn right)
        {
            return left.Or(right);
        }

        public static BaseColumn operator |(BaseColumn column, bool value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(bool value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, byte value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(byte value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, char value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(char value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, decimal value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(decimal value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, double value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(double value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, float value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(float value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, int value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(int value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, long value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(long value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, sbyte value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(sbyte value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, short value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(short value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, uint value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(uint value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, ulong value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(ulong value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator |(BaseColumn column, ushort value)
        {
            return column.Or(value, inPlace: false);
        }

        public static BaseColumn operator |(ushort value, BaseColumn column)
        {
            return column.Or(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn left, BaseColumn right)
        {
            return left.Xor(right);
        }

        public static BaseColumn operator ^(BaseColumn column, bool value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(bool value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, byte value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(byte value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, char value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(char value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, decimal value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(decimal value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, double value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(double value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, float value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(float value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, int value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(int value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, long value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(long value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, sbyte value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(sbyte value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, short value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(short value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, uint value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(uint value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, ulong value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(ulong value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator ^(BaseColumn column, ushort value)
        {
            return column.Xor(value, inPlace: false);
        }

        public static BaseColumn operator ^(ushort value, BaseColumn column)
        {
            return column.Xor(value, inPlace: false, reverseOrderOfOperations: true);
        }

        public static BaseColumn operator <<(BaseColumn column, int value)
        {
            return column.LeftShift(value);
        }

        public static BaseColumn operator >>(BaseColumn column, int value)
        {
            return column.RightShift(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn left, BaseColumn right)
        {
            return left.Equals(right);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, bool value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, byte value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, char value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, decimal value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, double value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, float value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, int value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, long value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, sbyte value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, short value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, uint value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, ulong value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator ==(BaseColumn column, ushort value)
        {
            return column.Equals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn left, BaseColumn right)
        {
            return left.NotEquals(right);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, bool value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, byte value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, char value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, decimal value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, double value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, float value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, int value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, long value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, sbyte value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, short value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, uint value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, ulong value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator !=(BaseColumn column, ushort value)
        {
            return column.NotEquals(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn left, BaseColumn right)
        {
            return left.GreaterThanOrEqual(right);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, bool value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, byte value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, char value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, decimal value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, double value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, float value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, int value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, long value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, sbyte value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, short value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, uint value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, ulong value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >=(BaseColumn column, ushort value)
        {
            return column.GreaterThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn left, BaseColumn right)
        {
            return left.LessThanOrEqual(right);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, bool value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, byte value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, char value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, decimal value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, double value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, float value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, int value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, long value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, sbyte value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, short value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, uint value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, ulong value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator <=(BaseColumn column, ushort value)
        {
            return column.LessThanOrEqual(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn left, BaseColumn right)
        {
            return left.GreaterThan(right);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, bool value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, byte value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, char value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, decimal value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, double value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, float value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, int value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, long value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, sbyte value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, short value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, uint value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, ulong value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator >(BaseColumn column, ushort value)
        {
            return column.GreaterThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn left, BaseColumn right)
        {
            return left.LessThan(right);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, bool value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, byte value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, char value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, decimal value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, double value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, float value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, int value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, long value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, sbyte value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, short value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, uint value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, ulong value)
        {
            return column.LessThan(value);
        }

        public static PrimitiveColumn<bool> operator <(BaseColumn column, ushort value)
        {
            return column.LessThan(value);
        }

    }
}
