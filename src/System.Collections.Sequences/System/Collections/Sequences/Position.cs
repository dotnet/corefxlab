// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct Position : IEquatable<Position>
    {
        object _item;
        public int Index { get; set; }

        public static Position Create<T>(int index, T item) where T : class
        {
            Position position = default;
            position.Set(item, index);
            return position;
        }

        public static Position Create(int index) => new Position(index, null);

        public static explicit operator int(Position position) => position.Index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetItem<T>() => _item == null || IsEnd ? default : (T)_item;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T item, int index) Get<T>() => (GetItem<T>(), (int)this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItem<T>(T item) where T : class
        {
            if (item == null) this = End;
            else _item = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set<T>(T item, int index) where T : class
        {
            if (item == null) this = End;
            else
            {
                _item = item;
                Index = index;
            }
        }

        public static readonly Position End = new Position(int.MaxValue, new object());

        public bool IsEnd => this == End;

        public static bool operator ==(Position left, Position right) => left.Index == right.Index && left._item == right._item;
        public static bool operator !=(Position left, Position right) => left.Index != right.Index || left._item != right._item;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(Position position) => this == position;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) =>
            obj is Position ? this == (Position)obj : false;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
            Index.GetHashCode() ^ (_item == null ? 0 : _item.GetHashCode());

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() =>
            _item == null ? $"{Index}" : $"{Index}, {_item}";

        private Position(int index, object item)
        {
            _item = item;
            Index = index;
        }
    }
}
