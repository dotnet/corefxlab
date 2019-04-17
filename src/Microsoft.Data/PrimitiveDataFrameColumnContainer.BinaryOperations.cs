
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Data
{
    internal partial class PrimitiveDataFrameColumnContainer<T>
        where T : struct
    {
       public PrimitiveDataFrameColumnContainer<T> Add(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Add(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Add(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Add(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Subtract(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Subtract(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Subtract(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Subtract(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Multiply(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Multiply(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Multiply(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Multiply(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Divide(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Divide(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Divide(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Divide(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Modulo(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Modulo(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Modulo(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Modulo(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> And(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.And(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> And(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.And(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Or(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Or(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Or(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Or(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Xor(PrimitiveDataFrameColumnContainer<T> right)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Xor(this, right);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Xor(T scalar)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Xor(this, scalar);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> LeftShift(int value)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LeftShift(this, value);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> RightShift(int value)
        {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.RightShift(this, value);
            return this;
        }
       public PrimitiveDataFrameColumnContainer<T> Equals(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Equals(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> Equals(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.Equals(this, scalar, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> NotEquals(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.NotEquals(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> NotEquals(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.NotEquals(this, scalar, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> GreaterThanOrEqual(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.GreaterThanOrEqual(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> GreaterThanOrEqual(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.GreaterThanOrEqual(this, scalar, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> LessThanOrEqual(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LessThanOrEqual(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> LessThanOrEqual(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LessThanOrEqual(this, scalar, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> GreaterThan(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.GreaterThan(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> GreaterThan(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.GreaterThan(this, scalar, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> LessThan(PrimitiveDataFrameColumnContainer<T> right, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LessThan(this, right, ret);
            return this;
       }
       public PrimitiveDataFrameColumnContainer<T> LessThan(T scalar, PrimitiveDataFrameColumnContainer<bool> ret)
       {
            PrimitiveDataFrameColumnArithmetic<T>.Instance.LessThan(this, scalar, ret);
            return this;
       }
    }
}
