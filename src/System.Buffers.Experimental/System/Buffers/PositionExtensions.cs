// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Runtime.InteropServices;

namespace System.Buffers
{

    public static class PositionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T segment, int index) Get<T>(this Position position)
        {
            var segment = position.Segment == null ? default : (T)position.Segment;
            return (segment, position.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSegment<T>(this Position position)
        {
            switch (position.Segment)
            {
                case null:
                    return default;
                case T segment:
                    return segment;
            }

            throw new InvalidOperationException($"Unexpected segment type '{position.Segment.GetType()}'");
        }

        public static Position Add(this Position position, int offset)
        {
            return new Position(position.Segment, position.Index + offset);
        }
    }
}
