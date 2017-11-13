// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.MemoryProtection
{
    public static partial class NativeMemory
    {
        // for non-Windows systems
        private static INativeMemory AllocateWithoutDataPopulationDefault(int cb)
        {
            return new ArrayWrapper(cb);
        }

        private sealed class ArrayWrapper : INativeMemory
        {
            private readonly byte[] _array;

            public ArrayWrapper(int cb)
            {
                _array = new byte[cb];
            }

            public bool IsReadonly => false;

            public Span<byte> Span => _array;

            public void Dispose()
            {
                // no-op
            }

            public void MakeReadonly()
            {
                // no-op
            }

            public void MakeWriteable()
            {
                // no-op
            }
        }
    }
}
