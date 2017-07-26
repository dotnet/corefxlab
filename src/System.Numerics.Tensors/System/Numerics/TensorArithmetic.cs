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
        Tensor<T> Increment(Tensor<T> tensor);
        Tensor<T> Decrement(Tensor<T> tensor);
        Tensor<T> Multiply(Tensor<T> left, Tensor<T> right);
        Tensor<T> Multiply(Tensor<T> tensor, T scalar);
        Tensor<T> Divide(Tensor<T> left, Tensor<T> right);
        Tensor<T> Divide(Tensor<T> tensor, T scalar);
        Tensor<T> Modulo(Tensor<T> left, Tensor<T> right);
        Tensor<T> Modulo(Tensor<T> tensor, T scalar);
        Tensor<T> And(Tensor<T> left, Tensor<T> right);
        Tensor<T> And(Tensor<T> tensor, T scalar);
        Tensor<T> Or(Tensor<T> left, Tensor<T> right);
        Tensor<T> Or(Tensor<T> tensor, T scalar);
        Tensor<T> Xor(Tensor<T> left, Tensor<T> right);
        Tensor<T> Xor(Tensor<T> tensor, T scalar);
        Tensor<T> LeftShift(Tensor<T> tensor, int value);
        Tensor<T> RightShift(Tensor<T> tensor, int value);
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
        public Tensor<bool> Increment(Tensor<bool> tensor)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Decrement(Tensor<bool> tensor)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Multiply(Tensor<bool> left, Tensor<bool> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Multiply(Tensor<bool> tensor, bool scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Divide(Tensor<bool> left, Tensor<bool> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Divide(Tensor<bool> tensor, bool scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Modulo(Tensor<bool> left, Tensor<bool> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> Modulo(Tensor<bool> tensor, bool scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> And(Tensor<bool> left, Tensor<bool> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<bool> And(Tensor<bool> tensor, bool scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<bool> Or(Tensor<bool> left, Tensor<bool> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<bool> Or(Tensor<bool> tensor, bool scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<bool> Xor(Tensor<bool> left, Tensor<bool> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<bool> Xor(Tensor<bool> tensor, bool scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<bool> LeftShift(Tensor<bool> tensor, int value)
        {
            throw new NotSupportedException();
        }
        public Tensor<bool> RightShift(Tensor<bool> tensor, int value)
        {
            throw new NotSupportedException();
        }
    }
    internal class ByteArithmetic : ITensorArithmetic<byte>
    {
        public byte One => 1;

        public Tensor<byte> Add(Tensor<byte> left, Tensor<byte> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<byte> Increment(Tensor<byte> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<byte> Decrement(Tensor<byte> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<byte> Multiply(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> Multiply(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<byte> Divide(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> Divide(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<byte> Modulo(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> Modulo(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<byte> And(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> And(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<byte> Or(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> Or(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<byte> Xor(Tensor<byte> left, Tensor<byte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<byte> Xor(Tensor<byte> tensor, byte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<byte> LeftShift(Tensor<byte> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<byte> RightShift(Tensor<byte> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class CharArithmetic : ITensorArithmetic<char>
    {
        public char One => (char)1;

        public Tensor<char> Add(Tensor<char> left, Tensor<char> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<char> Increment(Tensor<char> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<char> Decrement(Tensor<char> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<char> Multiply(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> Multiply(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<char> Divide(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> Divide(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<char> Modulo(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> Modulo(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<char> And(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> And(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<char> Or(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> Or(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<char> Xor(Tensor<char> left, Tensor<char> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<char> Xor(Tensor<char> tensor, char scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<char> LeftShift(Tensor<char> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<char> RightShift(Tensor<char> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class DecimalArithmetic : ITensorArithmetic<decimal>
    {
        public decimal One => 1;

        public Tensor<decimal> Add(Tensor<decimal> left, Tensor<decimal> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<decimal> Increment(Tensor<decimal> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<decimal> Decrement(Tensor<decimal> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<decimal> Multiply(Tensor<decimal> left, Tensor<decimal> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<decimal> Multiply(Tensor<decimal> tensor, decimal scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<decimal> Divide(Tensor<decimal> left, Tensor<decimal> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<decimal> Divide(Tensor<decimal> tensor, decimal scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<decimal> Modulo(Tensor<decimal> left, Tensor<decimal> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<decimal> Modulo(Tensor<decimal> tensor, decimal scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<decimal> And(Tensor<decimal> left, Tensor<decimal> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> And(Tensor<decimal> tensor, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> Or(Tensor<decimal> left, Tensor<decimal> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> Or(Tensor<decimal> tensor, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> Xor(Tensor<decimal> left, Tensor<decimal> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> Xor(Tensor<decimal> tensor, decimal scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> LeftShift(Tensor<decimal> tensor, int value)
        {
            throw new NotSupportedException();
        }
        public Tensor<decimal> RightShift(Tensor<decimal> tensor, int value)
        {
            throw new NotSupportedException();
        }
    }
    internal class DoubleArithmetic : ITensorArithmetic<double>
    {
        public double One => 1.0;

        public Tensor<double> Add(Tensor<double> left, Tensor<double> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<double> Increment(Tensor<double> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<double> Decrement(Tensor<double> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<double> Multiply(Tensor<double> left, Tensor<double> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<double> Multiply(Tensor<double> tensor, double scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<double> Divide(Tensor<double> left, Tensor<double> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<double> Divide(Tensor<double> tensor, double scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<double> Modulo(Tensor<double> left, Tensor<double> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<double> Modulo(Tensor<double> tensor, double scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<double> And(Tensor<double> left, Tensor<double> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> And(Tensor<double> tensor, double scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> Or(Tensor<double> left, Tensor<double> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> Or(Tensor<double> tensor, double scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> Xor(Tensor<double> left, Tensor<double> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> Xor(Tensor<double> tensor, double scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> LeftShift(Tensor<double> tensor, int value)
        {
            throw new NotSupportedException();
        }
        public Tensor<double> RightShift(Tensor<double> tensor, int value)
        {
            throw new NotSupportedException();
        }
    }
    internal class FloatArithmetic : ITensorArithmetic<float>
    {
        public float One => 1.0f;

        public Tensor<float> Add(Tensor<float> left, Tensor<float> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<float> Increment(Tensor<float> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<float> Decrement(Tensor<float> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<float> Multiply(Tensor<float> left, Tensor<float> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<float> Multiply(Tensor<float> tensor, float scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<float> Divide(Tensor<float> left, Tensor<float> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<float> Divide(Tensor<float> tensor, float scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<float> Modulo(Tensor<float> left, Tensor<float> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<float> Modulo(Tensor<float> tensor, float scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<float> And(Tensor<float> left, Tensor<float> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> And(Tensor<float> tensor, float scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> Or(Tensor<float> left, Tensor<float> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> Or(Tensor<float> tensor, float scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> Xor(Tensor<float> left, Tensor<float> right)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> Xor(Tensor<float> tensor, float scalar)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> LeftShift(Tensor<float> tensor, int value)
        {
            throw new NotSupportedException();
        }
        public Tensor<float> RightShift(Tensor<float> tensor, int value)
        {
            throw new NotSupportedException();
        }
    }
    internal class IntArithmetic : ITensorArithmetic<int>
    {
        public int One => 1;

        public Tensor<int> Add(Tensor<int> left, Tensor<int> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<int> Increment(Tensor<int> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<int> Decrement(Tensor<int> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<int> Multiply(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> Multiply(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<int> Divide(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> Divide(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<int> Modulo(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> Modulo(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<int> And(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> And(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<int> Or(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> Or(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<int> Xor(Tensor<int> left, Tensor<int> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<int> Xor(Tensor<int> tensor, int scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<int> LeftShift(Tensor<int> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<int> RightShift(Tensor<int> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class LongArithmetic : ITensorArithmetic<long>
    {
        public long One => 1;

        public Tensor<long> Add(Tensor<long> left, Tensor<long> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<long> Increment(Tensor<long> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<long> Decrement(Tensor<long> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<long> Multiply(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> Multiply(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<long> Divide(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> Divide(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<long> Modulo(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> Modulo(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<long> And(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> And(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<long> Or(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> Or(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<long> Xor(Tensor<long> left, Tensor<long> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<long> Xor(Tensor<long> tensor, long scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<long> LeftShift(Tensor<long> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<long> RightShift(Tensor<long> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class SByteArithmetic : ITensorArithmetic<sbyte>
    {
        public sbyte One => 1;

        public Tensor<sbyte> Add(Tensor<sbyte> left, Tensor<sbyte> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<sbyte> Increment(Tensor<sbyte> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<sbyte> Decrement(Tensor<sbyte> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<sbyte> Multiply(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> Multiply(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<sbyte> Divide(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> Divide(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<sbyte> Modulo(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> Modulo(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<sbyte> And(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> And(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<sbyte> Or(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> Or(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<sbyte> Xor(Tensor<sbyte> left, Tensor<sbyte> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<sbyte> Xor(Tensor<sbyte> tensor, sbyte scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<sbyte> LeftShift(Tensor<sbyte> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<sbyte> RightShift(Tensor<sbyte> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class ShortArithmetic : ITensorArithmetic<short>
    {
        public short One => 1;

        public Tensor<short> Add(Tensor<short> left, Tensor<short> right)
        {
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
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)-tensor.Buffer[i];
            }

            return result;
        }
        public Tensor<short> Increment(Tensor<short> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<short> Decrement(Tensor<short> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<short> Multiply(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> Multiply(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<short> Divide(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> Divide(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<short> Modulo(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> Modulo(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<short> And(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> And(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<short> Or(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> Or(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<short> Xor(Tensor<short> left, Tensor<short> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<short> Xor(Tensor<short> tensor, short scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<short> LeftShift(Tensor<short> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<short> RightShift(Tensor<short> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class UIntArithmetic : ITensorArithmetic<uint>
    {
        public uint One => 1;

        public Tensor<uint> Add(Tensor<uint> left, Tensor<uint> right)
        {
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
        public Tensor<uint> Increment(Tensor<uint> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<uint> Decrement(Tensor<uint> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<uint> Multiply(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> Multiply(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<uint> Divide(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> Divide(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<uint> Modulo(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> Modulo(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<uint> And(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> And(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<uint> Or(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> Or(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<uint> Xor(Tensor<uint> left, Tensor<uint> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<uint> Xor(Tensor<uint> tensor, uint scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<uint> LeftShift(Tensor<uint> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<uint> RightShift(Tensor<uint> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class ULongArithmetic : ITensorArithmetic<ulong>
    {
        public ulong One => 1;

        public Tensor<ulong> Add(Tensor<ulong> left, Tensor<ulong> right)
        {
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
        public Tensor<ulong> Increment(Tensor<ulong> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<ulong> Decrement(Tensor<ulong> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<ulong> Multiply(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> Multiply(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<ulong> Divide(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> Divide(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<ulong> Modulo(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> Modulo(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<ulong> And(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> And(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<ulong> Or(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> Or(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<ulong> Xor(Tensor<ulong> left, Tensor<ulong> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ulong> Xor(Tensor<ulong> tensor, ulong scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<ulong> LeftShift(Tensor<ulong> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<ulong> RightShift(Tensor<ulong> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
    internal class UShortArithmetic : ITensorArithmetic<ushort>
    {
        public ushort One => 1;

        public Tensor<ushort> Add(Tensor<ushort> left, Tensor<ushort> right)
        {
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
        public Tensor<ushort> Increment(Tensor<ushort> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }

            return result;
        }
        public Tensor<ushort> Decrement(Tensor<ushort> tensor)
        {
            var result = tensor.Clone();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }

            return result;
        }
        public Tensor<ushort> Multiply(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] * right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> Multiply(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] * scalar);
            }

            return result;
        }
        public Tensor<ushort> Divide(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] / right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> Divide(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] / scalar);
            }

            return result;
        }
        public Tensor<ushort> Modulo(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] % right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> Modulo(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] % scalar);
            }

            return result;
        }
        public Tensor<ushort> And(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] & right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> And(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] & scalar);
            }

            return result;
        }
        public Tensor<ushort> Or(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] | right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> Or(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] | scalar);
            }

            return result;
        }
        public Tensor<ushort> Xor(Tensor<ushort> left, Tensor<ushort> right)
        {
            var result = left.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] ^ right.Buffer[i]);
            }

            return result;
        }
        public Tensor<ushort> Xor(Tensor<ushort> tensor, ushort scalar)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] ^ scalar);
            }

            return result;
        }
        public Tensor<ushort> LeftShift(Tensor<ushort> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] << value);
            }

            return result;
        }
        public Tensor<ushort> RightShift(Tensor<ushort> tensor, int value)
        {
            var result = tensor.CloneEmpty();

            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] >> value);
            }

            return result;
        }
    }
}
