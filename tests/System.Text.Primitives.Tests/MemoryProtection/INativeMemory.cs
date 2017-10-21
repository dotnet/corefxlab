// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.MemoryProtection
{
    /// <summary>
    /// Represents a region of native memory. The <see cref="Span"/> property can be used
    /// to get a span backed by this memory region.
    /// </summary>
    public interface INativeMemory : IDisposable
    {
        /// <summary>
        /// Returns a value stating whether this native memory block is readonly.
        /// </summary>
        bool IsReadonly { get; }

        /// <summary>
        /// Gets the <see cref="Span{byte}"/> which represents this native memory.
        /// This <see cref="INativeMemory"/> instance must be kept alive while working with the span.
        /// </summary>
        Span<byte> Span { get; }

        /// <summary>
        /// Sets this native memory block to be readonly. Writes to this block will cause an AV.
        /// This method has no effect if the memory block is zero length or if the underlying
        /// OS does not support marking the memory block as readonly.
        /// </summary>
        void MakeReadonly();

        /// <summary>
        /// Sets this native memory block to be read+write.
        /// This method has no effect if the memory block is zero length or if the underlying
        /// OS does not support marking the memory block as read+write.
        /// </summary>
        void MakeWriteable();
    }
}
