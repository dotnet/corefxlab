
namespace System.Collections.Sequences
{
    // This type is illustrating how to implement the new enumerable on index based datastructure
    public sealed class ArrayList<T> : ISequence<T>
    {
        T[] _array;
        int _count;

        public ArrayList()
        {
            _array = new T[0];
        }
        public ArrayList(int capacity)
        {
            _array = new T[capacity];
        }

        public int Length {
            get {
                return _count;
            }
        }

        public T this[int index] {
            get {
                return _array[index];
            }
        }

        public void Add(T item)
        {
            if (_array.Length == _count) {
                Resize();
            }
            _array[_count++] = item;
        }

        public T TryGetItem(ref Position position)
        {
            // TODO: this is too many checks
            if (!position.IsEnd && position.IntegerPosition < _count) {
                var item = _array[position.IntegerPosition++];
                if (position.IntegerPosition >= _count) position = Position.End;
                return item;
            } else {
                position = Position.Invalid;
                return default(T);
            }
        }

        void Resize()
        {
            int newLength = 4;
            if (_array != null && _array.Length != 0) {
                newLength = _array.Length << 1;
            }
            var newArray = new T[newLength];
            _array.CopyTo(newArray, 0);
            _array = newArray;
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }
    }
}
