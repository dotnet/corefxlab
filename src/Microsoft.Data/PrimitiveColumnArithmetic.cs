

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumnArithmetic.tt. Do not modify directly

using System;

namespace Microsoft.Data
{
    internal interface IPrimitiveColumnArithmetic<T>
        where T : struct
    {
       void Add(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Add(PrimitiveColumnContainer<T> column, T scalar);
       void Subtract(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Subtract(PrimitiveColumnContainer<T> column, T scalar);
       void Multiply(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Multiply(PrimitiveColumnContainer<T> column, T scalar);
       void Divide(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Divide(PrimitiveColumnContainer<T> column, T scalar);
       void Modulo(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Modulo(PrimitiveColumnContainer<T> column, T scalar);
       void And(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void And(PrimitiveColumnContainer<T> column, T scalar);
       void Or(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Or(PrimitiveColumnContainer<T> column, T scalar);
       void Xor(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void Xor(PrimitiveColumnContainer<T> column, T scalar);
       void LeftShift(PrimitiveColumnContainer<T> column, int value);
       void RightShift(PrimitiveColumnContainer<T> column, int value);
       void Equals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void Equals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void NotEquals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void NotEquals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void GreaterThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void GreaterThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void LessThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void LessThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void GreaterThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void GreaterThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void LessThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void LessThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
    }

    internal static class PrimitiveColumnArithmetic<T>
        where T : struct
    {
        public static IPrimitiveColumnArithmetic<T> Instance { get; } = PrimitiveColumnArithmetic.GetArithmetic<T>();
    }

    internal static class PrimitiveColumnArithmetic
    {
        public static IPrimitiveColumnArithmetic<T> GetArithmetic<T>()
            where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return (IPrimitiveColumnArithmetic<T>)new BoolArithmetic();
            }
            else if (typeof(T) == typeof(byte))
            {
                return (IPrimitiveColumnArithmetic<T>)new ByteArithmetic();
            }
            else if (typeof(T) == typeof(char))
            {
                return (IPrimitiveColumnArithmetic<T>)new CharArithmetic();
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (IPrimitiveColumnArithmetic<T>)new DecimalArithmetic();
            }
            else if (typeof(T) == typeof(double))
            {
                return (IPrimitiveColumnArithmetic<T>)new DoubleArithmetic();
            }
            else if (typeof(T) == typeof(float))
            {
                return (IPrimitiveColumnArithmetic<T>)new FloatArithmetic();
            }
            else if (typeof(T) == typeof(int))
            {
                return (IPrimitiveColumnArithmetic<T>)new IntArithmetic();
            }
            else if (typeof(T) == typeof(long))
            {
                return (IPrimitiveColumnArithmetic<T>)new LongArithmetic();
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (IPrimitiveColumnArithmetic<T>)new SByteArithmetic();
            }
            else if (typeof(T) == typeof(short))
            {
                return (IPrimitiveColumnArithmetic<T>)new ShortArithmetic();
            }
            else if (typeof(T) == typeof(uint))
            {
                return (IPrimitiveColumnArithmetic<T>)new UIntArithmetic();
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (IPrimitiveColumnArithmetic<T>)new ULongArithmetic();
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (IPrimitiveColumnArithmetic<T>)new UShortArithmetic();
            }
            throw new NotSupportedException();
        }
    }

    internal class BoolArithmetic : IPrimitiveColumnArithmetic<bool>
    {
        public void Add(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Add(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Subtract(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Subtract(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Multiply(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Multiply(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Divide(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Divide(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Modulo(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Modulo(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (bool)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<bool> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveColumnContainer<bool> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : IPrimitiveColumnArithmetic<byte>
    {
        public void Add(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<byte> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<byte> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (byte)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class CharArithmetic : IPrimitiveColumnArithmetic<char>
    {
        public void Add(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<char> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<char> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (char)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class DecimalArithmetic : IPrimitiveColumnArithmetic<decimal>
    {
        public void Add(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (decimal)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveColumnContainer<decimal> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveColumnContainer<decimal> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class DoubleArithmetic : IPrimitiveColumnArithmetic<double>
    {
        public void Add(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (double)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveColumnContainer<double> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveColumnContainer<double> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class FloatArithmetic : IPrimitiveColumnArithmetic<float>
    {
        public void Add(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (float)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveColumnContainer<float> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveColumnContainer<float> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class IntArithmetic : IPrimitiveColumnArithmetic<int>
    {
        public void Add(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<int> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<int> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (int)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class LongArithmetic : IPrimitiveColumnArithmetic<long>
    {
        public void Add(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<long> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<long> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (long)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class SByteArithmetic : IPrimitiveColumnArithmetic<sbyte>
    {
        public void Add(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<sbyte> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<sbyte> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (sbyte)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class ShortArithmetic : IPrimitiveColumnArithmetic<short>
    {
        public void Add(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<short> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<short> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (short)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class UIntArithmetic : IPrimitiveColumnArithmetic<uint>
    {
        public void Add(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<uint> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<uint> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (uint)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class ULongArithmetic : IPrimitiveColumnArithmetic<ulong>
    {
        public void Add(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<ulong> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<ulong> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ulong)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }
    internal class UShortArithmetic : IPrimitiveColumnArithmetic<ushort>
    {
        public void Add(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] + right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] - right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] * right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] / right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] % right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] & right.Buffers[bb].Span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] | right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] ^ right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<ushort> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<ushort> column, int value)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer.Span[i] = (ushort)(buffer.Span[i] >> value);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == right.Buffers[bb].Span[i]);
                }
            }
        }
        public void Equals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] == scalar);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != right.Buffers[bb].Span[i]);
                }
            }
        }
        public void NotEquals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] != scalar);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] >= scalar);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] <= scalar);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > right.Buffers[bb].Span[i]);
                }
            }
        }
        public void GreaterThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] > scalar);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < left.Buffers.Count; bb++)
            {
                var buffer = left.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < right.Buffers[bb].Span[i]);
                }
            }
        }
        public void LessThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int bb = 0 ; bb < column.Buffers.Count; bb++)
            {
                var buffer = column.Buffers[bb];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret[i] = (buffer.Span[i] < scalar);
                }
            }
        }
    }




}
