
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameColumn.BinaryOperators.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    public abstract partial class DataFrameColumn
    {
        public static DataFrameColumn operator +(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Add(right);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, bool value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(bool value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, byte value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(byte value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, char value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(char value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, decimal value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(decimal value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, double value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(double value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, float value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(float value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, int value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(int value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, long value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(long value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, sbyte value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(sbyte value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, short value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(short value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, uint value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(uint value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, ulong value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(ulong value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }

        public static DataFrameColumn operator +(DataFrameColumn column, ushort value)
        {
            return column.AddValue(value);
        }

        public static DataFrameColumn operator +(ushort value, DataFrameColumn column)
        {
            return column.ReverseAddValue(value);
        }


        public static DataFrameColumn operator -(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Subtract(right);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, bool value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(bool value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, byte value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(byte value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, char value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(char value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, decimal value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(decimal value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, double value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(double value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, float value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(float value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, int value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(int value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, long value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(long value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, sbyte value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(sbyte value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, short value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(short value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, uint value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(uint value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, ulong value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(ulong value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }

        public static DataFrameColumn operator -(DataFrameColumn column, ushort value)
        {
            return column.SubtractValue(value);
        }

        public static DataFrameColumn operator -(ushort value, DataFrameColumn column)
        {
            return column.ReverseSubtractValue(value);
        }


        public static DataFrameColumn operator *(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Multiply(right);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, bool value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(bool value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, byte value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(byte value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, char value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(char value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, decimal value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(decimal value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, double value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(double value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, float value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(float value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, int value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(int value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, long value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(long value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, sbyte value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(sbyte value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, short value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(short value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, uint value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(uint value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, ulong value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(ulong value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }

        public static DataFrameColumn operator *(DataFrameColumn column, ushort value)
        {
            return column.MultiplyValue(value);
        }

        public static DataFrameColumn operator *(ushort value, DataFrameColumn column)
        {
            return column.ReverseMultiplyValue(value);
        }


        public static DataFrameColumn operator /(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Divide(right);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, bool value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(bool value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, byte value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(byte value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, char value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(char value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, decimal value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(decimal value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, double value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(double value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, float value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(float value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, int value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(int value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, long value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(long value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, sbyte value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(sbyte value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, short value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(short value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, uint value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(uint value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, ulong value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(ulong value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }

        public static DataFrameColumn operator /(DataFrameColumn column, ushort value)
        {
            return column.DivideValue(value);
        }

        public static DataFrameColumn operator /(ushort value, DataFrameColumn column)
        {
            return column.ReverseDivideValue(value);
        }


        public static DataFrameColumn operator %(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Modulo(right);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, bool value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(bool value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, byte value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(byte value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, char value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(char value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, decimal value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(decimal value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, double value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(double value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, float value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(float value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, int value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(int value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, long value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(long value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, sbyte value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(sbyte value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, short value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(short value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, uint value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(uint value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, ulong value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(ulong value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }

        public static DataFrameColumn operator %(DataFrameColumn column, ushort value)
        {
            return column.ModuloValue(value);
        }

        public static DataFrameColumn operator %(ushort value, DataFrameColumn column)
        {
            return column.ReverseModuloValue(value);
        }


        public static DataFrameColumn operator &(DataFrameColumn left, DataFrameColumn right)
        {
            return left.And(right);
        }

        public static DataFrameColumn operator &(DataFrameColumn column, bool value)
        {
            return column.AndValue(value);
        }

        public static DataFrameColumn operator &(bool value, DataFrameColumn column)
        {
            return column.ReverseAndValue(value);
        }

        public static DataFrameColumn operator |(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Or(right);
        }

        public static DataFrameColumn operator |(DataFrameColumn column, bool value)
        {
            return column.OrValue(value);
        }

        public static DataFrameColumn operator |(bool value, DataFrameColumn column)
        {
            return column.ReverseOrValue(value);
        }

        public static DataFrameColumn operator ^(DataFrameColumn left, DataFrameColumn right)
        {
            return left.Xor(right);
        }

        public static DataFrameColumn operator ^(DataFrameColumn column, bool value)
        {
            return column.XorValue(value);
        }

        public static DataFrameColumn operator ^(bool value, DataFrameColumn column)
        {
            return column.ReverseXorValue(value);
        }

        public static DataFrameColumn operator <<(DataFrameColumn column, int value)
        {
            return column.LeftShift(value);
        }

        public static DataFrameColumn operator >>(DataFrameColumn column, int value)
        {
            return column.RightShift(value);
        }

    }
}
