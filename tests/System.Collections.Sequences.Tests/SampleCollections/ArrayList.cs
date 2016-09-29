
namespace System.Collections.Sequences
{
    // This type is illustrating how to implement the new enumerable on index based datastructure
    public sealed class ArrayList<T> : ISequence<T>
    {
        ResizableArray<T> _items;

        public ArrayList()
        {
            _items = new ResizableArray<T>(0);
        }
        public ArrayList(int capacity)
        {
            _items = new ResizableArray<T>(capacity);
        }

        public int Length => _items.Count;

        public T this[int index] => _items[index];

        public void Add(T item)
        {
            _items.Add(item);
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }

        public bool TryGet(ref Position position, out T item, bool advance = false)
        {
            return _items.TryGet(ref position, out item, advance);
        }
    }
}
