// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    static class Extensions
    {
        public static ArraySegment<T> Slice<T>(this ArraySegment<T> source, int count)
        {
            var result = new ArraySegment<T>(source.Array, source.Offset + count, source.Count - count);
            return result;
        }
    }
}
