

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveDataFrameColumnArithmetic.tt. Do not modify directly

using System;

namespace Microsoft.Data.Analysis
{
    internal interface IPrimitiveDataFrameColumnArithmetic<T>
        where T : struct
    {
       void Add(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void AddValue(PrimitiveColumnContainer<T> column, T scalar);
       void AddValue(T scalar, PrimitiveColumnContainer<T> column);
       void Subtract(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void SubtractValue(PrimitiveColumnContainer<T> column, T scalar);
       void SubtractValue(T scalar, PrimitiveColumnContainer<T> column);
       void Multiply(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void MultiplyValue(PrimitiveColumnContainer<T> column, T scalar);
       void MultiplyValue(T scalar, PrimitiveColumnContainer<T> column);
       void Divide(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void DivideValue(PrimitiveColumnContainer<T> column, T scalar);
       void DivideValue(T scalar, PrimitiveColumnContainer<T> column);
       void Modulo(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void ModuloValue(PrimitiveColumnContainer<T> column, T scalar);
       void ModuloValue(T scalar, PrimitiveColumnContainer<T> column);
       void And(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void AndValue(PrimitiveColumnContainer<T> column, T scalar);
       void AndValue(T scalar, PrimitiveColumnContainer<T> column);
       void Or(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void OrValue(PrimitiveColumnContainer<T> column, T scalar);
       void OrValue(T scalar, PrimitiveColumnContainer<T> column);
       void Xor(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right);
       void XorValue(PrimitiveColumnContainer<T> column, T scalar);
       void XorValue(T scalar, PrimitiveColumnContainer<T> column);
       void LeftShift(PrimitiveColumnContainer<T> column, int value);
       void RightShift(PrimitiveColumnContainer<T> column, int value);
       void ElementwiseEquals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueEquals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void ElementwiseNotEquals(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueNotEquals(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void ElementwiseGreaterThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueGreaterThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
       void ElementwiseLessThan(PrimitiveColumnContainer<T> left, PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret);
       void ElementwiseValueLessThan(PrimitiveColumnContainer<T> column, T scalar, PrimitiveColumnContainer<bool> ret);
    }

    internal static class PrimitiveDataFrameColumnArithmetic<T>
        where T : struct
    {
        public static IPrimitiveDataFrameColumnArithmetic<T> Instance { get; } = PrimitiveDataFrameColumnArithmetic.GetArithmetic<T>();
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
            throw new NotSupportedException();
        }
    }

    internal class BoolArithmetic : IPrimitiveDataFrameColumnArithmetic<bool>
    {
        public void Add(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void AddValue(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
 
        public void AddValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException(); 
        }
        public void Subtract(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void SubtractValue(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
 
        public void SubtractValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException(); 
        }
        public void Multiply(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void MultiplyValue(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
 
        public void MultiplyValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException(); 
        }
        public void Divide(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void DivideValue(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
 
        public void DivideValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException(); 
        }
        public void Modulo(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right)
        {
            throw new NotSupportedException();
        }
        public void ModuloValue(PrimitiveColumnContainer<bool> column, bool scalar)
        {
            throw new NotSupportedException();
        }
 
        public void ModuloValue(bool scalar, PrimitiveColumnContainer<bool> column)
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
        public void AndValue(PrimitiveColumnContainer<bool> column, bool scalar)
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
 
        public void AndValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (bool)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<bool> column, bool scalar)
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
 
        public void OrValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (bool)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<bool> column, bool scalar)
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
 
        public void XorValue(bool scalar, PrimitiveColumnContainer<bool> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<bool>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (bool)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseLessThan(PrimitiveColumnContainer<bool> left, PrimitiveColumnContainer<bool> right, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<bool> column, bool scalar, PrimitiveColumnContainer<bool> ret)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : IPrimitiveDataFrameColumnArithmetic<byte>
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
        public void AddValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void AddValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void SubtractValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void MultiplyValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void DivideValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void ModuloValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void AndValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void OrValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<byte> column, byte scalar)
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
 
        public void XorValue(byte scalar, PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<byte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (byte)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<byte> left, PrimitiveColumnContainer<byte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<byte> column, byte scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class CharArithmetic : IPrimitiveDataFrameColumnArithmetic<char>
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
        public void AddValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void AddValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void SubtractValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void MultiplyValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void DivideValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void ModuloValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void AndValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void OrValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<char> column, char scalar)
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
 
        public void XorValue(char scalar, PrimitiveColumnContainer<char> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<char>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (char)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<char> left, PrimitiveColumnContainer<char> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<char> column, char scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class DecimalArithmetic : IPrimitiveDataFrameColumnArithmetic<decimal>
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
        public void AddValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
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
 
        public void AddValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (decimal)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
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
 
        public void SubtractValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (decimal)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
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
 
        public void MultiplyValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (decimal)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
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
 
        public void DivideValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (decimal)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
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
 
        public void ModuloValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (decimal)(scalar % span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void AndValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
 
        public void AndValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            throw new NotSupportedException(); 
        }
        public void Or(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void OrValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
 
        public void OrValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
        {
            throw new NotSupportedException(); 
        }
        public void Xor(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right)
        {
            throw new NotSupportedException();
        }
        public void XorValue(PrimitiveColumnContainer<decimal> column, decimal scalar)
        {
            throw new NotSupportedException();
        }
 
        public void XorValue(decimal scalar, PrimitiveColumnContainer<decimal> column)
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
        public void ElementwiseEquals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<decimal> left, PrimitiveColumnContainer<decimal> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<decimal> column, decimal scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class DoubleArithmetic : IPrimitiveDataFrameColumnArithmetic<double>
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
        public void AddValue(PrimitiveColumnContainer<double> column, double scalar)
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
 
        public void AddValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (double)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<double> column, double scalar)
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
 
        public void SubtractValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (double)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<double> column, double scalar)
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
 
        public void MultiplyValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (double)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<double> column, double scalar)
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
 
        public void DivideValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (double)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<double> column, double scalar)
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
 
        public void ModuloValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<double>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (double)(scalar % span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void AndValue(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
 
        public void AndValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            throw new NotSupportedException(); 
        }
        public void Or(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void OrValue(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
 
        public void OrValue(double scalar, PrimitiveColumnContainer<double> column)
        {
            throw new NotSupportedException(); 
        }
        public void Xor(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right)
        {
            throw new NotSupportedException();
        }
        public void XorValue(PrimitiveColumnContainer<double> column, double scalar)
        {
            throw new NotSupportedException();
        }
 
        public void XorValue(double scalar, PrimitiveColumnContainer<double> column)
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
        public void ElementwiseEquals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<double> left, PrimitiveColumnContainer<double> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<double> column, double scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class FloatArithmetic : IPrimitiveDataFrameColumnArithmetic<float>
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
        public void AddValue(PrimitiveColumnContainer<float> column, float scalar)
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
 
        public void AddValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (float)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<float> column, float scalar)
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
 
        public void SubtractValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (float)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<float> column, float scalar)
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
 
        public void MultiplyValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (float)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<float> column, float scalar)
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
 
        public void DivideValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (float)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<float> column, float scalar)
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
 
        public void ModuloValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<float>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (float)(scalar % span[i]);
                }
            }
        }
        public void And(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void AndValue(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
 
        public void AndValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            throw new NotSupportedException(); 
        }
        public void Or(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void OrValue(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
 
        public void OrValue(float scalar, PrimitiveColumnContainer<float> column)
        {
            throw new NotSupportedException(); 
        }
        public void Xor(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right)
        {
            throw new NotSupportedException();
        }
        public void XorValue(PrimitiveColumnContainer<float> column, float scalar)
        {
            throw new NotSupportedException();
        }
 
        public void XorValue(float scalar, PrimitiveColumnContainer<float> column)
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
        public void ElementwiseEquals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<float> left, PrimitiveColumnContainer<float> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<float> column, float scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class IntArithmetic : IPrimitiveDataFrameColumnArithmetic<int>
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
        public void AddValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void AddValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void SubtractValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void MultiplyValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void DivideValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void ModuloValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void AndValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void OrValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<int> column, int scalar)
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
 
        public void XorValue(int scalar, PrimitiveColumnContainer<int> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<int>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (int)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<int> left, PrimitiveColumnContainer<int> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<int> column, int scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class LongArithmetic : IPrimitiveDataFrameColumnArithmetic<long>
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
        public void AddValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void AddValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void SubtractValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void MultiplyValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void DivideValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void ModuloValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void AndValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void OrValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<long> column, long scalar)
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
 
        public void XorValue(long scalar, PrimitiveColumnContainer<long> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<long>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (long)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<long> left, PrimitiveColumnContainer<long> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<long> column, long scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class SByteArithmetic : IPrimitiveDataFrameColumnArithmetic<sbyte>
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
        public void AddValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void AddValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void SubtractValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void MultiplyValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void DivideValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void ModuloValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void AndValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void OrValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<sbyte> column, sbyte scalar)
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
 
        public void XorValue(sbyte scalar, PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (sbyte)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<sbyte> left, PrimitiveColumnContainer<sbyte> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<sbyte> column, sbyte scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class ShortArithmetic : IPrimitiveDataFrameColumnArithmetic<short>
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
        public void AddValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void AddValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void SubtractValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void MultiplyValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void DivideValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void ModuloValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void AndValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void OrValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<short> column, short scalar)
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
 
        public void XorValue(short scalar, PrimitiveColumnContainer<short> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<short>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (short)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<short> left, PrimitiveColumnContainer<short> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<short> column, short scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class UIntArithmetic : IPrimitiveDataFrameColumnArithmetic<uint>
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
        public void AddValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void AddValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void SubtractValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void MultiplyValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void DivideValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void ModuloValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void AndValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void OrValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<uint> column, uint scalar)
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
 
        public void XorValue(uint scalar, PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<uint>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (uint)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<uint> left, PrimitiveColumnContainer<uint> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<uint> column, uint scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class ULongArithmetic : IPrimitiveDataFrameColumnArithmetic<ulong>
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
        public void AddValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void AddValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void SubtractValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void MultiplyValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void DivideValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void ModuloValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void AndValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void OrValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<ulong> column, ulong scalar)
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
 
        public void XorValue(ulong scalar, PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ulong)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<ulong> left, PrimitiveColumnContainer<ulong> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<ulong> column, ulong scalar, PrimitiveColumnContainer<bool> ret)
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
    internal class UShortArithmetic : IPrimitiveDataFrameColumnArithmetic<ushort>
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
        public void AddValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void AddValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar + span[i]);
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
        public void SubtractValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void SubtractValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar - span[i]);
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
        public void MultiplyValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void MultiplyValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar * span[i]);
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
        public void DivideValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void DivideValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar / span[i]);
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
        public void ModuloValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void ModuloValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar % span[i]);
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
        public void AndValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void AndValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar & span[i]);
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
        public void OrValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void OrValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar | span[i]);
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
        public void XorValue(PrimitiveColumnContainer<ushort> column, ushort scalar)
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
 
        public void XorValue(ushort scalar, PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b]; 
                var mutableBuffer = DataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                column.Buffers[b] = mutableBuffer;
                var span = mutableBuffer.Span;
                for (int i = 0; i < span.Length; i++) 
                {
                    span[i] = (ushort)(scalar ^ span[i]);
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
        public void ElementwiseEquals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueEquals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseNotEquals(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueNotEquals(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThanOrEqual(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThanOrEqual(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseGreaterThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueGreaterThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseLessThan(PrimitiveColumnContainer<ushort> left, PrimitiveColumnContainer<ushort> right, PrimitiveColumnContainer<bool> ret)
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
        public void ElementwiseValueLessThan(PrimitiveColumnContainer<ushort> column, ushort scalar, PrimitiveColumnContainer<bool> ret)
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
