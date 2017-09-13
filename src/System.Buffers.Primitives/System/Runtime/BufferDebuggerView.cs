// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Runtime
{
    internal class BufferDebuggerView<T>
    {
        private ReadOnlyMemory<T> _buffer;

        public BufferDebuggerView(Memory<T> buffer)
        {
            _buffer = buffer;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get {
                return _buffer.ToArray();
            }
        }
    }
}
