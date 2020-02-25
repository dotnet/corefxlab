// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    public partial class ShortDataFrameColumn : PrimitiveDataFrameColumn<short>
    {
        public ShortDataFrameColumn(string name, IEnumerable<short?> values) : base(name, values) { }

        public ShortDataFrameColumn(string name, IEnumerable<short> values) : base(name, values) { }

        public ShortDataFrameColumn(string name, long length = 0) : base(name, length) { }

        public ShortDataFrameColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, buffer, nullBitMap, length, nullCount) { }

        internal ShortDataFrameColumn(PrimitiveDataFrameColumn<short> shortColumn) : base(shortColumn.Name, shortColumn._columnContainer) { }
    }
}
