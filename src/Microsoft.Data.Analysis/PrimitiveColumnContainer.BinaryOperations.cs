
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumnContainer.BinaryOperations.tt. Do not modify directly

namespace Microsoft.Data.Analysis
{
    internal partial class PrimitiveColumnContainer<T>
        where T : struct
    {
        public PrimitiveColumnContainer<T> Add(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Add(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> AddValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.AddValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Subtract(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Subtract(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> SubtractValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.SubtractValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Multiply(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Multiply(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> MultiplyValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.MultiplyValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Divide(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Divide(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> DivideValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.DivideValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Modulo(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Modulo(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> ModuloValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ModuloValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> And(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.And(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> AndValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.AndValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Or(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Or(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> OrValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.OrValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> Xor(PrimitiveColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Xor(this, right);
            return this;
        }

        public PrimitiveColumnContainer<T> XorValue(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.XorValue(this, scalar);
            return this;
        }

        public PrimitiveColumnContainer<T> LeftShift(int value)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LeftShift(this, value);
            return this;
        }

        public PrimitiveColumnContainer<T> RightShift(int value)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.RightShift(this, value);
            return this;
        }

       public PrimitiveColumnContainer<T> ElementwiseEquals(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseEquals(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueEquals(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueEquals(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseNotEquals(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseNotEquals(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueNotEquals(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueNotEquals(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseGreaterThanOrEqual(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseGreaterThanOrEqual(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueGreaterThanOrEqual(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueGreaterThanOrEqual(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseLessThanOrEqual(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseLessThanOrEqual(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueLessThanOrEqual(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueLessThanOrEqual(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseGreaterThan(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseGreaterThan(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueGreaterThan(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueGreaterThan(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseLessThan(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseLessThan(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ElementwiseValueLessThan(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ElementwiseValueLessThan(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> ReverseAddValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.AddValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseSubtractValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.SubtractValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseMultiplyValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.MultiplyValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseDivideValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.DivideValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseModuloValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.ModuloValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseAndValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.AndValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseOrValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.OrValue(scalar, this);
            return this;
       }
       public PrimitiveColumnContainer<T> ReverseXorValue(T scalar)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.XorValue(scalar, this);
            return this;
       }
    }
}
