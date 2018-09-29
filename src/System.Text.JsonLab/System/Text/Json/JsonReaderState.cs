// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.JsonLab
{
    public struct JsonReaderState : IEquatable<JsonReaderState>
    {
        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        internal ulong _containerMask;
        internal Stack<bool> _stack;
        internal bool _inObject;
        internal int _depth;
        internal JsonTokenType _tokenType;
        internal bool _searchedNextLast;
        internal int _lineNumber;
        internal int _position;

        public override bool Equals(object obj)
        {
            return obj is JsonReaderState && Equals((JsonReaderState)obj);
        }

        public bool Equals(JsonReaderState other)
        {
            return _containerMask == other._containerMask &&
                   EqualityComparer<Stack<bool>>.Default.Equals(_stack, other._stack) &&
                   _inObject == other._inObject &&
                   _depth == other._depth &&
                   _tokenType == other._tokenType &&
                   _searchedNextLast == other._searchedNextLast &&
                   _lineNumber == other._lineNumber &&
                   _position == other._position;
        }

        public override int GetHashCode()
        {
            int hashCode = -1676708545;
            hashCode = hashCode * -1521134295 + _containerMask.GetHashCode();
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            hashCode = hashCode * -1521134295 + _tokenType.GetHashCode();
            hashCode = hashCode * -1521134295 + _lineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + _position.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(JsonReaderState left, JsonReaderState right)
            => left.Equals(right);

        public static bool operator !=(JsonReaderState left, JsonReaderState right)
            => !left.Equals(right);
    }
}
