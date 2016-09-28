// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct Position
    {
        public int IntegerPosition;
        public object ObjectPosition;

        public static Position End = new Position() { IntegerPosition = int.MinValue };
        public static Position Invalid = new Position() { IntegerPosition = int.MinValue + 1 };
        public static Position BeforeFirst = new Position();

        public bool IsValid {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return IntegerPosition != Invalid.IntegerPosition; }
        }
        public bool IsEnd {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return IntegerPosition == End.IntegerPosition; }
        }
    }
}
