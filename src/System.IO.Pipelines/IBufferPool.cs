// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    /// <summary>
    /// An interface that represents a <see cref="IBufferPool"/> that will be used to allocate memory.
    /// </summary>
    public interface IBufferPool : IDisposable
    {
        /// <summary>
        /// Leases a <see cref="IBuffer"/> from the <see cref="IBufferPool"/>
        /// </summary>
        /// <param name="size">The size of the requested buffer</param>
        /// <returns>A <see cref="IBuffer"/> which is a wrapper around leased memory</returns>
        OwnedMemory<byte> Lease(int size);
    }
}
