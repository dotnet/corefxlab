// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    public static class PipeSequencesExtensions
    {
        public static ReadableBufferSequence AsSequence(this ReadOnlyBuffer buffer)
        {
            return new ReadableBufferSequence(buffer);
        }
    }
}
