// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct Position : IEquatable<Position>
    {
        public int IntegerPosition;
        public object ObjectPosition;

        public static Position BeforeFirst = new Position() { IntegerPosition = -1 };
        public static Position First = new Position();
        public static Position AfterLast = new Position() { IntegerPosition = int.MaxValue - 1 };

        //public bool IsValid {
        //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //    get { return IntegerPosition <= AfterLast.IntegerPosition; }
        //}
        //public bool IsEnd {
        //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //    get { return IntegerPosition == AfterLast.IntegerPosition; }
        //}

        public bool Equals(Position other)
        {
            return IntegerPosition == other.IntegerPosition && ObjectPosition == other.ObjectPosition;
        }
    }
}
