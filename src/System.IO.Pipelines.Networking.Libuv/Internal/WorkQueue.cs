using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.IO.Pipelines.Networking.Libuv.Internal
{
    // Lock free linked list that for multi producers and a single consumer
    internal class WorkQueue<T>
    {
        private Node _head;

        public void Add(T value)
        {
            Node node = new Node(), oldHead;
            node.Value = value;

            do
            {
                oldHead = _head;
                node.Next = _head;
                node.Count = 1 + (oldHead?.Count ?? 0);
            } while (Interlocked.CompareExchange(ref _head, node, oldHead) != oldHead);
        }


        public Enumerable DequeAll()
        {
            // swap out the head
            var node = Interlocked.Exchange(ref _head, null);

            // we now have a detatched head, but we're backwards
            // note: 0/1 are a trivial case
            if (node == null || node.Count == 1)
            {
                return new Enumerable(node);
            }
            // otherwise, we need to reverse the linked-list
            // note: use the iterative method to avoid a stack-dive
            Node prev = null;
            int count = 1; // rebuild the counts
            while (node != null)
            {
                var next = node.Next;
                node.Next = prev;
                node.Count = count++;
                prev = node;
                node = next;
            }
            return new Enumerable(prev);
        }

        public struct Enumerable : IEnumerable<T>
        {
            private Node _node;
            public int Count => _node?.Count ?? 0;
            internal Enumerable(Node node)
            {
                _node = node;
            }
            public Enumerator GetEnumerator() => new Enumerator(_node);
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        }

        public struct Enumerator : IEnumerator<T>
        {
            private Node _next;
            private T _current;
            internal Enumerator(Node node)
            {
                _current = default(T);
                _next = node;
            }
            object IEnumerator.Current => _current;
            public T Current => _current;

            void IDisposable.Dispose() { }

            public bool MoveNext()
            {
                if (_next == null)
                {
                    _current = default(T);
                    return false;
                }
                _current = _next.Value;
                _next = _next.Next;
                return true;
            }
            public void Reset() { throw new NotSupportedException(); }
        }

        internal class Node // need internal for Enumerator / Enumerable
        {
            public T Value;
            public Node Next;
            public int Count;
        }

    }
}