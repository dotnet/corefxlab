// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Numerics.Experimental;

namespace Microsoft.Data.Analysis
{
    public partial class HalfDataFrameColumn : PrimitiveDataFrameColumn<Half>
    {
        public HalfDataFrameColumn(string name, IEnumerable<Half?> values) : base(name, values) { }

        public HalfDataFrameColumn(string name, IEnumerable<Half> values) : base(name, values) { }

        public HalfDataFrameColumn(string name, long length = 0) : base(name, length) { }

        public HalfDataFrameColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, buffer, nullBitMap, length, nullCount) { }

        internal HalfDataFrameColumn(string name, PrimitiveColumnContainer<Half> values) : base(name, values) { }
    }
}
