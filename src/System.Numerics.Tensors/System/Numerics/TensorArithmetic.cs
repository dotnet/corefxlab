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
        void Add(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Add(Tensor<T> tensor, T scalar, Tensor<T> result);
        void And(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void And(Tensor<T> tensor, T scalar, Tensor<T> result);
        void Contract(Tensor<T> left, Tensor<T> right, int[] leftAxes, int[] rightAxes, Tensor<T> result);
        void Decrement(Tensor<T> tensor, Tensor<T> result);
        void Divide(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Divide(Tensor<T> tensor, T scalar, Tensor<T> result);
        void Equals(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void GreaterThan(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void GreaterThanOrEqual(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void Increment(Tensor<T> tensor, Tensor<T> result);
        void LeftShift(Tensor<T> tensor, int value, Tensor<T> result);
        void LessThan(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void LessThanOrEqual(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void Modulo(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Modulo(Tensor<T> tensor, T scalar, Tensor<T> result);
        void Multiply(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Multiply(Tensor<T> tensor, T scalar, Tensor<T> result);
        void NotEquals(Tensor<T> left, Tensor<T> right, Tensor<bool> result);
        void Or(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Or(Tensor<T> tensor, T scalar, Tensor<T> result);
        void RightShift(Tensor<T> tensor, int value, Tensor<T> result);
        void Subtract(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Subtract(Tensor<T> tensor, T scalar, Tensor<T> result);
        void UnaryMinus(Tensor<T> tensor, Tensor<T> result);
        void UnaryPlus(Tensor<T> tensor, Tensor<T> result);
        void Xor(Tensor<T> left, Tensor<T> right, Tensor<T> result);
        void Xor(Tensor<T> tensor, T scalar, Tensor<T> result);
    }

    internal static class TensorArithmetic<T>
    {
        public static ITensorArithmetic<T> Instance => TensorArithmetic.GetArithmetic<T>();
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

        public void Add(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Add(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void And(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<bool> left, Tensor<bool> right, int[] leftAxes, int[] rightAxes, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Decrement(Tensor<bool> tensor, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Divide(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Divide(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Equals(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void GreaterThanOrEqual(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Increment(Tensor<bool> tensor, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void LeftShift(Tensor<bool> tensor, int value, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void LessThan(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void LessThanOrEqual(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Modulo(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Modulo(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Multiply(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Multiply(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void NotEquals(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<bool> tensor, int value, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Subtract(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Subtract(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void UnaryMinus(Tensor<bool> tensor, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void UnaryPlus(Tensor<bool> tensor, Tensor<bool> result)
        {
            throw new NotSupportedException();
        }
        public void Xor(Tensor<bool> left, Tensor<bool> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<bool> tensor, bool scalar, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (bool)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class ByteArithmetic : ITensorArithmetic<byte>
    {
        public byte One => 1;

        public void Add(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<byte> left, Tensor<byte> right, int[] leftAxes, int[] rightAxes, Tensor<byte> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                byte sum = (byte)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (byte)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<byte> tensor, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<byte> tensor, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<byte> tensor, int value, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<byte> left, Tensor<byte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<byte> tensor, int value, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<byte> tensor, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<byte> tensor, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<byte> left, Tensor<byte> right, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<byte> tensor, byte scalar, Tensor<byte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (byte)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class CharArithmetic : ITensorArithmetic<char>
    {
        public char One => (char)1;

        public void Add(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<char> left, Tensor<char> right, int[] leftAxes, int[] rightAxes, Tensor<char> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                char sum = (char)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (char)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<char> tensor, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<char> tensor, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<char> tensor, int value, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<char> left, Tensor<char> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<char> tensor, int value, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<char> tensor, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<char> tensor, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<char> left, Tensor<char> right, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<char> tensor, char scalar, Tensor<char> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (char)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class DecimalArithmetic : ITensorArithmetic<decimal>
    {
        public decimal One => 1;

        public void Add(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void And(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void Contract(Tensor<decimal> left, Tensor<decimal> right, int[] leftAxes, int[] rightAxes, Tensor<decimal> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                decimal sum = (decimal)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (decimal)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<decimal> tensor, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<decimal> tensor, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<decimal> tensor, int value, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void LessThan(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<decimal> left, Tensor<decimal> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void Or(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void RightShift(Tensor<decimal> tensor, int value, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void Subtract(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<decimal> tensor, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<decimal> tensor, Tensor<decimal> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (decimal)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<decimal> left, Tensor<decimal> right, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
        public void Xor(Tensor<decimal> tensor, decimal scalar, Tensor<decimal> result)
        {
            throw new NotSupportedException();
        }
    }
    internal class DoubleArithmetic : ITensorArithmetic<double>
    {
        public double One => 1.0;

        public void Add(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void And(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void Contract(Tensor<double> left, Tensor<double> right, int[] leftAxes, int[] rightAxes, Tensor<double> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                double sum = (double)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (double)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<double> tensor, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<double> tensor, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<double> tensor, int value, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void LessThan(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<double> left, Tensor<double> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void Or(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void RightShift(Tensor<double> tensor, int value, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void Subtract(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<double> tensor, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<double> tensor, Tensor<double> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (double)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<double> left, Tensor<double> right, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
        public void Xor(Tensor<double> tensor, double scalar, Tensor<double> result)
        {
            throw new NotSupportedException();
        }
    }
    internal class FloatArithmetic : ITensorArithmetic<float>
    {
        public float One => 1.0f;

        public void Add(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void And(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void Contract(Tensor<float> left, Tensor<float> right, int[] leftAxes, int[] rightAxes, Tensor<float> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                float sum = (float)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (float)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<float> tensor, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<float> tensor, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<float> tensor, int value, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void LessThan(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<float> left, Tensor<float> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void Or(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void RightShift(Tensor<float> tensor, int value, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void Subtract(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<float> tensor, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<float> tensor, Tensor<float> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (float)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<float> left, Tensor<float> right, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
        public void Xor(Tensor<float> tensor, float scalar, Tensor<float> result)
        {
            throw new NotSupportedException();
        }
    }
    internal class IntArithmetic : ITensorArithmetic<int>
    {
        public int One => 1;

        public void Add(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<int> left, Tensor<int> right, int[] leftAxes, int[] rightAxes, Tensor<int> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                int sum = (int)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (int)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<int> tensor, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<int> tensor, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<int> tensor, int value, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<int> left, Tensor<int> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<int> tensor, int value, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<int> tensor, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<int> tensor, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<int> left, Tensor<int> right, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<int> tensor, int scalar, Tensor<int> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (int)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class LongArithmetic : ITensorArithmetic<long>
    {
        public long One => 1;

        public void Add(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<long> left, Tensor<long> right, int[] leftAxes, int[] rightAxes, Tensor<long> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                long sum = (long)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (long)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<long> tensor, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<long> tensor, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<long> tensor, int value, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<long> left, Tensor<long> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<long> tensor, int value, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<long> tensor, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<long> tensor, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<long> left, Tensor<long> right, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<long> tensor, long scalar, Tensor<long> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (long)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class SByteArithmetic : ITensorArithmetic<sbyte>
    {
        public sbyte One => 1;

        public void Add(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<sbyte> left, Tensor<sbyte> right, int[] leftAxes, int[] rightAxes, Tensor<sbyte> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                sbyte sum = (sbyte)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (sbyte)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<sbyte> tensor, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<sbyte> tensor, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<sbyte> tensor, int value, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<sbyte> tensor, int value, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<sbyte> tensor, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<sbyte> tensor, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<sbyte> left, Tensor<sbyte> right, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<sbyte> tensor, sbyte scalar, Tensor<sbyte> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (sbyte)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class ShortArithmetic : ITensorArithmetic<short>
    {
        public short One => 1;

        public void Add(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<short> left, Tensor<short> right, int[] leftAxes, int[] rightAxes, Tensor<short> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                short sum = (short)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (short)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<short> tensor, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<short> tensor, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<short> tensor, int value, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<short> left, Tensor<short> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<short> tensor, int value, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<short> tensor, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)-tensor.Buffer[i];
            }
        }
        public void UnaryPlus(Tensor<short> tensor, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<short> left, Tensor<short> right, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<short> tensor, short scalar, Tensor<short> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (short)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class UIntArithmetic : ITensorArithmetic<uint>
    {
        public uint One => 1;

        public void Add(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<uint> left, Tensor<uint> right, int[] leftAxes, int[] rightAxes, Tensor<uint> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                uint sum = (uint)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (uint)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<uint> tensor, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<uint> tensor, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<uint> tensor, int value, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<uint> left, Tensor<uint> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<uint> tensor, int value, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<uint> tensor, Tensor<uint> result)
        {
            throw new NotSupportedException();
        }
        public void UnaryPlus(Tensor<uint> tensor, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<uint> left, Tensor<uint> right, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<uint> tensor, uint scalar, Tensor<uint> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (uint)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class ULongArithmetic : ITensorArithmetic<ulong>
    {
        public ulong One => 1;

        public void Add(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<ulong> left, Tensor<ulong> right, int[] leftAxes, int[] rightAxes, Tensor<ulong> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                ulong sum = (ulong)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (ulong)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<ulong> tensor, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<ulong> tensor, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<ulong> tensor, int value, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<ulong> left, Tensor<ulong> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<ulong> tensor, int value, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<ulong> tensor, Tensor<ulong> result)
        {
            throw new NotSupportedException();
        }
        public void UnaryPlus(Tensor<ulong> tensor, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<ulong> left, Tensor<ulong> right, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<ulong> tensor, ulong scalar, Tensor<ulong> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ulong)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
    internal class UShortArithmetic : ITensorArithmetic<ushort>
    {
        public ushort One => 1;

        public void Add(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] + right.Buffer[i]);
            }
        }
        public void Add(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] + scalar);
            }
        }
        public void And(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] & right.Buffer[i]);
            }
        }
        public void And(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] & scalar);
            }
        }
        public void Contract(Tensor<ushort> left, Tensor<ushort> right, int[] leftAxes, int[] rightAxes, Tensor<ushort> result)
        {
            var summingDimensions = new int[leftAxes.Length];
            for(int i = 0; i < leftAxes.Length; i++)
            {
                summingDimensions[i] = left.dimensions[leftAxes[i]];
            }

            var summingStrides = ArrayUtilities.GetStrides(summingDimensions);
            int summingLength = (int)ArrayUtilities.GetProduct(summingDimensions);

            var resultStrides = ArrayUtilities.GetStrides(result.dimensions);

            // translates from result index to left non-summing dimensions' index portion
            // since left non-summing dimensions are given precedence in result, the end is zero-padded
            int[] leftNonSummingStrides = new int[result.Rank];

            // translates from summing index to left summing dimensions' index portion
            int[] leftSummingStrides = new int[leftAxes.Length];
            ArrayUtilities.GetSplitStrides(left.dimensions, leftAxes, leftNonSummingStrides, 0, leftSummingStrides, 0);

            // translates from result index to right non-summing dimensions' index portion
            // since right non-summing dimensions are given not precedence in result, the begingin is zero-padded to account for dimensions that come from left.
            int[] rightNonSummingStrides = new int[result.Rank];
            int rightNonSummingStridesOffset = result.Rank - (left.Rank - leftAxes.Length);

            // translates from summing index to right summing dimensions' index portion
            int[] rightSummingStrides = new int[rightAxes.Length];
            ArrayUtilities.GetSplitStrides(right.dimensions, rightAxes, rightNonSummingStrides, rightNonSummingStridesOffset, rightSummingStrides, 0);

            for (int resultIndex = 0; resultIndex < result.Length; resultIndex++)
            {
                ushort sum = (ushort)0;

                int leftIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, leftNonSummingStrides);
                int rightIndexNonSumming = ArrayUtilities.TransformIndexByStrides(resultIndex, resultStrides, rightNonSummingStrides);

                for (int summingIndex = 0; summingIndex < summingLength; summingIndex++)
                {
                    int leftIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, leftSummingStrides);
                    int rightIndexSumming = ArrayUtilities.TransformIndexByStrides(summingIndex, summingStrides, rightSummingStrides);

                    int leftIndex = leftIndexNonSumming + leftIndexSumming;
                    int rightIndex = rightIndexNonSumming + rightIndexSumming;

                    sum += (ushort)(left.Buffer[leftIndex] * right.Buffer[rightIndex]);
                }

                result.Buffer[resultIndex] = sum;
            }
        }
        public void Decrement(Tensor<ushort> tensor, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]--;
            }
        }
        public void Divide(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] / right.Buffer[i]);
            }
        }
        public void Divide(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] / scalar);
            }
        }
        public void Equals(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] == right.Buffer[i];
            }
        }
        public void GreaterThan(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] > right.Buffer[i];
            }
        }
        public void GreaterThanOrEqual(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] >= right.Buffer[i];
            }
        }
        public void Increment(Tensor<ushort> tensor, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i]++;
            }
        }
        public void LeftShift(Tensor<ushort> tensor, int value, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] << value);
            }
        }
        public void LessThan(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] < right.Buffer[i];
            }
        }
        public void LessThanOrEqual(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] <= right.Buffer[i];
            }
        }
        public void Modulo(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] % right.Buffer[i]);
            }
        }
        public void Modulo(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] % scalar);
            }
        }
        public void Multiply(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] * right.Buffer[i]);
            }
        }
        public void Multiply(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] * scalar);
            }
        }
        public void NotEquals(Tensor<ushort> left, Tensor<ushort> right, Tensor<bool> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = left.Buffer[i] != right.Buffer[i];
            }
        }
        public void Or(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] | right.Buffer[i]);
            }
        }
        public void Or(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] | scalar);
            }
        }
        public void RightShift(Tensor<ushort> tensor, int value, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] >> value);
            }
        }
        public void Subtract(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] - right.Buffer[i]);
            }
        }
        public void Subtract(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] - scalar);
            }
        }
        public void UnaryMinus(Tensor<ushort> tensor, Tensor<ushort> result)
        {
            throw new NotSupportedException();
        }
        public void UnaryPlus(Tensor<ushort> tensor, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)+tensor.Buffer[i];
            }
        }
        public void Xor(Tensor<ushort> left, Tensor<ushort> right, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(left.Buffer[i] ^ right.Buffer[i]);
            }
        }
        public void Xor(Tensor<ushort> tensor, ushort scalar, Tensor<ushort> result)
        {
            for(int i = 0; i < result.Length; i++)
            {
                // TODO: vectorize
                result.Buffer[i] = (ushort)(tensor.Buffer[i] ^ scalar);
            }
        }
    }
}
