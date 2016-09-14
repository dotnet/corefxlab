using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
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

    public struct Enumerator<T>
    {
        Position _position;
        ISequence<T> _sequence;
        T _item;

        public Enumerator(ISequence<T> sequence)
        {
            _sequence = sequence;
            _position = Position.BeforeFirst;
            _item = default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _item = _sequence.TryGetItem(ref _position);
            if (_position.IsValid) {
                return true;
            }
            return false;
        }

        public T Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return _item;
            }
        }
    }

    // new interface
    public interface ISequence<T>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        T TryGetItem(ref Position position);

        [EditorBrowsable(EditorBrowsableState.Never)]
        Enumerator<T> GetEnumerator();
    }
}
