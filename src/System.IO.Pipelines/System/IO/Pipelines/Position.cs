// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO.Pipelines
{
    [DebuggerDisplay("{Segment}[{Index}]")]
    public readonly struct Position : IEquatable<Position>
    {
        internal readonly object Segment;
        internal readonly int Index;

        internal Position(object segment, int index)
        {
            Segment = segment;
            Index = index;
        }

        internal bool IsDefault => Segment == null;

        public static bool operator ==(Position c1, Position c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Position c1, Position c2)
        {
            return !c1.Equals(c2);
        }

        public bool Equals(Position other)
        {
            return other.Segment == Segment && other.Index == Index;
        }

        public override bool Equals(object obj)
        {
            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            var h1 = Segment?.GetHashCode() ?? 0;
            var h2 = Index.GetHashCode();

            var shift5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)shift5 + h1) ^ h2;
        }

        internal T Get<T>()
        {
            switch (Segment)
            {
                case null:
                    return default;
                case T segment:
                    return segment;
            }

            PipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.UnexpectedSegmentType);
            return default;
        }
    }
}
