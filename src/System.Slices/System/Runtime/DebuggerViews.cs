// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Runtime
{
    internal class MemoryDebuggerView<T>
    {
        private ReadOnlyMemory<T> _memory;

        public MemoryDebuggerView(Memory<T> memory)
        {
            _memory = memory;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get {
                return _memory.ToArray();
            }
        }
    }

    internal class ReadOnlyMemoryDebuggerView<T>
    {
        private ReadOnlyMemory<T> _memory;

        public ReadOnlyMemoryDebuggerView(ReadOnlyMemory<T> memory)
        {
            _memory = memory;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get {
                return _memory.ToArray();
            }
        }
    }
}
