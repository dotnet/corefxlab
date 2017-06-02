// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;

// This should most likely be in System.Text.Primitives and not here. Will move once Base64 moves.
namespace System.Binary.Base64
{
    public static class BufferExtensions
    {
        const int stackLength = 32;

        public static void Pipe(this ITransformation transformation, ReadOnlyBytes source, IOutput destination)
        {
            int afterMergeSlice = 0;
            ReadOnlySpan<byte> remainder;
            Span<byte> stackSpan;

            unsafe
            {
                byte* stackBytes = stackalloc byte[stackLength];
                stackSpan = new Span<byte>(stackBytes, stackLength);
            }

            var poisition = Position.First;
            while (source.TryGet(ref poisition, out var sourceBuffer, true))
            {
                Span<byte> outputSpan = destination.Buffer;
                ReadOnlySpan<byte> sourceSpan = sourceBuffer.Span;

                if (!remainder.IsEmpty)
                {
                    int leftOverBytes = remainder.Length;
                    remainder.CopyTo(stackSpan);
                    int amountToCopy = Math.Min(sourceSpan.Length, stackSpan.Length - leftOverBytes);
                    sourceSpan.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));
                    int amountOfData = leftOverBytes + amountToCopy;

                    Span<byte> spanToTransform = stackSpan.Slice(0, amountOfData);

                    TryTransformWithRemainder:
                    TransformationStatus status = transformation.Transform(spanToTransform, outputSpan, out int bytesConsumed, out int bytesWritten);
                    if (status != TransformationStatus.Done)
                    {
                        destination.Advance(bytesWritten);
                        spanToTransform = spanToTransform.Slice(bytesConsumed);

                        if (status == TransformationStatus.DestinationTooSmall)
                        {
                            destination.Enlarge();  // output buffer is too small
                            outputSpan = destination.Buffer;

                            if (outputSpan.Length - bytesWritten < 3)
                            {
                                return; // no more output space, user decides what to do.
                            }
                            goto TryTransformWithRemainder;
                        }
                        else
                        {
                            if (status == TransformationStatus.InvalidData)
                            {
                                continue; // source buffer contains invalid bytes, user decides what to do for fallback
                            }

                            // at this point, status = TransformationStatus.NeedMoreSourceData
                            // left over bytes in stack span
                            remainder = spanToTransform;
                        }
                        continue;
                    }
                    else    // success
                    {
                        afterMergeSlice = bytesConsumed - remainder.Length;
                        remainder = Span<byte>.Empty;
                        destination.Advance(bytesWritten);
                        outputSpan = destination.Buffer;
                    }
                }

                TryTransform:
                TransformationStatus result = transformation.Transform(sourceSpan.Slice(afterMergeSlice), outputSpan, out int consumed, out int written);
                afterMergeSlice = 0;
                destination.Advance(written);
                sourceSpan = sourceSpan.Slice(consumed);

                if (result == TransformationStatus.Done) continue;

                // Not successful
                if (result == TransformationStatus.DestinationTooSmall)
                {
                    destination.Enlarge();  // output buffer is too small
                    outputSpan = destination.Buffer;
                    if (outputSpan.Length - written < 3)
                    {
                        return; // no more output space, user decides what to do.
                    }
                    goto TryTransform;
                }
                else
                {
                    if (result == TransformationStatus.InvalidData)
                    {
                        continue; // source buffer contains invalid bytes, user decides what to do for fallback
                    }

                    // at this point, result = TransformationStatus.NeedMoreSourceData
                    // left over bytes in source span
                    remainder = sourceSpan;
                }
            }
            return;
        }
    }
}
