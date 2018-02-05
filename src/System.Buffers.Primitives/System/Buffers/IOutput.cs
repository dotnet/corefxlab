// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    /// <summary>
    /// Represents a <see cref="Byte"/> sink
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Notifies <see cref="IOutput"/> that <see cref="bytes"/> amount of data was written to <see cref="Span{Byte}"/>/<see cref="Memory{Byte}"/>
        /// </summary>
        void Advance(int bytes);

        /// <summary>
        /// Requests the <see cref="Memory{Byte}"/> of at least <see cref="minimumLength"/> in size.
        /// If <see cref="minimumLength"/> is equal to <code>0</code> currently available memory would get returned.
        /// </summary>
        Memory<byte> GetMemory(int minimumLength = 0);

        /// <summary>
        /// Requests the <see cref="Span{Byte}"/> of at least <see cref="minimumLength"/> in size.
        /// If <see cref="minimumLength"/> is equal to <code>0</code> currently available memory would get returned.
        /// </summary>
        Span<byte> GetSpan(int minimumLength = 0);
    }
}
