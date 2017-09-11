// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public struct PreservedBuffer : IDisposable
    {
        private ReadableBuffer _buffer;

        internal PreservedBuffer(ref ReadableBuffer buffer)
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
            var returnStart = _buffer.Start.Segment;
            var returnEnd = _buffer.End.Segment;

            while (true)
            {
                var returnSegment = returnStart;
                returnStart = returnStart?.Next;
                returnSegment?.Dispose();

                if (returnSegment == returnEnd)
                {
                    break;
                }
            }

            _buffer.ClearCursors();
        }

    }
}
