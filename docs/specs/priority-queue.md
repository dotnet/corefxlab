# Priority queue proposal

[Revised 8/11/20]

# Scenario and key assumptions.

## A priority queue is a set of items with *updatable* priorities

A priority queue is used to track priorities of a *set* of distinct objects (or *keys*). 
A priority is a *value* used to prioritize
keys so the one with first priority can easily be dequeued and processed first. 
Any key should be assigned a *unique* priority value.

You may notice this sounds a bit like a dictionary where the objects are the 
unique *keys*, and the priorities are their *values* - with the minor addition of being 
able to easily dequeue items in priority order.

## Smallest priority comes first

The queues are *smallest-priority-first*, i.e. small priorities
are the 'highest' priority. This is practical for many algorithms.

## Priorities are updatable
Priorities can change, and items may be added and 
removed from the set at any time. This is optimizing for convenience.

## Prerequisites

### Priorities must be ordered (i.e. have an IComparer)

Priorities need a definite ordering, like numbers. There needs to be a 
'total ordering' on the set, which can optionally be supplied via the 
`IComparer<>` abstraction. If you use a numeric type like `int`, 
there is a default comparer.

### Keys in a priority dictionary are hashable (i.e. have an IEqualityComparer)

This can be optionally supplied via the `IEqualityComparer<>` abstraction. 
The default is to use `object.Equals()` and  `object.GetHashCode()`.

# `PriorityQueue<TKey, TPriority>`

`PriorityQueue<TKey, TPriority>` is a set (or dictionary if you like), of items and their 
priorities. If not otherwise specified, it should uss the 'default equality 
comparer' for comparing objects of type `TKey` and a 'default priority comparer' 
for comparing priorities of type `TPriority`. Keys in the 
set are required to be unique.

```csharp
public class PriorityQueue<TKey, TPriority> :
    IDictionary<TKey, TPriority>,
    ICollection<KeyValuePair<TKey key, TPriority priority>>, // inherited from IDictionary<TKey,TPriority>
    IEnumerable<KeyValuePair<TKey key, TPriority priority>>, // inherited from ICollection<KeyValuePair<TKey,TPriority>>
    IEnumerable // inherited from IEnumerable<KeyValuePair<TKey, TPriority>>
{
    public PriorityQueue();
    public PriorityQueue(IComparer<TPriority> priorityComparer);
    public PriorityQueue(IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

    public PriorityQueue(IEnumerable<TKey, TPriority> dictionary);
    public PriorityQueue(IEnumerable<TKey, TPriority> dictionary, IComparer<TPriority> priorityComparer);
    public PriorityQueue(IEnumerable<TKey, TPriority> dictionary, IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

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
    public bool TryDequeue(out TKey key, out TPriority priority); // returns false if the queue is empty
    public bool TryPeek(out TKey key, out TPriority priority); // returns false if the queue is empty

    public struct Enumerator : IEnumerator<KeyValuePair<TKey key, TPriority priority>> {} // enumerates the collection in arbitrary order, but with the least element first
    {
        // member declarations omitted for brevity
    }
}
```

### Example usage

```csharp
WorkItem workItem = new WorkItem(/* initial parameters */);
var work = new PriorityQueue<WorkItem, int>();
work.Enqueue(workItem, priority: 5); //adds it with an initial priority
work[workItem] = 7; //updates priority
WorkItem todo = work.Dequeue(); //dequeues item with the lowest priority
if (work.TryDequeue(out var todoNext, out var priority))
{
}
```

## General Notes

* When `IComparer<T>` or `IEqualityComparer<T>` is not provided, `Comparer<T>.Default` or `IEqualityComparer<T>.Default` would be used.
* `TryPeek` and `TryDequeue` is modelled on the Queue class behavior, returning false if used when the collection is empty.
* `Peek` and `Dequeue` throw an exception if used when the collection is empty.
* `Add` behavior for PriorityQueue is modelled on Collection.Add behavior and always succeeds
* `Add` and `Enqueue` behavior for PriorityQueue is modelled on Dictionary.Add(), not Queue<T>.Enqueue because keys have a uniqueness constraint.
* `Remove` returns false when the item to remove is not found, modelled on Collection<T> behavior

## FAQ 
* Question: Why does `PriorityQueue<TKey,TPriority>.Add()` throw for duplicate keys, but `PriorityQueue<T>.Add()` does not?
* Answer: Because each `Add` method actually conforms to a different interface, with different behavior, and those have different requirements on uniqueness! PriorityQueue conforms to the `ICollection<T>.Add(T item)` interface, whereas PriorityQueue must conform to the `IDictionary<TKey, TValue>.Add(TKey key, TValue value)` interface.

* Question: Why are the type parameters different for the two optional comparers of `PriorityQueue<TKey, TPriority>` - `IEqualityComparer<TKey>` but with `IComparer<TPriority>`?
* Answer: Because they serve different purposes. Equality comparison is needed for locating unique items (keys) in the collection and updating their priority. Relative priority comparison with `IComparer<TPriority>` is needed so that items can be partially-sorted internally, and then dequeued in priority order.

* Question: Why no new interfaces? Don't we need an interface since there could be different implementations of priority queue / priority dictionary? 
* Answer: it would be *nice* to have a general priority queue interface for allowing developers to write algorithms that can be reused with different implementations of priority queue. They might have slightly different/better algorithmic complexity, in plug-together fashion. But there could be an adoption issue - not sure it would be widely useful without multiple implementations of PQ already in the framework / or other popular libraries, and not sure they will have different *enough* performance to actually matter in applications. So this is cut for now.

* Question: Why smallest priority first?
* Answer: mainly becuase its convenient for shortest-path algorithms.

* Question: Why doesn't the enumerator return the elements in sorted order?
* Answer: so that it doesn't have to modify or copy the collection. You can sort the collection with OrderBy etc if you need fully sorted order, or use SortedSet.

* Question: Why doesn't the enumerator return the elements in sorted order?
* Answer: so that it doesn't have to modify or copy the collection. You can sort the collection with OrderBy etc if you need fully sorted order, or use SortedSet.

* Question: Why isn't it thread-safe or concurrency optimized?
* Answer: because its guessed that the main killer app for this is simple single-threaded algorithm implementations, not fancy high performance work schedulers.

* Question: Why not call it heap? Or priority dictionary?
* Answer: priority queue is fairly self-explanatory, and lowest common denominator in a good, accessible way. And heap has another common meaning.

* Question: Why isn't it implementing an IQueue interface?
* Answer: there's no such interface today, and it doesn't seem worth complicating this further by trying to add one now.
