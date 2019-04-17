

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Data
{
    internal interface IPrimitiveDataFrameColumnArithmetic<T>
        where T : struct
    {
       void Add(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Add(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Subtract(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Subtract(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Multiply(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Multiply(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Divide(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Divide(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Modulo(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Modulo(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void And(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void And(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Or(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Or(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void Xor(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right);
       void Xor(PrimitiveDataFrameColumnContainer<T> column, T scalar);
       void LeftShift(PrimitiveDataFrameColumnContainer<T> column, int value);
       void RightShift(PrimitiveDataFrameColumnContainer<T> column, int value);
       void Equals(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void Equals(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
       void NotEquals(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void NotEquals(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
       void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
       void LessThanOrEqual(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void LessThanOrEqual(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
       void GreaterThan(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void GreaterThan(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
       void LessThan(PrimitiveDataFrameColumnContainer<T> left, PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret);
       void LessThan(PrimitiveDataFrameColumnContainer<T> column, T scalar, PrimitiveDataFrameColumnContainer<bool> ret);
    }

    internal static class PrimitiveDataFrameColumnArithmetic<T>
        where T : struct
    {
        public static IPrimitiveDataFrameColumnArithmetic<T> Instance => PrimitiveDataFrameColumnArithmetic.GetArithmetic<T>();
    }

    internal static class PrimitiveDataFrameColumnArithmetic
    {
        public static IPrimitiveDataFrameColumnArithmetic<T> GetArithmetic<T>()
            where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new BoolArithmetic();
            }
            else if (typeof(T) == typeof(byte))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new ByteArithmetic();
            }
            else if (typeof(T) == typeof(char))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new CharArithmetic();
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new DecimalArithmetic();
            }
            else if (typeof(T) == typeof(double))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new DoubleArithmetic();
            }
            else if (typeof(T) == typeof(float))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new FloatArithmetic();
            }
            else if (typeof(T) == typeof(int))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new IntArithmetic();
            }
            else if (typeof(T) == typeof(long))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new LongArithmetic();
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new SByteArithmetic();
            }
            else if (typeof(T) == typeof(short))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new ShortArithmetic();
            }
            else if (typeof(T) == typeof(uint))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new UIntArithmetic();
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new ULongArithmetic();
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (IPrimitiveDataFrameColumnArithmetic<T>)new UShortArithmetic();
            }
            return null;
        }
    }

    internal class BoolArithmetic : IPrimitiveDataFrameColumnArithmetic<bool>
    {
        public void Add(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Add(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Subtract(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Subtract(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Multiply(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Multiply(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Divide(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Divide(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void Modulo(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void Modulo(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
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
        public void And(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<bool> column, bool scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<bool> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveDataFrameColumnContainer<bool> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThan(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void GreaterThan(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThan(PrimitiveDataFrameColumnContainer<bool> left, PrimitiveDataFrameColumnContainer<bool> right, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void LessThan(PrimitiveDataFrameColumnContainer<bool> column, bool scalar, PrimitiveDataFrameColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : IPrimitiveDataFrameColumnArithmetic<byte>
    {
        public void Add(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void And(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<byte> column, byte scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<byte> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<byte> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<byte> left, PrimitiveDataFrameColumnContainer<byte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<byte> column, byte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class CharArithmetic : IPrimitiveDataFrameColumnArithmetic<char>
    {
        public void Add(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void And(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<char> column, char scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<char> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<char> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<char> left, PrimitiveDataFrameColumnContainer<char> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<char> column, char scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class DecimalArithmetic : IPrimitiveDataFrameColumnArithmetic<decimal>
    {
        public void Add(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveDataFrameColumnContainer<decimal> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveDataFrameColumnContainer<decimal> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<decimal> left, PrimitiveDataFrameColumnContainer<decimal> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<decimal> column, decimal scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class DoubleArithmetic : IPrimitiveDataFrameColumnArithmetic<double>
    {
        public void Add(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<double> column, double scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<double> column, double scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<double> column, double scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<double> column, double scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<double> column, double scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveDataFrameColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveDataFrameColumnContainer<double> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveDataFrameColumnContainer<double> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<double> left, PrimitiveDataFrameColumnContainer<double> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<double> column, double scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class FloatArithmetic : IPrimitiveDataFrameColumnArithmetic<float>
    {
        public void Add(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<float> column, float scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<float> column, float scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<float> column, float scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<float> column, float scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<float> column, float scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void And(PrimitiveDataFrameColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void Or(PrimitiveDataFrameColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void Xor(PrimitiveDataFrameColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(PrimitiveDataFrameColumnContainer<float> column, int value)
        {
            throw new NotSupportedException();
        }
        public void RightShift(PrimitiveDataFrameColumnContainer<float> column, int value)
        {
            throw new NotSupportedException();
        }
        public void Equals(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<float> left, PrimitiveDataFrameColumnContainer<float> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<float> column, float scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class IntArithmetic : IPrimitiveDataFrameColumnArithmetic<int>
    {
        public void Add(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void And(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<int> column, int scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<int> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<int> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<int> left, PrimitiveDataFrameColumnContainer<int> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<int> column, int scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class LongArithmetic : IPrimitiveDataFrameColumnArithmetic<long>
    {
        public void Add(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void And(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<long> column, long scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<long> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<long> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<long> left, PrimitiveDataFrameColumnContainer<long> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<long> column, long scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class SByteArithmetic : IPrimitiveDataFrameColumnArithmetic<sbyte>
    {
        public void Add(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void And(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<sbyte> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<sbyte> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<sbyte> left, PrimitiveDataFrameColumnContainer<sbyte> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<sbyte> column, sbyte scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class ShortArithmetic : IPrimitiveDataFrameColumnArithmetic<short>
    {
        public void Add(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void And(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<short> column, short scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<short> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<short> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<short> left, PrimitiveDataFrameColumnContainer<short> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<short> column, short scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class UIntArithmetic : IPrimitiveDataFrameColumnArithmetic<uint>
    {
        public void Add(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void And(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<uint> column, uint scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<uint> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<uint> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<uint> left, PrimitiveDataFrameColumnContainer<uint> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<uint> column, uint scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class ULongArithmetic : IPrimitiveDataFrameColumnArithmetic<ulong>
    {
        public void Add(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void And(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<ulong> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<ulong> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<ulong> left, PrimitiveDataFrameColumnContainer<ulong> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<ulong> column, ulong scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
    internal class UShortArithmetic : IPrimitiveDataFrameColumnArithmetic<ushort>
    {
        public void Add(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Add(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Subtract(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Multiply(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Divide(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Divide(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Modulo(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void And(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void And(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Or(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Or(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void Xor(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right)
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
        public void Xor(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar)
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
        public void LeftShift(PrimitiveDataFrameColumnContainer<ushort> column, int value)
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
        public void RightShift(PrimitiveDataFrameColumnContainer<ushort> column, int value)
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
        public void Equals(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void Equals(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void NotEquals(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThanOrEqual(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void GreaterThan(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<ushort> left, PrimitiveDataFrameColumnContainer<ushort> right, PrimitiveDataFrameColumnContainer<bool> ret)
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
        public void LessThan(PrimitiveDataFrameColumnContainer<ushort> column, ushort scalar, PrimitiveDataFrameColumnContainer<bool> ret)
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
