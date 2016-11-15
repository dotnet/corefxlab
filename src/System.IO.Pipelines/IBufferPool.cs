using System;
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
