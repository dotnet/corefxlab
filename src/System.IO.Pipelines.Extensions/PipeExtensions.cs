// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.IO.Pipelines
{
    public static class PipeExtensions
    {
        public static PipeOutput AsOutput(this IPipeWriter writer)
        {
            return new PipeOutput(writer);
        }

        public static PipeOutput AsOutput(this WritableBuffer buffer)
        {
            return new PipeOutput(buffer.PipeWriter);
        }

        public static ISequence<ReadOnlyBuffer<byte>> AsSequence(this ReadableBuffer buffer)
        {
            return new PipeSequence(buffer);
        }
    }
}
