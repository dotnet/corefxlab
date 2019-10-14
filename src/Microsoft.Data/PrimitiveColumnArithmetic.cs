

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
       void PairwiseEquals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseEquals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void PairwiseNotEquals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseNotEquals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void PairwiseLessThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseLessThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void PairwiseGreaterThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseGreaterThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void PairwiseLessThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void PairwiseLessThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
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
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (bool)(span[i] ^ scalar);
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
        public void PairwiseEquals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : IPrimitiveColumnArithmetic<byte>
    {
        public void Add(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<byte> column, byte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<byte> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<byte> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class CharArithmetic : IPrimitiveColumnArithmetic<char>
    {
        public void Add(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<char> column, char scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<char> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<char> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (char)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class DecimalArithmetic : IPrimitiveColumnArithmetic<decimal>
    {
        public void Add(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (decimal)(span[i] % scalar);
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
        public void PairwiseEquals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class DoubleArithmetic : IPrimitiveColumnArithmetic<double>
    {
        public void Add(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<double> column, double scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (double)(span[i] % scalar);
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
        public void PairwiseEquals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class FloatArithmetic : IPrimitiveColumnArithmetic<float>
    {
        public void Add(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<float> column, float scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (float)(span[i] % scalar);
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
        public void PairwiseEquals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class IntArithmetic : IPrimitiveColumnArithmetic<int>
    {
        public void Add(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<int> column, int scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<int> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<int> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (int)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class LongArithmetic : IPrimitiveColumnArithmetic<long>
    {
        public void Add(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<long> column, long scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<long> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<long> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (long)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class SByteArithmetic : IPrimitiveColumnArithmetic<sbyte>
    {
        public void Add(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<sbyte> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<sbyte> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (sbyte)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class ShortArithmetic : IPrimitiveColumnArithmetic<short>
    {
        public void Add(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<short> column, short scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<short> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<short> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (short)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class UIntArithmetic : IPrimitiveColumnArithmetic<uint>
    {
        public void Add(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<uint> column, uint scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<uint> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<uint> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (uint)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class ULongArithmetic : IPrimitiveColumnArithmetic<ulong>
    {
        public void Add(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ulong> column, ulong scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<ulong> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<ulong> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ulong)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }
    internal class UShortArithmetic : IPrimitiveColumnArithmetic<ushort>
    {
        public void Add(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] + otherSpan[i]);
                }
            }
        }
        public void Add(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] + scalar);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] - otherSpan[i]);
                }
            }
        }
        public void Subtract(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] - scalar);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] * otherSpan[i]);
                }
            }
        }
        public void Multiply(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] * scalar);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] / otherSpan[i]);
                }
            }
        }
        public void Divide(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] / scalar);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] % otherSpan[i]);
                }
            }
        }
        public void Modulo(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] % scalar);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] & otherSpan[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] & scalar);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] | otherSpan[i]);
                }
            }
        }
        public void Or(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] | scalar);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] ^ otherSpan[i]);
                }
            }
        }
        public void Xor(PrimitiveColumnContainer<ushort> column, ushort scalar)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] ^ scalar);
                }
            }
        }
        public void LeftShift(PrimitiveColumnContainer<ushort> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] << value);
                }
            }
        }
        public void RightShift(PrimitiveColumnContainer<ushort> column, int value)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (ushort)(span[i] >> value);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == otherSpan[i]);
                }
            }
        }
        public void PairwiseEquals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] == scalar);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != otherSpan[i]);
                }
            }
        }
        public void PairwiseNotEquals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] != scalar);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] >= scalar);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] <= scalar);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > otherSpan[i]);
                }
            }
        }
        public void PairwiseGreaterThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] > scalar);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < left.Buffers.Count; b++)
            {
                var buffer = left.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                left.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                var otherSpan = right.Buffers[b].ReadOnlySpan;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < otherSpan[i]);
                }
            }
        }
        public void PairwiseLessThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    ret[i] = (span[i] < scalar);
                }
            }
        }
    }




}
