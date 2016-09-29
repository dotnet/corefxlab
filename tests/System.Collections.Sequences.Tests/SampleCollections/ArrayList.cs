
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

        public int Length {
            get {
                return _items.Count;
            }
        }

        public T this[int index] {
            get {
                return _items[index];
            }
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T GetAt(ref Position position, bool advance = false)
        {
            return _items.GetAt(ref position, advance);
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }
    }
}
