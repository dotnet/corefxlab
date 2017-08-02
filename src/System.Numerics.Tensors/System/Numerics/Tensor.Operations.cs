// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Numerics
{
    public static partial class Tensor
    { 
        public static void Add<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Add(left, right, result);
        }

        public static Tensor<T> Add<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Add(left, right, result);

            return result;
        }

        public static void Add<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Add(tensor, scalar, result);
        }

        public static Tensor<T> Add<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Add(tensor, scalar, result);

            return result;
        }

        public static void And<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.And(left, right, result);
        }

        public static Tensor<T> And<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.And(left, right, result);

            return result;
        }

        public static void And<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.And(tensor, scalar, result);
        }

        public static Tensor<T> And<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.And(tensor, scalar, result);

            return result;
        }

        public static void Contract<T>(Tensor<T> left, Tensor<T> right, int[] leftAxes, int[] rightAxes, Tensor<T> result)
        {
            var resultDimensions = ValidateContractArgs(left, right, leftAxes, rightAxes, result);

            TensorArithmetic<T>.Instance.Contract(left, right, leftAxes, rightAxes, result);
        }

        public static Tensor<T> Contract<T>(Tensor<T> left, Tensor<T> right, int[] leftAxes, int[] rightAxes)
        {
            var resultDimensions = ValidateContractArgs(left, right, leftAxes, rightAxes);

            var result = new Tensor<T>(resultDimensions);
            
            TensorArithmetic<T>.Instance.Contract(left, right, leftAxes, rightAxes, result);

            return result;
        }

        public static void Decrement<T>(Tensor<T> tensor, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Decrement(tensor, result);
        }

        public static Tensor<T> Decrement<T>(Tensor<T> tensor)
        {
            ValidateArgs(tensor);

            var result = tensor.Clone();
            
            TensorArithmetic<T>.Instance.Decrement(tensor, result);

            return result;
        }

        public static void Divide<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Divide(left, right, result);
        }

        public static Tensor<T> Divide<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Divide(left, right, result);

            return result;
        }

        public static void Divide<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Divide(tensor, scalar, result);
        }

        public static Tensor<T> Divide<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Divide(tensor, scalar, result);

            return result;
        }

        public static void Equals<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Equals(left, right, result);
        }

        public static Tensor<bool> Equals<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.Equals(left, right, result);

            return result;
        }

        public static void GreaterThan<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.GreaterThan(left, right, result);
        }

        public static Tensor<bool> GreaterThan<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.GreaterThan(left, right, result);

            return result;
        }

        public static void GreaterThanOrEqual<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.GreaterThanOrEqual(left, right, result);
        }

        public static Tensor<bool> GreaterThanOrEqual<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.GreaterThanOrEqual(left, right, result);

            return result;
        }

        public static void Increment<T>(Tensor<T> tensor, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Increment(tensor, result);
        }

        public static Tensor<T> Increment<T>(Tensor<T> tensor)
        {
            ValidateArgs(tensor);

            var result = tensor.Clone();
            
            TensorArithmetic<T>.Instance.Increment(tensor, result);

            return result;
        }

        public static void LeftShift<T>(Tensor<T> tensor, int value, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.LeftShift(tensor, value, result);
        }

        public static Tensor<T> LeftShift<T>(Tensor<T> tensor, int value)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.LeftShift(tensor, value, result);

            return result;
        }

        public static void LessThan<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.LessThan(left, right, result);
        }

        public static Tensor<bool> LessThan<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.LessThan(left, right, result);

            return result;
        }

        public static void LessThanOrEqual<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.LessThanOrEqual(left, right, result);
        }

        public static Tensor<bool> LessThanOrEqual<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.LessThanOrEqual(left, right, result);

            return result;
        }

        public static void Modulo<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Modulo(left, right, result);
        }

        public static Tensor<T> Modulo<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Modulo(left, right, result);

            return result;
        }

        public static void Modulo<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Modulo(tensor, scalar, result);
        }

        public static Tensor<T> Modulo<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Modulo(tensor, scalar, result);

            return result;
        }

        public static void Multiply<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Multiply(left, right, result);
        }

        public static Tensor<T> Multiply<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Multiply(left, right, result);

            return result;
        }

        public static void Multiply<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Multiply(tensor, scalar, result);
        }

        public static Tensor<T> Multiply<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Multiply(tensor, scalar, result);

            return result;
        }

        public static void NotEquals<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.NotEquals(left, right, result);
        }

        public static Tensor<bool> NotEquals<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty<bool>();
            
            TensorArithmetic<T>.Instance.NotEquals(left, right, result);

            return result;
        }

        public static void Or<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Or(left, right, result);
        }

        public static Tensor<T> Or<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Or(left, right, result);

            return result;
        }

        public static void Or<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Or(tensor, scalar, result);
        }

        public static Tensor<T> Or<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Or(tensor, scalar, result);

            return result;
        }

        public static void RightShift<T>(Tensor<T> tensor, int value, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.RightShift(tensor, value, result);
        }

        public static Tensor<T> RightShift<T>(Tensor<T> tensor, int value)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.RightShift(tensor, value, result);

            return result;
        }

        public static void Subtract<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Subtract(left, right, result);
        }

        public static Tensor<T> Subtract<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Subtract(left, right, result);

            return result;
        }

        public static void Subtract<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Subtract(tensor, scalar, result);
        }

        public static Tensor<T> Subtract<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Subtract(tensor, scalar, result);

            return result;
        }

        public static void UnaryMinus<T>(Tensor<T> tensor, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.UnaryMinus(tensor, result);
        }

        public static Tensor<T> UnaryMinus<T>(Tensor<T> tensor)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.UnaryMinus(tensor, result);

            return result;
        }

        public static void UnaryPlus<T>(Tensor<T> tensor, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.UnaryPlus(tensor, result);
        }

        public static Tensor<T> UnaryPlus<T>(Tensor<T> tensor)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.UnaryPlus(tensor, result);

            return result;
        }

        public static void Xor<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            ValidateBinaryArgs(left, right, result);

            TensorArithmetic<T>.Instance.Xor(left, right, result);
        }

        public static Tensor<T> Xor<T>(Tensor<T> left, Tensor<T> right)
        {
            ValidateBinaryArgs(left, right);

            var result = left.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Xor(left, right, result);

            return result;
        }

        public static void Xor<T>(Tensor<T> tensor, T scalar, Tensor<T> result)
        {
            ValidateArgs(tensor, result);

            TensorArithmetic<T>.Instance.Xor(tensor, scalar, result);
        }

        public static Tensor<T> Xor<T>(Tensor<T> tensor, T scalar)
        {
            ValidateArgs(tensor);

            var result = tensor.CloneEmpty();
            
            TensorArithmetic<T>.Instance.Xor(tensor, scalar, result);

            return result;
        }

    }
}
