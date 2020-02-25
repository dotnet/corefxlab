// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    public partial class UIntDataFrameColumn : PrimitiveDataFrameColumn<uint>
    {
        public UIntDataFrameColumn(string name, IEnumerable<uint?> values) : base(name, values) { }

        public UIntDataFrameColumn(string name, IEnumerable<uint> values) : base(name, values) { }

        public UIntDataFrameColumn(string name, long length = 0) : base(name, length) { }

        public UIntDataFrameColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, buffer, nullBitMap, length, nullCount) { }

        internal UIntDataFrameColumn(PrimitiveDataFrameColumn<uint> uintColumn) : base(uintColumn.Name, uintColumn._columnContainer) { }
    }
}
