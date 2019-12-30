# Priority queue proposal

Before proposing the classes, let's explain the scenarios they meet...

## Scenario 1 - Priority queue - data items *are* intrinsically priorities

They can be numbers, or any other `IComparable<>` type. Importantly, comparing
any two items must *always return the same result*. In other words, relative item 
priorities never change. So there is no 'update priority' operation.

## Scenario 2 - Priority queue - data items have *updatable* priorities

Sometimes a priority queue is used to track *changing* priority of a *set* of 
unique objects (or *keys*). Priorities can change, and items may be added and 
removed at any time. A priority is an *attached value* which just prioritizes 
which object (or key) will be removed first.

This is like a dictionary where the objects are the unique *keys*, and the 
priorities are their *values* - with the addition of being able to easily 
dequeue items in priority order.

## Note: smallest priority comes first

The queues are *smallest-priority-first*, i.e. small priorities
are the 'highest' priority.

## Prerequisites

### Priorities must have an IComparer 

Priorities have to function like numbers. There needs to be a 
'total ordering' on the set, which can optionally be supplied via the 
`IComparer<>` abstraction. If you use a numeric type like `int`, 
there is a default comparer.

### Keys in a priority dictionary are equality comparable (and hashable)

This can be optionally supplied via the `IEqualityComparer<>` abstraction. 
But by default it uses `Equals()` and  `GetHashCode()`.

# `PriorityQueue<T>`

`PriorityQueue<T>` is a queue of items, where the least item (according to 
the comparer) will be dequeued first. Duplicates are allowed.

```csharp
public class PriorityQueue<T>:
    ICollection<T>
    IEnumerable<T> // inherited from ICollection<T>
    IEnumerable
{
    public PriorityQueue();
    public PriorityQueue(IComparer<T> priorityComparer);

    public PriorityQueue(IEnumerable<T> items);
    public PriorityQueue(IEnumerable<T> items, IComparer<T> priorityComparer);

    // (inherited from ICollection<T>)
    public int Count { get; }
    public bool IsReadOnly { get; } // returns false

    // new
    public IComparer<T> Comparer { get; } // compares priorities using .CompareTo(T)
    
    // (inherited from IEnumerable<T>)
    public IEnumerator<T> GetEnumerator(); // enumeration is not in strict priority order, so that it is O(n) (inherited from IEnumerable<T>)
    public IEnumerator GetEnumerator();

    // (inherited from ICollection<T>)
    public void Add(T item); // synonym for Enqueue
    public void Clear();
    public bool Contains(T item); // performance is worst-case O(n)
    public void CopyTo (T[] array, int arrayIndex); // performance is O(n)
    public bool Remove(T item); // removes the object from the collection. Returns true if successfully removed.

    // new
    public void Enqueue(T item); // performance is O(log n)
    public T Dequeue();  // performance is O(log n) throws InvalidOperationException if the queue is empty (like Queue<>)
    public T Peek(); // performance is O(1) throws InvalidOperationException if the queue is empty (like Queue<>)
    public bool TryPeek(out T item); // returns false if the queue is empty (like Queue<>)
    public bool TryDequeue(out T item); // returns false if the queue is empty (like Queue<>)

    public struct Enumerator : IEnumerator<T> {} // enumerates the collection in priority order
    {
        // member declarations omitted for brevity
    }
}
```

### Example usage

```csharp
var queue = new PriorityQueue<int>();
queue.Enqueue(3);
queue.Enqueue(2);
queue.Enqueue(5);
queue.Enqueue(3);
Assert.Equal(2, queue.Dequeue());
Assert.Equal(3, queue.Dequeue());
Assert.Equal(3, queue.Dequeue());
Assert.Equal(5, queue.Dequeue());
Assert.True(queue.Empty());
```

# `PriorityDictionary<TKey, TPriority>`

`PriorityDictionary<TKey, TPriority>` is a dictionary of items and their 
priorities. If not otherwise  specified, it will use a 'default equality 
comparer' for comparing objects (of type `TKey`) and a 'default priority comparer' 
for comparing priorities (of type `TPriority`). Items or keys in the 
dictionary must be unique.

