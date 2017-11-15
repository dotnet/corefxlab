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
        object _item;

        public static explicit operator int(Position position) => (int)position._index;

        public static implicit operator long(Position position) => position._index;

        public long GetIndex() => _index;

        public T GetItem<T>() => _item == null || IsEnd ? default : (T)_item;

        public (T item, long index) Get<T>() => (GetItem<T>(), this);

        public void SetIndex(long index) => _index = index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItem<T>(T item) where T:class
        {
            if (item == null) this = End;
            else _item = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set<T>(T item, long index) where T : class
        {
            if (item == null) this = End;
            else
            {
                _item = item;
                _index = index;
            }
        }

        public void Advance<T>(T item, long offset) where T:class
        {
            if (item == null) this = End;
            else
            {
                _item = item;
                _index += offset;
            }
        }

        public static Position operator +(Position position, long offset) => new Position(position._item, position._index + offset);

        public static Position operator -(Position position, long offset) => new Position(position._item, position._index - offset);

        public static readonly Position End = new Position(new object(), long.MaxValue);

        public bool IsEnd => this == End;

        public static bool operator ==(Position left, Position right) => left._index == right._index && left._item == right._item;
        public static bool operator !=(Position left, Position right) => left._index != right._index || left._item != right._item;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(Position position) => this == position;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) =>
            obj is Position ? this == (Position)obj : false;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
            _index.GetHashCode() ^ (_item == null ? 0 : _item.GetHashCode());

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() =>
            _item == null ? @"{_index}" : @"{_index}, {_obj}";

        private Position(object obj, long index)
        {
            _item = obj;
            _index = index;
        }
    }
}
