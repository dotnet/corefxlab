// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Numerics
{
    internal interface ITensorArithmetic<T>
    {
        T One { get; }
        Tensor<T> Add(Tensor<T> left, Tensor<T> right);
        Tensor<T> Add(Tensor<T> tensor, T scalar);
        Tensor<T> UnaryPlus(Tensor<T> tensor);
        Tensor<T> Subtract(Tensor<T> left, Tensor<T> right);
        Tensor<T> Subtract(Tensor<T> tensor, T scalar);
        Tensor<T> UnaryMinus(Tensor<T> tensor);
    }

    internal static class TensorArithmetic
    {   
        public static ITensorArithmetic<T> GetArithmetic<T>()
        {
            if (typeof(T) == typeof(bool))
            {
                return (ITensorArithmetic<T>)new BoolArithmetic();
            }
            else if (typeof(T) == typeof(byte))
            {
                return (ITensorArithmetic<T>)new ByteArithmetic();
            }
            else if (typeof(T) == typeof(char))
            {
                return (ITensorArithmetic<T>)new CharArithmetic();
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (ITensorArithmetic<T>)new DecimalArithmetic();
            }
            else if (typeof(T) == typeof(double))
            {
                return (ITensorArithmetic<T>)new DoubleArithmetic();
            }
            else if (typeof(T) == typeof(float))
            {
                return (ITensorArithmetic<T>)new FloatArithmetic();
            }
            else if (typeof(T) == typeof(int))
            {
                return (ITensorArithmetic<T>)new IntArithmetic();
            }
            else if (typeof(T) == typeof(long))
            {
                return (ITensorArithmetic<T>)new LongArithmetic();
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (ITensorArithmetic<T>)new SByteArithmetic();
            }
            else if (typeof(T) == typeof(short))
            {
                return (ITensorArithmetic<T>)new ShortArithmetic();
            }
            else if (typeof(T) == typeof(uint))
            {
                return (ITensorArithmetic<T>)new UIntArithmetic();
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (ITensorArithmetic<T>)new ULongArithmetic();
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (ITensorArithmetic<T>)new UShortArithmetic();
            }
            return null;
        }
    }
    
    internal class BoolArithmetic : ITensorArithmetic<bool>
    {
        public bool One => true;

        public Tensor<bool> Add(Tensor<bool> left, Tensor<bool> right)
        {
            throw new NotSupportedException();
        }

        public Tensor<bool> Add(Tensor<bool> tensor, bool scalar)
        {
            throw new NotSupportedException();
        }

        public Tensor<bool> UnaryPlus(Tensor<bool> tensor)
        {
            throw new NotSupportedException();
        }

        public Tensor<bool> Subtract(Tensor<bool> left, Tensor<bool> right)
        {
            throw new NotSupportedException();
        }

        public Tensor<bool> Subtract(Tensor<bool> tensor, bool scalar)
        {
            throw new NotSupportedException();
        }

        public Tensor<bool> UnaryMinus(Tensor<bool> tensor)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : ITensorArithmetic<byte>
    {
        public byte One => 1;

        public Tensor<byte> Add(Tensor<byte> left, Tensor<byte> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<byte> Add(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<byte> UnaryPlus(Tensor<byte> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<byte> Subtract(Tensor<byte> left, Tensor<byte> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<byte> Subtract(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<byte> UnaryMinus(Tensor<byte> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class CharArithmetic : ITensorArithmetic<char>
    {
        public char One => (char)1;

        public Tensor<char> Add(Tensor<char> left, Tensor<char> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<char> Add(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<char> UnaryPlus(Tensor<char> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<char> Subtract(Tensor<char> left, Tensor<char> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<char> Subtract(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<char> UnaryMinus(Tensor<char> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class DecimalArithmetic : ITensorArithmetic<decimal>
    {
        public decimal One => 1;

        public Tensor<decimal> Add(Tensor<decimal> left, Tensor<decimal> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<decimal> Add(Tensor<decimal> tensor, decimal scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<decimal> UnaryPlus(Tensor<decimal> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<decimal> Subtract(Tensor<decimal> left, Tensor<decimal> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<decimal> Subtract(Tensor<decimal> tensor, decimal scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<decimal> UnaryMinus(Tensor<decimal> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class DoubleArithmetic : ITensorArithmetic<double>
    {
        public double One => 1.0;

        public Tensor<double> Add(Tensor<double> left, Tensor<double> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<double> Add(Tensor<double> tensor, double scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<double> UnaryPlus(Tensor<double> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<double> Subtract(Tensor<double> left, Tensor<double> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<double> Subtract(Tensor<double> tensor, double scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<double> UnaryMinus(Tensor<double> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class FloatArithmetic : ITensorArithmetic<float>
    {
        public float One => 1.0f;

        public Tensor<float> Add(Tensor<float> left, Tensor<float> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<float> Add(Tensor<float> tensor, float scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<float> UnaryPlus(Tensor<float> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<float> Subtract(Tensor<float> left, Tensor<float> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<float> Subtract(Tensor<float> tensor, float scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<float> UnaryMinus(Tensor<float> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class IntArithmetic : ITensorArithmetic<int>
    {
        public int One => 1;

        public Tensor<int> Add(Tensor<int> left, Tensor<int> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<int> Add(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<int> UnaryPlus(Tensor<int> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<int> Subtract(Tensor<int> left, Tensor<int> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<int> Subtract(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<int> UnaryMinus(Tensor<int> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class LongArithmetic : ITensorArithmetic<long>
    {
        public long One => 1;

        public Tensor<long> Add(Tensor<long> left, Tensor<long> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<long> Add(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<long> UnaryPlus(Tensor<long> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<long> Subtract(Tensor<long> left, Tensor<long> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<long> Subtract(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<long> UnaryMinus(Tensor<long> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class SByteArithmetic : ITensorArithmetic<sbyte>
    {
        public sbyte One => 1;

        public Tensor<sbyte> Add(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<sbyte> Add(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<sbyte> UnaryPlus(Tensor<sbyte> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<sbyte> Subtract(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<sbyte> Subtract(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<sbyte> UnaryMinus(Tensor<sbyte> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class ShortArithmetic : ITensorArithmetic<short>
    {
        public short One => 1;

        public Tensor<short> Add(Tensor<short> left, Tensor<short> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<short> Add(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<short> UnaryPlus(Tensor<short> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<short> Subtract(Tensor<short> left, Tensor<short> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<short> Subtract(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<short> UnaryMinus(Tensor<short> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)-tensor.Buffer[i];
            }

            return result;
        }
    }
    internal class UIntArithmetic : ITensorArithmetic<uint>
    {
        public uint One => 1;

        public Tensor<uint> Add(Tensor<uint> left, Tensor<uint> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<uint> Add(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<uint> UnaryPlus(Tensor<uint> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<uint> Subtract(Tensor<uint> left, Tensor<uint> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<uint> Subtract(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<uint> UnaryMinus(Tensor<uint> tensor)
        {
            throw new NotSupportedException();
        }
    }
    internal class ULongArithmetic : ITensorArithmetic<ulong>
    {
        public ulong One => 1;

        public Tensor<ulong> Add(Tensor<ulong> left, Tensor<ulong> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<ulong> Add(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<ulong> UnaryPlus(Tensor<ulong> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<ulong> Subtract(Tensor<ulong> left, Tensor<ulong> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<ulong> Subtract(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<ulong> UnaryMinus(Tensor<ulong> tensor)
        {
            throw new NotSupportedException();
        }
    }
    internal class UShortArithmetic : ITensorArithmetic<ushort>
    {
        public ushort One => 1;

        public Tensor<ushort> Add(Tensor<ushort> left, Tensor<ushort> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] + right.Buffer[i]);
            }

            return result;
        }

        public Tensor<ushort> Add(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] + scalar);
            }

            return result;
        }

        public Tensor<ushort> UnaryPlus(Tensor<ushort> tensor)
        {
            // TODO: Assert same shape
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)+tensor.Buffer[i];
            }

            return result;
        }

        public Tensor<ushort> Subtract(Tensor<ushort> left, Tensor<ushort> right)
        {
            // TODO: Assert same shape
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] - right.Buffer[i]);
            }

            return result;
        }

        public Tensor<ushort> Subtract(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] - scalar);
            }

            return result;
        }

        public Tensor<ushort> UnaryMinus(Tensor<ushort> tensor)
        {
            throw new NotSupportedException();
        }
    }
}
