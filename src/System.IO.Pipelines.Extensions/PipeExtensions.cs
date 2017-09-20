// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public static class PipeSequencesExtensions
    {
        public static WritableBufferOutput AsOutput(this WritableBuffer buffer)
        {
            return new WritableBufferOutput(buffer);
        }

        public static ReadableBufferSequence AsSequence(this ReadableBuffer buffer)
        {
            return new ReadableBufferSequence(buffer);
        }
    }
}
