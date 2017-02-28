// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.IO.Pipelines
{
    public static class ReadCursorOperations
    {
        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0)
        {
            var enumerator = new MemoryEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;
                var segment = enumerator.CurrentSegment;
                int index = span.IndexOfVectorized(byte0);
                if (index != -1)
                {
                    result = new ReadCursor(segment, enumerator.CurrentSegmentStartIndex + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1)
        {
            // use address of ushort rather than stackalloc as the inliner won't inline functions with stackalloc
            ushort twoBytes;
            byte* byteArray = (byte*)&twoBytes;
            byteArray[0] = byte0;
            byteArray[1] = byte1;
            var targets = new Span<byte>(byteArray, 2);
            var enumerator = new MemoryEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;
                var segment = enumerator.CurrentSegment;

                // TODO: Vectorize
                int index = span.IndexOf(targets);
                if (index != -1)
                {
                    result = new ReadCursor(segment, enumerator.CurrentSegmentStartIndex + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1, byte byte2)
        {
            // use address of uint rather than stackalloc as the inliner won't inline functions with stackalloc
            uint fourBytes;
            byte* byteArray = (byte*)&fourBytes;
            byteArray[0] = byte0;
            byteArray[1] = byte1;
            byteArray[2] = byte2;

            var targets = new Span<byte>(byteArray, 3);
            var enumerator = new MemoryEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;
                var segment = enumerator.CurrentSegment;

                // TODO: Vectorize
                int index = span.IndexOf(targets);
                if (index != -1)
                {
                    result = new ReadCursor(segment, enumerator.CurrentSegmentStartIndex + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }
    }
}
