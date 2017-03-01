// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.IO.Pipelines
{
    public static class ReadCursorOperations
    {
        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0)
        {
            var enumerator = new SegmentEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var segmentPart = enumerator.Current;
                var segment = segmentPart.Segment;
                var span = segment.Memory.Span.Slice(segmentPart.Start, segmentPart.Length);

                int index = span.IndexOfVectorized(byte0);
                if (index != -1)
                {
                    result = new ReadCursor(segment, segmentPart.Start + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1)
        {
            var enumerator = new SegmentEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var segmentPart = enumerator.Current;
                var segment = segmentPart.Segment;
                var span = segment.Memory.Span.Slice(segmentPart.Start, segmentPart.Length);

                int index1 = span.IndexOfVectorized(byte0);
                int index2 = span.IndexOfVectorized(byte1);

                var index = MinIndex(index1, index2);
                if (index != -1)
                {
                    result = new ReadCursor(segment, segmentPart.Start + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1, byte byte2)
        {
            var enumerator = new SegmentEnumerator(begin, end);
            while (enumerator.MoveNext())
            {
                var segmentPart = enumerator.Current;
                var segment = segmentPart.Segment;
                var span = segment.Memory.Span.Slice(segmentPart.Start, segmentPart.Length);

                int index1 = span.IndexOfVectorized(byte0);
                int index2 = span.IndexOfVectorized(byte1);
                int index3 = span.IndexOfVectorized(byte2);

                var index = MinIndex(index1, index2, index3);

                if (index != -1)
                {
                    result = new ReadCursor(segment, segmentPart.Start + index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        private static int MinIndex(int v1, int v2)
        {
            v1 = v1 == -1 ? int.MaxValue : v1;
            v2 = v2 == -1 ? int.MaxValue : v2;
            var result = Math.Min(v1, v2);
            return result == int.MaxValue ? -1 : result;
        }

        private static int MinIndex(int v1, int v2, int v3)
        {
            v1 = v1 == -1 ? int.MaxValue : v1;
            v2 = v2 == -1 ? int.MaxValue : v2;
            v3 = v3 == -1 ? int.MaxValue : v3;
            var result = Math.Min(Math.Min(v1, v2), v3);
            return result == int.MaxValue ? -1 : result;
        }
    }
}