```csharp
public class PriorityDictionary<TKey, TPriority> :
    IDictionary<TKey, TPriority>,
    ICollection<KeyValuePair<TKey key, TPriority priority>>, // inherited from IDictionary<TKey,TPriority>
    IEnumerable<KeyValuePair<TKey key, TPriority priority>>, // inherited from ICollection<KeyValuePair<TKey,TPriority>>
    IEnumerable // inherited from IEnumerable<KeyValuePair<TKey, TPriority>>
{
    public PriorityDictionary();
    public PriorityDictionary(IComparer<TPriority> priorityComparer);
    public PriorityDictionary(IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

    public PriorityDictionary(IEnumerable<TKey, TPriority> dictionary);
    public PriorityDictionary(IEnumerable<TKey, TPriority> dictionary, IComparer<TPriority> priorityComparer);
    public PriorityDictionary(IEnumerable<TKey, TPriority> dictionary, IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

    // new
    public IComparer<TPriority> PriorityComparer { get; }
    public IEqualityComparer<TKey> EqualityComparer { get; }
    public ICollection<TPriority> Priorities { get; } // an alias for 'IDictionary<TKey, TPriority>.Values'

    // inherited from IDictionary<TKey, TPriority>
    public int Count { get; }
    public bool IsReadOnly { get; } // returns false
    public TPriority Item[TKey] { get; set; } // get or update priorities, can also be used to add keys and their priorities to the queue
    public ICollection<TKey> Keys { get; } // collection of keys, in arbitrary order
    public ICollection<TKPriority> Values { get; } // colletion of priorities, in arbitrary order

    // inherited from IDictionary<TKey, TPriority>
    public void Add(TKey key, TPriority priority); // throws ArgumentException if the key was already in the collection
    public void Clear(); 
    public bool ContainsKey(TKey key);
    public bool Remove(TKey key); // Returns true if object was successfully removed.
    public bool TryGetValue(TKey key, out TPriority priority);

    // inherited from ICollection<KeyValuePair<TKey, TPriority>>
    public void Add(KeyValuePair<TKey, TPriority> item); // throws ArgumentException if the key was already in the collection
    public bool Contains(KeyValuePair<TKey, TPriority> item);
    public void CopyTo(KeyValuePair<TKey key, TPriority>[] array, int arrayIndex);
    public IEnumerator<KeyValuePair<TKey key, TPriority priority>> GetEnumerator();
    public bool Remove(KeyValuePair<TKey, TPriority> item);

    // new
    public KeyValuePair<TKey, TPriority> Dequeue();  // throws InvalidOperationException if the queue is empty (like Queue<>)
    public void Enqueue(TKey key, TPriority priority); // an alias for IDictionary<TKey,TPriority>.Add()
    public KeyValuePair<TKey, TPriority> Peek(); // throws InvalidOperationException if the queue is empty (like Queue<>)
    public bool TryDequeue(out TKey key, out TPriority priority);
    public bool TryPeek(out TKey key, out TPriority priority); // returns false if the queue is empty

    public struct Enumerator : IEnumerator<KeyValuePair<TKey key, TPriority priority>> {} // enumerates the collection in priority order
    {
        // member declarations omitted for brevity
    }
}
```

### Example usage

```csharp
WorkItem workItem = new WorkItem(/* initial parameters */);
var work = new PriorityDictionary<WorkItem, int>();
work.Enqueue(workItem, priority: 5); //adds with initial priority
work[workItem] = 7; //updates priority
workItem todo = work.Dequeue();
```

## General Notes

* When `IComparer<T>` or `IEqualityComparer<T>` is not provided, `Comparer<T>.Default` or `IEqualityComparer<T>.Default` would be used.
* `TryPeek` and `TryDequeue` is modelled on the Queue class behavior, returning false if used when the collection is empty.
* `Peek` and `Dequeue` throw an exception if used when the collection is empty.
* `Add` behavior for PriorityQueue is modelled on Collection.Add behavior and always succeeds
* `Add` and `Enqueue` behavior for PriorityDictionary is modelled on Dictionary.Add(), not Queue<T>.Enqueue because keys have a uniqueness constraint.
* `Remove` returns false when the item to remove is not found, modelled on Collection<T> behavior

## FAQ 
* Question: Why does `PriorityDictionary<TKey,TPriority>.Add()` throw for duplicate keys, but `PriorityQueue<T>.Add()` does not?
* Answer: Because each `Add` method actually conforms to a different interface, with different behavior, and those have different requirements on uniqueness! PriorityQueue conforms to the `ICollection<T>.Add(T item)` interface, whereas PriorityDictionary must conform to the `IDictionary<TKey, TValue>.Add(TKey key, TValue value)` interface.

* Question: Why are the type parameters different for the two optional comparers of `PriorityDictionary<TKey, TPriority>` - `IEqualityComparer<TKey>` but with `IComparer<TPriority>`?
* Answer: Because they serve different purposes. Equality comparison is needed for locating unique items (keys) in the collection and updating their priority. Relative priority comparison with `IComparer<TPriority>` is needed so that items can be partially-sorted internally, and then dequeued in priority order.

* Question: Why no new interfaces? Don't we need an interface since there could be different implementations of priority queue / priority dictionary? 
* Answer: it would be *nice* to have a general priority queue interface for allowing developers to write algorithms that can be reused with different implementations of priority queue. They might have slightly different/better algorithmic complexity, in plug-together fashion. But there could be an adoption issue - not sure it would be widely useful without multiple implementations of PQ already in the framework / or other popular libraries, and not sure they will have different *enough* performance to actually matter in applications. So this is cut for now.

* Question: Why smallest priority first?
* Answer: mainly becuase its convenient for shortest-path algorithms.