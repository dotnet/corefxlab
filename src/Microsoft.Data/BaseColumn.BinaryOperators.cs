
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
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, byte value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, char value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, decimal value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, double value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, float value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, int value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, long value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, sbyte value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, short value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, uint value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, ulong value)
        {
            return column.Add(value);
        }
        public static BaseColumn operator +(BaseColumn column, ushort value)
        {
            return column.Add(value);
        }

        public static BaseColumn operator -(BaseColumn left, BaseColumn right)
        {
            return left.Subtract(right);
        }

        public static BaseColumn operator -(BaseColumn column, bool value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, byte value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, char value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, decimal value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, double value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, float value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, int value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, long value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, sbyte value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, short value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, uint value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, ulong value)
        {
            return column.Subtract(value);
        }
        public static BaseColumn operator -(BaseColumn column, ushort value)
        {
            return column.Subtract(value);
        }

        public static BaseColumn operator *(BaseColumn left, BaseColumn right)
        {
            return left.Multiply(right);
        }

        public static BaseColumn operator *(BaseColumn column, bool value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, byte value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, char value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, decimal value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, double value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, float value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, int value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, long value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, sbyte value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, short value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, uint value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, ulong value)
        {
            return column.Multiply(value);
        }
        public static BaseColumn operator *(BaseColumn column, ushort value)
        {
            return column.Multiply(value);
        }

        public static BaseColumn operator /(BaseColumn left, BaseColumn right)
        {
            return left.Divide(right);
        }

        public static BaseColumn operator /(BaseColumn column, bool value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, byte value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, char value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, decimal value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, double value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, float value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, int value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, long value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, sbyte value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, short value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, uint value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, ulong value)
        {
            return column.Divide(value);
        }
        public static BaseColumn operator /(BaseColumn column, ushort value)
        {
            return column.Divide(value);
        }

        public static BaseColumn operator %(BaseColumn left, BaseColumn right)
        {
            return left.Modulo(right);
        }

        public static BaseColumn operator %(BaseColumn column, bool value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, byte value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, char value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, decimal value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, double value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, float value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, int value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, long value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, sbyte value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, short value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, uint value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, ulong value)
        {
            return column.Modulo(value);
        }
        public static BaseColumn operator %(BaseColumn column, ushort value)
        {
            return column.Modulo(value);
        }

        public static BaseColumn operator &(BaseColumn left, BaseColumn right)
        {
            return left.And(right);
        }

        public static BaseColumn operator &(BaseColumn column, bool value)
        {
            return column.And(value);
        }
        public static BaseColumn operator |(BaseColumn left, BaseColumn right)
        {
            return left.Or(right);
        }

        public static BaseColumn operator |(BaseColumn column, bool value)
        {
            return column.Or(value);
        }
        public static BaseColumn operator ^(BaseColumn left, BaseColumn right)
        {
            return left.Xor(right);
        }

        public static BaseColumn operator ^(BaseColumn column, bool value)
        {
            return column.Xor(value);
        }
        public static BaseColumn operator <<(BaseColumn column, int value)
        {
            return column.LeftShift(value);
        }

        public static BaseColumn operator >>(BaseColumn column, int value)
        {
            return column.RightShift(value);
        }

    }
}
