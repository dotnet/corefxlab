

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumnComputations.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    internal interface IPrimitiveColumnComputation<T>
        where T : struct
    {
        void Abs(PrimitiveColumnContainer<T> column);
        void All(PrimitiveColumnContainer<T> column, out bool ret);
        void Any(PrimitiveColumnContainer<T> column, out bool ret);
        void CumulativeMax(PrimitiveColumnContainer<T> column);
        void CumulativeMax(PrimitiveColumnContainer<T> column, IEnumerable<long> rows);
        void CumulativeMin(PrimitiveColumnContainer<T> column);
        void CumulativeMin(PrimitiveColumnContainer<T> column, IEnumerable<long> rows);
        void CumulativeProduct(PrimitiveColumnContainer<T> column);
        void CumulativeProduct(PrimitiveColumnContainer<T> column, IEnumerable<long> rows);
        void CumulativeSum(PrimitiveColumnContainer<T> column);
        void CumulativeSum(PrimitiveColumnContainer<T> column, IEnumerable<long> rows);
        void Max(PrimitiveColumnContainer<T> column, out T ret);
        void Max(PrimitiveColumnContainer<T> column, IEnumerable<long> rows, out T ret);
        void Min(PrimitiveColumnContainer<T> column, out T ret);
        void Min(PrimitiveColumnContainer<T> column, IEnumerable<long> rows, out T ret);
        void Product(PrimitiveColumnContainer<T> column, out T ret);
        void Product(PrimitiveColumnContainer<T> column, IEnumerable<long> rows, out T ret);
        void Sum(PrimitiveColumnContainer<T> column, out T ret);
        void Sum(PrimitiveColumnContainer<T> column, IEnumerable<long> rows, out T ret);
        void Round(PrimitiveColumnContainer<T> column);
    }

    internal static class PrimitiveColumnComputation<T>
        where T : struct
    {
        public static IPrimitiveColumnComputation<T> Instance { get; } = PrimitiveColumnComputation.GetComputation<T>();
    }

    internal static class PrimitiveColumnComputation
    {
        public static IPrimitiveColumnComputation<T> GetComputation<T>()
            where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return (IPrimitiveColumnComputation<T>)new BoolComputation();
            }
            else if (typeof(T) == typeof(byte))
            {
                return (IPrimitiveColumnComputation<T>)new ByteComputation();
            }
            else if (typeof(T) == typeof(char))
            {
                return (IPrimitiveColumnComputation<T>)new CharComputation();
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (IPrimitiveColumnComputation<T>)new DecimalComputation();
            }
            else if (typeof(T) == typeof(double))
            {
                return (IPrimitiveColumnComputation<T>)new DoubleComputation();
            }
            else if (typeof(T) == typeof(float))
            {
                return (IPrimitiveColumnComputation<T>)new FloatComputation();
            }
            else if (typeof(T) == typeof(int))
            {
                return (IPrimitiveColumnComputation<T>)new IntComputation();
            }
            else if (typeof(T) == typeof(long))
            {
                return (IPrimitiveColumnComputation<T>)new LongComputation();
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (IPrimitiveColumnComputation<T>)new SByteComputation();
            }
            else if (typeof(T) == typeof(short))
            {
                return (IPrimitiveColumnComputation<T>)new ShortComputation();
            }
            else if (typeof(T) == typeof(uint))
            {
                return (IPrimitiveColumnComputation<T>)new UIntComputation();
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (IPrimitiveColumnComputation<T>)new ULongComputation();
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (IPrimitiveColumnComputation<T>)new UShortComputation();
            }
            throw new NotSupportedException();
        }
    }

    internal class BoolComputation : IPrimitiveColumnComputation<bool>
    {
        public void Abs(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

        public void All(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            ret = true;
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer.ReadOnlySpan[i] == false)
                    {
                        ret = false;
                        return;
                    }
                }
            }
        }

        public void Any(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            ret = false;
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer.ReadOnlySpan[i] == true)
                    {
                        ret = true;
                        return;
                    }
                }
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMin(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMin(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows)
        {
            throw new NotSupportedException();
        }

        public void CumulativeProduct(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

        public void CumulativeProduct(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows)
        {
            throw new NotSupportedException();
        }

        public void CumulativeSum(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

        public void CumulativeSum(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows)
        {
            throw new NotSupportedException();
        }

        public void Max(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Max(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Min(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Min(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Product(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Product(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Sum(PrimitiveColumnContainer<bool> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Sum(PrimitiveColumnContainer<bool> column, IEnumerable<long> rows, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Round(PrimitiveColumnContainer<bool> column)
        {
            throw new NotSupportedException();
        }

    }
    internal class ByteComputation : IPrimitiveColumnComputation<byte>
    {
        public void Abs(PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (byte)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<byte> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<byte> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<byte> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows)
        {
            var ret = default(byte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<byte> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows)
        {
            var ret = default(byte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<byte> column)
        {
            var ret = (byte)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows)
        {
            var ret = default(byte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<byte> column)
        {
            var ret = (byte)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows)
        {
            var ret = default(byte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<byte> column, out byte ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows, out byte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<byte> column, out byte ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows, out byte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<byte> column, out byte ret)
        {
            ret = (byte)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows, out byte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<byte> column, out byte ret)
        {
            ret = (byte)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (byte)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<byte> column, IEnumerable<long> rows, out byte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (byte)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<byte> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<byte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (byte)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class CharComputation : IPrimitiveColumnComputation<char>
    {
        public void Abs(PrimitiveColumnContainer<char> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (char)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<char> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<char> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<char> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<char> column, IEnumerable<long> rows)
        {
            var ret = default(char);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<char> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<char> column, IEnumerable<long> rows)
        {
            var ret = default(char);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<char> column)
        {
            var ret = (char)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<char> column, IEnumerable<long> rows)
        {
            var ret = default(char);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<char> column)
        {
            var ret = (char)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<char> column, IEnumerable<long> rows)
        {
            var ret = default(char);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<char> column, out char ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<char> column, IEnumerable<long> rows, out char ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<char> column, out char ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<char> column, IEnumerable<long> rows, out char ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<char> column, out char ret)
        {
            ret = (char)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<char> column, IEnumerable<long> rows, out char ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<char> column, out char ret)
        {
            ret = (char)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (char)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<char> column, IEnumerable<long> rows, out char ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (char)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<char> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<char>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (char)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class DecimalComputation : IPrimitiveColumnComputation<decimal>
    {
        public void Abs(PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (decimal)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<decimal> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<decimal> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<decimal> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows)
        {
            var ret = default(decimal);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<decimal> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows)
        {
            var ret = default(decimal);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<decimal> column)
        {
            var ret = (decimal)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows)
        {
            var ret = default(decimal);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<decimal> column)
        {
            var ret = (decimal)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows)
        {
            var ret = default(decimal);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<decimal> column, out decimal ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows, out decimal ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<decimal> column, out decimal ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows, out decimal ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<decimal> column, out decimal ret)
        {
            ret = (decimal)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows, out decimal ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<decimal> column, out decimal ret)
        {
            ret = (decimal)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (decimal)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<decimal> column, IEnumerable<long> rows, out decimal ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (decimal)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<decimal> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<decimal>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (decimal)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class DoubleComputation : IPrimitiveColumnComputation<double>
    {
        public void Abs(PrimitiveColumnContainer<double> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (double)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<double> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<double> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<double> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<double> column, IEnumerable<long> rows)
        {
            var ret = default(double);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<double> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<double> column, IEnumerable<long> rows)
        {
            var ret = default(double);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<double> column)
        {
            var ret = (double)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<double> column, IEnumerable<long> rows)
        {
            var ret = default(double);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<double> column)
        {
            var ret = (double)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<double> column, IEnumerable<long> rows)
        {
            var ret = default(double);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<double> column, out double ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<double> column, IEnumerable<long> rows, out double ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<double> column, out double ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<double> column, IEnumerable<long> rows, out double ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<double> column, out double ret)
        {
            ret = (double)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<double> column, IEnumerable<long> rows, out double ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<double> column, out double ret)
        {
            ret = (double)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (double)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<double> column, IEnumerable<long> rows, out double ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (double)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<double> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<double>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (double)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class FloatComputation : IPrimitiveColumnComputation<float>
    {
        public void Abs(PrimitiveColumnContainer<float> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (float)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<float> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<float> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<float> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<float> column, IEnumerable<long> rows)
        {
            var ret = default(float);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<float> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<float> column, IEnumerable<long> rows)
        {
            var ret = default(float);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<float> column)
        {
            var ret = (float)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<float> column, IEnumerable<long> rows)
        {
            var ret = default(float);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<float> column)
        {
            var ret = (float)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<float> column, IEnumerable<long> rows)
        {
            var ret = default(float);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<float> column, out float ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<float> column, IEnumerable<long> rows, out float ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<float> column, out float ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<float> column, IEnumerable<long> rows, out float ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<float> column, out float ret)
        {
            ret = (float)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<float> column, IEnumerable<long> rows, out float ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<float> column, out float ret)
        {
            ret = (float)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (float)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<float> column, IEnumerable<long> rows, out float ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (float)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<float> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<float>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (float)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class IntComputation : IPrimitiveColumnComputation<int>
    {
        public void Abs(PrimitiveColumnContainer<int> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (int)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<int> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<int> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<int> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<int> column, IEnumerable<long> rows)
        {
            var ret = default(int);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<int> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<int> column, IEnumerable<long> rows)
        {
            var ret = default(int);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<int> column)
        {
            var ret = (int)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<int> column, IEnumerable<long> rows)
        {
            var ret = default(int);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<int> column)
        {
            var ret = (int)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<int> column, IEnumerable<long> rows)
        {
            var ret = default(int);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<int> column, out int ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<int> column, IEnumerable<long> rows, out int ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<int> column, out int ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<int> column, IEnumerable<long> rows, out int ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<int> column, out int ret)
        {
            ret = (int)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<int> column, IEnumerable<long> rows, out int ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<int> column, out int ret)
        {
            ret = (int)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (int)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<int> column, IEnumerable<long> rows, out int ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (int)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<int> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<int>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (int)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class LongComputation : IPrimitiveColumnComputation<long>
    {
        public void Abs(PrimitiveColumnContainer<long> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (long)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<long> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<long> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<long> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<long> column, IEnumerable<long> rows)
        {
            var ret = default(long);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<long> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<long> column, IEnumerable<long> rows)
        {
            var ret = default(long);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<long> column)
        {
            var ret = (long)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<long> column, IEnumerable<long> rows)
        {
            var ret = default(long);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<long> column)
        {
            var ret = (long)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<long> column, IEnumerable<long> rows)
        {
            var ret = default(long);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<long> column, out long ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<long> column, IEnumerable<long> rows, out long ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<long> column, out long ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<long> column, IEnumerable<long> rows, out long ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<long> column, out long ret)
        {
            ret = (long)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<long> column, IEnumerable<long> rows, out long ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<long> column, out long ret)
        {
            ret = (long)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (long)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<long> column, IEnumerable<long> rows, out long ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (long)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<long> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<long>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (long)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class SByteComputation : IPrimitiveColumnComputation<sbyte>
    {
        public void Abs(PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (sbyte)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<sbyte> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<sbyte> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<sbyte> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows)
        {
            var ret = default(sbyte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<sbyte> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows)
        {
            var ret = default(sbyte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<sbyte> column)
        {
            var ret = (sbyte)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows)
        {
            var ret = default(sbyte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<sbyte> column)
        {
            var ret = (sbyte)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows)
        {
            var ret = default(sbyte);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<sbyte> column, out sbyte ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows, out sbyte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<sbyte> column, out sbyte ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows, out sbyte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<sbyte> column, out sbyte ret)
        {
            ret = (sbyte)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows, out sbyte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<sbyte> column, out sbyte ret)
        {
            ret = (sbyte)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (sbyte)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<sbyte> column, IEnumerable<long> rows, out sbyte ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (sbyte)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<sbyte> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<sbyte>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (sbyte)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class ShortComputation : IPrimitiveColumnComputation<short>
    {
        public void Abs(PrimitiveColumnContainer<short> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (short)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<short> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<short> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<short> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<short> column, IEnumerable<long> rows)
        {
            var ret = default(short);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<short> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<short> column, IEnumerable<long> rows)
        {
            var ret = default(short);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<short> column)
        {
            var ret = (short)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<short> column, IEnumerable<long> rows)
        {
            var ret = default(short);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<short> column)
        {
            var ret = (short)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<short> column, IEnumerable<long> rows)
        {
            var ret = default(short);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<short> column, out short ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<short> column, IEnumerable<long> rows, out short ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<short> column, out short ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<short> column, IEnumerable<long> rows, out short ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<short> column, out short ret)
        {
            ret = (short)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<short> column, IEnumerable<long> rows, out short ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<short> column, out short ret)
        {
            ret = (short)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (short)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<short> column, IEnumerable<long> rows, out short ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (short)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<short> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<short>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (short)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class UIntComputation : IPrimitiveColumnComputation<uint>
    {
        public void Abs(PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (uint)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<uint> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<uint> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<uint> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows)
        {
            var ret = default(uint);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<uint> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows)
        {
            var ret = default(uint);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<uint> column)
        {
            var ret = (uint)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows)
        {
            var ret = default(uint);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<uint> column)
        {
            var ret = (uint)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows)
        {
            var ret = default(uint);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<uint> column, out uint ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows, out uint ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<uint> column, out uint ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows, out uint ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<uint> column, out uint ret)
        {
            ret = (uint)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows, out uint ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<uint> column, out uint ret)
        {
            ret = (uint)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (uint)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<uint> column, IEnumerable<long> rows, out uint ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (uint)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<uint> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<uint>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (uint)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class ULongComputation : IPrimitiveColumnComputation<ulong>
    {
        public void Abs(PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (ulong)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<ulong> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<ulong> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<ulong> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows)
        {
            var ret = default(ulong);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<ulong> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows)
        {
            var ret = default(ulong);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<ulong> column)
        {
            var ret = (ulong)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows)
        {
            var ret = default(ulong);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<ulong> column)
        {
            var ret = (ulong)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows)
        {
            var ret = default(ulong);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<ulong> column, out ulong ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows, out ulong ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<ulong> column, out ulong ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows, out ulong ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<ulong> column, out ulong ret)
        {
            ret = (ulong)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows, out ulong ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<ulong> column, out ulong ret)
        {
            ret = (ulong)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ulong)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<ulong> column, IEnumerable<long> rows, out ulong ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ulong)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<ulong> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ulong>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (ulong)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
    internal class UShortComputation : IPrimitiveColumnComputation<ushort>
    {
        public void Abs(PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (ushort)(Math.Abs((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void All(PrimitiveColumnContainer<ushort> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void Any(PrimitiveColumnContainer<ushort> column, out bool ret)
        {
            throw new NotSupportedException();
        }

        public void CumulativeMax(PrimitiveColumnContainer<ushort> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(Math.Max(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMax(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows)
        {
            var ret = default(ushort);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)Math.Max(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<ushort> column)
        {
            var ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(Math.Min(buffer.ReadOnlySpan[i], ret));
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeMin(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows)
        {
            var ret = default(ushort);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)Math.Min(column[row] ?? default, ret);
                column[row] = ret;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<ushort> column)
        {
            var ret = (ushort)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(buffer.ReadOnlySpan[i] * ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeProduct(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows)
        {
            var ret = default(ushort);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)((column[row] ?? default) * ret);
                column[row] = ret;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<ushort> column)
        {
            var ret = (ushort)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(buffer.ReadOnlySpan[i] + ret);
                    mutableBuffer.Span[i] = ret;
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

        public void CumulativeSum(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows)
        {
            var ret = default(ushort);
            IEnumerator<long> enumerator = rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ret = column[enumerator.Current] ?? default;
            }

            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)((column[row] ?? default) + ret);
                column[row] = ret;
            }
        }

        public void Max(PrimitiveColumnContainer<ushort> column, out ushort ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(Math.Max(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Max(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows, out ushort ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)(Math.Max(column[row] ?? default, ret));
            }
        }

        public void Min(PrimitiveColumnContainer<ushort> column, out ushort ret)
        {
            ret = column.Buffers[0].ReadOnlySpan[0];
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(Math.Min(buffer.ReadOnlySpan[i], ret));
                }
            }
        }

        public void Min(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows, out ushort ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)(Math.Min(column[row] ?? default, ret));
            }
        }

        public void Product(PrimitiveColumnContainer<ushort> column, out ushort ret)
        {
            ret = (ushort)1;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(buffer.ReadOnlySpan[i] * ret);
                }
            }
        }

        public void Product(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows, out ushort ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)((column[row] ?? default) * ret);
            }
        }

        public void Sum(PrimitiveColumnContainer<ushort> column, out ushort ret)
        {
            ret = (ushort)0;
            for (int b = 0 ; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                for (int i = 0; i < buffer.Length; i++)
                {
                    ret = (ushort)(buffer.ReadOnlySpan[i] + ret);
                }
            }
        }

        public void Sum(PrimitiveColumnContainer<ushort> column, IEnumerable<long> rows, out ushort ret)
        {
            ret = default;
            IEnumerator<long> enumerator = rows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                long row = enumerator.Current;
                ret = (ushort)((column[row] ?? default) + ret);
            }
        }

        public void Round(PrimitiveColumnContainer<ushort> column)
        {
            for (int b = 0; b < column.Buffers.Count; b++)
            {
                var buffer = column.Buffers[b];
                var mutableBuffer = MutableDataFrameBuffer<ushort>.GetMutableBuffer(buffer);
                for (int i = 0; i < buffer.Length; i++)
                {
                    mutableBuffer.Span[i] = (ushort)(Math.Round((decimal)mutableBuffer.Span[i]));
                }
                column.Buffers[b] = mutableBuffer;
            }
        }

    }
}
