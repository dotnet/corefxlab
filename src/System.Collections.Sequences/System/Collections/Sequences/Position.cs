// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct Position : IEquatable<Position>
    {
        long _index;
        object _obj;

        public static explicit operator int(Position position) => (int)position._index;
        public static implicit operator long(Position position) => position._index;
        public T As<T>() => _obj == null || IsInfinity ? default : (T)_obj;
        public bool IsInfinity => this == Infinity;

        public static readonly Position Infinity = new Position(new object(), long.MaxValue);

        public static Position operator +(Position position, long value) => new Position(position._obj, position._index + value);

        public static Position operator -(Position position, long value) => new Position(position._obj, position._index - value);

        public static bool operator ==(Position left, Position right) => left._index == right._index && left._obj == right._obj;
        public static bool operator !=(Position left, Position right) => left._index != right._index || left._obj != right._obj;

        public void Set(long value) => _index = value;

        public void Set(int value) => _index = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set<T>(T item)
        {
            if(EqualityComparer<T>.Default.Equals(item, default)) this = Infinity;
            else _obj = item;
        }

        private Position(object obj, long index)
        {
            _obj = obj;
            _index = index;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(Position position) => this == position;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) =>
            obj is Position ? this == (Position)obj : false;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
            _index.GetHashCode() ^ (_obj == null ? 0 : _obj.GetHashCode());

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() =>
            _obj == null ? @"{_index}" : @"{_index}, {_obj}";
    }
}
