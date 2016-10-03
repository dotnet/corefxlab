// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    public struct Position : IEquatable<Position>
    {
        public int IntegerPosition;
        public object ObjectPosition;

        public static Position BeforeFirst = new Position() { IntegerPosition = -1 };
        public static Position First = new Position();
        public static Position AfterLast = new Position() { IntegerPosition = int.MaxValue - 1 };

        public static bool operator==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Position left, Position right)
        {
            return left.Equals(right);
        }

        public bool Equals(Position other)
        {
            return IntegerPosition == other.IntegerPosition && ObjectPosition == other.ObjectPosition;
        }

        public override int GetHashCode()
        {
            return ObjectPosition == null ? IntegerPosition.GetHashCode() : ObjectPosition.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is Position)
                return base.Equals((Position)obj);
            return false;
        }
    }
}
