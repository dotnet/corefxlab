﻿

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
        public override BaseColumn Abs(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.Abs(ret._columnContainer);
            return ret;
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
        public override BaseColumn CumulativeMax(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeMax(ret._columnContainer);
            return ret;
        }
        public override BaseColumn CumulativeMax(IEnumerable<long> rowIndices, bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeMax(ret._columnContainer, rowIndices);
            return ret;
        }
        public override BaseColumn CumulativeMin(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeMin(ret._columnContainer);
            return ret;
        }
        public override BaseColumn CumulativeMin(IEnumerable<long> rowIndices, bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeMin(ret._columnContainer, rowIndices);
            return ret;
        }
        public override BaseColumn CumulativeProduct(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeProduct(ret._columnContainer);
            return ret;
        }
        public override BaseColumn CumulativeProduct(IEnumerable<long> rowIndices, bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeProduct(ret._columnContainer, rowIndices);
            return ret;
        }
        public override BaseColumn CumulativeSum(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeSum(ret._columnContainer);
            return ret;
        }
        public override BaseColumn CumulativeSum(IEnumerable<long> rowIndices, bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.CumulativeSum(ret._columnContainer, rowIndices);
            return ret;
        }
        public override object Max()
        {
            PrimitiveColumnComputation<T>.Instance.Max(_columnContainer, out T ret);
            return ret;
        }
        public override object Max(IEnumerable<long> rowIndices)
        {
            PrimitiveColumnComputation<T>.Instance.Max(_columnContainer, rowIndices, out T ret);
            return ret;
        }
        public override object Min()
        {
            PrimitiveColumnComputation<T>.Instance.Min(_columnContainer, out T ret);
            return ret;
        }
        public override object Min(IEnumerable<long> rowIndices)
        {
            PrimitiveColumnComputation<T>.Instance.Min(_columnContainer, rowIndices, out T ret);
            return ret;
        }
        public override object Product()
        {
            PrimitiveColumnComputation<T>.Instance.Product(_columnContainer, out T ret);
            return ret;
        }
        public override object Product(IEnumerable<long> rowIndices)
        {
            PrimitiveColumnComputation<T>.Instance.Product(_columnContainer, rowIndices, out T ret);
            return ret;
        }
        public override object Sum()
        {
            PrimitiveColumnComputation<T>.Instance.Sum(_columnContainer, out T ret);
            return ret;
        }
        public override object Sum(IEnumerable<long> rowIndices)
        {
            PrimitiveColumnComputation<T>.Instance.Sum(_columnContainer, rowIndices, out T ret);
            return ret;
        }
        public override BaseColumn Round(bool inPlace = false)
        {
            PrimitiveColumn<T> ret = inPlace ? this : Clone();
            PrimitiveColumnComputation<T>.Instance.Round(ret._columnContainer);
            return ret;
        }
    }
}
