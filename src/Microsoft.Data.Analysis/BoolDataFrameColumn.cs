﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    public partial class BoolDataFrameColumn : PrimitiveDataFrameColumn<bool>
    {
        public BoolDataFrameColumn(string name, IEnumerable<bool?> values) : base(name, values) { }

        public BoolDataFrameColumn(string name, IEnumerable<bool> values) : base(name, values) { }

        public BoolDataFrameColumn(string name, long length = 0) : base(name, length) { }

        public BoolDataFrameColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, buffer, nullBitMap, length, nullCount) { }

        internal BoolDataFrameColumn(PrimitiveDataFrameColumn<bool> boolColumn) : base(boolColumn.Name, boolColumn._columnContainer) { }

        internal BoolDataFrameColumn(string name, PrimitiveColumnContainer<bool> values) : base(name, values) { }
    }
}
