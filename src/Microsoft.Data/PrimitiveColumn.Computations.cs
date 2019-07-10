

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from PrimitiveColumn.Computations.tt. Do not modify directly

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data
{
    public partial class PrimitiveColumn<T> : BaseColumn
        where T : unmanaged
    {
        #region Computations
        public override void Abs()
        {
            PrimitiveColumnComputation<T>.Instance.Abs(_columnContainer);
        }
        public override bool All()
        {
            PrimitiveColumnComputation<T>.Instance.All(_columnContainer, out bool ret);
            return ret;
        }
        public override bool Any()
        {
            PrimitiveColumnComputation<T>.Instance.Any(_columnContainer, out bool ret);
            return ret;
        }
        public override void CumulativeMax()
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeMax(_columnContainer);
        }
        public override void CumulativeMax(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeMax(_columnContainer, rows);
        }
        public override void CumulativeMin()
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeMin(_columnContainer);
        }
        public override void CumulativeMin(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeMin(_columnContainer, rows);
        }
        public override void CumulativeProduct()
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeProduct(_columnContainer);
        }
        public override void CumulativeProduct(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeProduct(_columnContainer, rows);
        }
        public override void CumulativeSum()
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeSum(_columnContainer);
        }
        public override void CumulativeSum(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.CumulativeSum(_columnContainer, rows);
        }
        public override object Max()
        {
            PrimitiveColumnComputation<T>.Instance.Max(_columnContainer, out T ret);
            return ret;
        }
        public override object Max(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.Max(_columnContainer, rows, out T ret);
            return ret;
        }
        public override object Min()
        {
            PrimitiveColumnComputation<T>.Instance.Min(_columnContainer, out T ret);
            return ret;
        }
        public override object Min(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.Min(_columnContainer, rows, out T ret);
            return ret;
        }
        public override object Product()
        {
            PrimitiveColumnComputation<T>.Instance.Product(_columnContainer, out T ret);
            return ret;
        }
        public override object Product(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.Product(_columnContainer, rows, out T ret);
            return ret;
        }
        public override object Sum()
        {
            PrimitiveColumnComputation<T>.Instance.Sum(_columnContainer, out T ret);
            return ret;
        }
        public override object Sum(IEnumerable<long> rows)
        {
            PrimitiveColumnComputation<T>.Instance.Sum(_columnContainer, rows, out T ret);
            return ret;
        }
        public override void Round()
        {
            PrimitiveColumnComputation<T>.Instance.Round(_columnContainer);
        }
        #endregion
    }
}
