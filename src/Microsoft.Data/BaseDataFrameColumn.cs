// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data
{
    public abstract class BaseDataFrameColumn
    {
        public BaseDataFrameColumn(string name, long length = 0)
        {
            Length = length;
            Name = name;
        }
        public long Length { get; protected set; }
        public long NullCount { get; protected set; }
        public string Name;

        public virtual object this[long rowIndex] { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        public virtual object this[long startIndex, int length] { get { throw new NotImplementedException(); } }
    }
}
