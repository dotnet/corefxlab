// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    public partial class UShortDataFrameColumn : PrimitiveDataFrameColumn<ushort>
    {
        public UShortDataFrameColumn(string name, IEnumerable<ushort?> values) : base(name, values) { }

        public UShortDataFrameColumn(string name, IEnumerable<ushort> values) : base(name, values) { }

        public UShortDataFrameColumn(string name, long length = 0) : base(name, length) { }

        public UShortDataFrameColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, buffer, nullBitMap, length, nullCount) { }

        internal UShortDataFrameColumn(PrimitiveDataFrameColumn<ushort> ushortColumn) : base(ushortColumn.Name, ushortColumn._columnContainer) { }

        internal UShortDataFrameColumn(string name, PrimitiveColumnContainer<ushort> values) : base(name, values) { }
    }
}
