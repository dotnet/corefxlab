// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public readonly struct Position : IEquatable<Position>
    {
        readonly object _item;
        readonly int _index;

        public static Position Create<T>(T item, int index = 0) where T : class
            => item == null ? default : new Position(index, item);

        public static implicit operator Position(int index) => new Position(index, null);

        public static explicit operator int(Position position) => position._index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T item, int index) Get<T>() {
            var item = _item == null ? default : (T) _item;
            return (item, _index);
        }

        public static bool operator ==(Position left, Position right) => left._index == right._index && left._item == right._item;
        public static bool operator !=(Position left, Position right) => left._index != right._index || left._item != right._item;

        public static Position operator +(Position value, int index)
            => new Position(value._index + index, value._item);

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
            this==default ? "(default)" : _item == null ? $"{_index}" : $"{_index}, {_item}";

        private Position(int index, object item)
        {
            _item = item;
            _index = index;
        }
        private Position(int index)
        {
            _item = null;
            _index = index;
        }
    }
}
