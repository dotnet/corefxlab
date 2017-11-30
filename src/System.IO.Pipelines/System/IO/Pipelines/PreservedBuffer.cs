// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public readonly struct PreservedBuffer : IDisposable
    {
        private readonly ReadableBuffer _buffer;

        internal PreservedBuffer(in ReadableBuffer buffer)
        {
            _buffer = buffer;
        }

        /// <summary>
        /// Returns the preserved <see cref="ReadableBuffer"/>.
        /// </summary>
        public ReadableBuffer Buffer => _buffer;

        /// <summary>
        /// Dispose the preserved buffer.
        /// </summary>
        public void Dispose()
        {
            var returnStart = _buffer.Start.GetSegment();
            var returnEnd = _buffer.End.GetSegment();

            while (true)
            {
                var returnSegment = returnStart;
                returnStart = returnStart?.Next;
                returnSegment?.ResetMemory();

                if (returnSegment == returnEnd)
                {
                    break;
                }
            }
        }
    }
}
