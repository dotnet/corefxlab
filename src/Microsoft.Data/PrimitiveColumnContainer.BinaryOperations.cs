
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumnContainer.BinaryOperations.tt. Do not modify directly

namespace Microsoft.Data
{
    internal partial class PrimitiveColumnContainer<T>
        where T : struct
    {
       public PrimitiveColumnContainer<T> Add(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Add(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Add(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Add(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Subtract(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Subtract(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Subtract(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Subtract(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Multiply(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Multiply(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Multiply(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Multiply(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Divide(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Divide(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Divide(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Divide(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Modulo(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Modulo(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Modulo(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Modulo(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> And(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.And(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> And(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.And(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Or(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Or(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Or(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Or(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> Xor(PrimitiveColumnContainer<T> right)
        {
            PrimitiveColumnArithmetic<T>.Instance.Xor(this, right);
            return this;
        }

       public PrimitiveColumnContainer<T> Xor(T scalar)
        {
            PrimitiveColumnArithmetic<T>.Instance.Xor(this, scalar);
            return this;
        }

       public PrimitiveColumnContainer<T> LeftShift(int value)
        {
            PrimitiveColumnArithmetic<T>.Instance.LeftShift(this, value);
            return this;
        }

       public PrimitiveColumnContainer<T> RightShift(int value)
        {
            PrimitiveColumnArithmetic<T>.Instance.RightShift(this, value);
            return this;
        }

       public PrimitiveColumnContainer<T> PairwiseEquals(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseEquals(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseEquals(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseEquals(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseNotEquals(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseNotEquals(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseNotEquals(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseNotEquals(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseGreaterThanOrEqual(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseGreaterThanOrEqual(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseGreaterThanOrEqual(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseGreaterThanOrEqual(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseLessThanOrEqual(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseLessThanOrEqual(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseLessThanOrEqual(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseLessThanOrEqual(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseGreaterThan(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseGreaterThan(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseGreaterThan(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseGreaterThan(this, scalar, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseLessThan(PrimitiveColumnContainer<T> right, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseLessThan(this, right, ret);
            return this;
       }

       public PrimitiveColumnContainer<T> PairwiseLessThan(T scalar, PrimitiveColumnContainer<bool> ret)
       {
            PrimitiveColumnArithmetic<T>.Instance.PairwiseLessThan(this, scalar, ret);
            return this;
       }

    }
}
