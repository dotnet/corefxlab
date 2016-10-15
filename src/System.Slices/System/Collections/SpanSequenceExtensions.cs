// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    public static class SpanSequenceExtensions
    {
        public static Span<T> Flatten<T>(this ISpanSequence<T> sequence)
        {
            var position = Position.First;
            Span<T> firstSpan;
            // if sequence length == 0
            if (!sequence.TryGet(ref position, out firstSpan, advance: true)) {
                return Span<T>.Empty;
            }
            Span<T> secondSpan;
            // if sequence length == 1
            if (!sequence.TryGet(ref position, out secondSpan, advance: true)) {
                return firstSpan;
            }

            // allocate and copy
            Span<T> result;

            // if we know the total size of the sequence
            if (sequence.TotalLength != null) {
                result = new T[sequence.TotalLength.Value];
                result.Set(firstSpan);
                result.Slice(firstSpan.Length).Set(secondSpan);
                int copied = firstSpan.Length + secondSpan.Length;
                Span<T> nextSpan;
                while (sequence.TryGet(ref position, out nextSpan, advance: true)) {
                    nextSpan.CopyTo(result.Slice(copied));
                    copied += nextSpan.Length;
                }
                return result;
            }
            else {
                var capacity = (firstSpan.Length + secondSpan.Length) * 2;
                var resizableArray = new ResizableArray<T>(capacity);
                firstSpan.CopyTo(ref resizableArray);
                secondSpan.CopyTo(ref resizableArray);
                Span<T> nextSpan;
                int copied = firstSpan.Length + secondSpan.Length;
                while (sequence.TryGet(ref position, out nextSpan, advance: true)) {
                    while (copied + nextSpan.Length > resizableArray.Capacity) {
                        var newLength = resizableArray.Capacity * 2;
                        resizableArray.Resize(newLength);
                    }
                    nextSpan.CopyTo(ref resizableArray);
                    copied += nextSpan.Length;
                }
                return resizableArray._array.Slice(0, copied);
            }
        }

        public static Span<T> First<T>(this ISpanSequence<T> sequence)
        {
            Span<T> result;
            var first = Position.First;
            if(!sequence.TryGet(ref first, out result)) {
                ThrowHelper.ThrowInvalidOperationException();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="destination">The parameter is updated if it's longer than the items in the sequence</param>
        /// <param name="skip">number of items from the begining of sequence to skip, i.e. not copy.</param>
        /// <returns>True if all items did fit in the destination, false otherwise.</returns>
        /// <remarks>If the destination is too short, up to destination.Length items are copied in, even if the function returns false.</remarks>
        public static bool TryCopyTo<T>(this ISpanSequence<T> sequence, ref Span<T> destination, int skip = 0)
        {
            var position = Position.First;
            Span<T> next;
            int copied = 0;
            while(sequence.TryGet(ref position, out next, advance : true)) {
                var free = destination.Slice(copied);

                if(skip > next.Length) {
                    skip -= next.Length;
                    continue;
                }
                if(skip > 0) {
                    next = next.Slice(skip);
                }

                if(free.Length > next.Length) {
                    free.Set(next);
                    copied += next.Length;
                }
                else {
                    free.Set(next.Slice(0, free.Length));
                    return false;
                }
            }

            destination = destination.Slice(0, copied);
            return true;
        }

        static void CopyTo<T>(this Span<T> span, ref ResizableArray<T> array)
        {
            span.CopyTo(array._array.Slice(array._count));
            array._count += span.Length;
        }
    }
}

