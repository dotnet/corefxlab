// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public static class PipeExtensions
    {
        public static PipeWriterOutput AsOutput(this IPipeWriter writer)
        {
            return new PipeWriterOutput(writer);
        }

        public static PipeWriterOutput AsOutput(this WritableBuffer buffer)
        {
            return new PipeWriterOutput(buffer.PipeWriter);
        }

        public static ReadableBufferSequence AsSequence(this ReadableBuffer buffer)
        {
            return new ReadableBufferSequence(buffer);
        }
    }
}
