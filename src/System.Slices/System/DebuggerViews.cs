// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System
{
    internal class SpanDebuggerView<T>
    {
        private ReadOnlySpan<T> _slice;

        public SpanDebuggerView(ReadOnlySpan<T> slice)
        {
            _slice = slice;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return _slice.CreateArray();
            }
        }
    }
}
