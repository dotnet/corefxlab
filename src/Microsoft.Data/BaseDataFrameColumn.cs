// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data
{
    /// <summary>
    /// The base column type. All APIs should have atleast a stub here first
    /// </summary>
    public abstract class BaseDataFrameColumn
    {
        public BaseDataFrameColumn(string name, long length = 0)
        {
            Length = length;
            Name = name;
        }

        private long _length;
        public long Length
        {
            get => _length;
            protected set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                _length = value;
            }
        }

        public long NullCount { get; protected set; }

        public string Name;

        public virtual object this[long rowIndex] { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

        public virtual object this[long startIndex, int length] { get { throw new NotImplementedException(); } }
    }
}
