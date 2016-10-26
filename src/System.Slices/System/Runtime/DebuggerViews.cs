// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Runtime
{

    internal class SpanDebuggerView<T>
    {
        private ReadOnlySpan<T> _slice;

        public SpanDebuggerView(Span<T> slice)
        {
            _slice = slice;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get {
                return _slice.ToArray();
            }
        }
    }

    internal class ReadOnlySpanDebuggerView<T>
    {
        private ReadOnlySpan<T> _slice;

        public ReadOnlySpanDebuggerView(ReadOnlySpan<T> slice)
        {
            _slice = slice;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return _slice.ToArray();
            }
        }
    }

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
                return _memory.Span.ToArray();
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
                return _memory.Span.ToArray();
            }
        }
    }
}
