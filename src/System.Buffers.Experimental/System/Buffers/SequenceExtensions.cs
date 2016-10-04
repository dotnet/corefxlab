// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Text;

namespace System.Buffers
{
    public static class ExperimentalSequenceExtensions
    {
        public delegate TResult FuncOfSpan<T, TResult>(Span<T> span);
        public delegate void ActionOfSpan<T>(Span<T> span);

        public static TResult Do<T, TResult>(this ISpanSequence<T> buffer, FuncOfSpan<T, TResult> function)
        {
            Span<T> flat;
            if (buffer.Count < 2) flat = buffer.Flatten();
            if (buffer.TotalLength < 128) {
                unsafe
                {
                    var stackArray = stackalloc byte[128];
                    flat = new Span<T>(stackArray, 128);
                    if (!buffer.TryCopyTo(ref flat, 0)) {
                        throw new Exception(nameof(buffer) + ".TotalLength returned bad value.");
                    }
                }
            }
            else flat = buffer.Flatten();

            return function(flat);
        }

        public static void Do<T>(this ISpanSequence<T> buffer, ActionOfSpan<T> action)
        {
            Span<T> flat;
            if (buffer.Count < 2) flat = buffer.Flatten();
            if (buffer.TotalLength < 128) {
                unsafe
                {
                    var stackArray = stackalloc byte[128];
                    flat = new Span<T>(stackArray, 128);
                    if (!buffer.TryCopyTo(ref flat, 0)) {
                        throw new Exception(nameof(buffer) + ".TotalLength returned bad value.");
                    }
                }
            }
            else flat = buffer.Flatten();

            action(flat);
        }

        public static bool TryParseUInt32<TSequence>(this TSequence bytes, EncodingData encoding, out uint value, out int consumed) where TSequence : ISpanSequence<byte>
        {
            Position position = Position.First;
            Span<byte> first;
            if (!bytes.TryGet(ref position, out first, advance: true)) {
                throw new ArgumentException("bytes cannot be empty");
            }

            if (!PrimitiveParser.TryParse(first, EncodingData.Encoding.Utf8, out value, out consumed)) {
                return false; // TODO: maybe we should continue in some cases, e.g. if the first span ends in a decimal separator
                                // ... cont, maybe consumed could be set even if TryParse returns false
            }
            if (position.Equals(Position.AfterLast) || first.Length > consumed) {
                return true;
            }

            Span<byte> second;
            if (!bytes.TryGet(ref position, out second, advance: true)) {
                throw new ArgumentException("bytes cannot be empty");
            }

            Span<byte> temp;
            int numberOfBytesFromSecond = second.Length;
            if (numberOfBytesFromSecond > 64) numberOfBytesFromSecond = 64;
            var tempBufferLength = first.Length + numberOfBytesFromSecond;
            if (tempBufferLength > 128) {
                temp = new byte[tempBufferLength];
            } else {
                unsafe
                {
                    byte* data = stackalloc byte[tempBufferLength];
                    temp = new Span<byte>(data, tempBufferLength);
                }
            }

            first.CopyTo(temp);

            second.Slice(0, numberOfBytesFromSecond).CopyTo(temp.Slice(first.Length));

            if (!PrimitiveParser.TryParse(temp, EncodingData.Encoding.Utf8, out value, out consumed)) {
                return false;
            }

            if (position.Equals(Position.AfterLast) || temp.Length > consumed) {
                return true;
            }

            throw new NotImplementedException();
        }
    }
}
