# Priority queue proposal

[Revised 8/19/20]

# Scenario and key assumptions.

## A priority queue is a set of items with *updatable* priorities

A priority queue is used to track priorities of a *set* of distinct, unequal items (or *keys*). 
Each item (key) should be assigned a *unique* priority value.

A priority is a *value* used to prioritize
keys so the one with first priority can easily be dequeued and processed first. 

Distinct items can have the same priority. In this case which item gets dequeued first is not specified.

## Smallest priority comes first

The queues are *smallest-priority-first*, i.e. small priorities
are the 'highest' priority.

## Priorities are updatable
Priorities can be changed, and items may be added and removed from the set at any time.

## Prerequisites

### Priorities must be ordered (i.e. have an IComparer)

Priorities need a definite ordering, like numbers. There needs to be a 
'total ordering' on the set, which can optionally be supplied via the 
`IComparer<>` abstraction. If you use a numeric type like `int`, 
there is a default comparer.

### Keys in a priority queue are hashable (i.e. have an IEqualityComparer)

This can be optionally supplied via the `IEqualityComparer<>` abstraction. 
The default is to use `object.Equals()` and  `object.GetHashCode()`.

# `PriorityQueue<TKey, TPriority>`

`PriorityQueue<TKey, TPriority>` is a set of items (keys) and their 
priorities. If not otherwise specified, it will use the 'default equality 
comparer' for comparing objects of type `TKey` and a 'default comparer' 
of type `TPriority` for priority ordering. Keys in the 
set are required to be unique.

```csharp
public class PriorityQueue<TKey, TPriority> :
    ICollection<KeyValuePair<TKey key, TPriority priority>>,
    IEnumerable<KeyValuePair<TKey key, TPriority priority>>, // inherited from ICollection<KeyValuePair<TKey,TPriority>>
    IEnumerable // inherited from IEnumerable<KeyValuePair<TKey, TPriority>>
{
    public PriorityQueue();
    public PriorityQueue(IComparer<TPriority> priorityComparer);
    public PriorityQueue(IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

    public PriorityQueue(IEnumerable<TKey, TPriority> keysAndPriorities);
    public PriorityQueue(IEnumerable<TKey, TPriority> keysAndPriorities, IComparer<TPriority> priorityComparer);
    public PriorityQueue(IEnumerable<TKey, TPriority> keysAndPriorities, IComparer<TPriority> priorityComparer, IEqualityComparer<TKey> equalityComparer);

    public IComparer<TPriority> PriorityComparer { get; }
    public IEqualityComparer<TKey> EqualityComparer { get; }
    
    public TPriority Item[TKey] { get; set; } // can get priorities, update priorities, or add items and their priorities

    public TKey Dequeue();  // throws InvalidOperationException if the queue is empty (like Queue<>)
    public TKey Peek(); // throws InvalidOperationException if the queue is empty (like Queue<>)
    public void Enqueue(TKey key, TPriority priority); // throws ArgumentException if the key was already in the collection, like IDictionary<TKey,TPriority>.Add(), since  adding a distinct item twice with different priorities may be a programming error
    public bool TryDequeue(out TKey key, out TPriority priority); // returns false if the queue is empty
    public bool TryPeek(out TKey key, out TPriority priority); // returns false if the queue is empty

    // inherited from ICollection<KeyValuePair<TKey, TPriority>>
    public int Count { get; }
    public bool IsReadOnly { get; } // returns false
    public void Clear(); 
    
    // similar to IDictionary<>
    public void Add(TKey key, TPriority priority); // an alias for 'Enqueue'
    public bool Contains(TKey key);
    public bool Remove(TKey key); // Returns true if object was successfully removed.
    public bool TryGetPriority(TKey key, out TPriority priority);

    // inherited from ICollection<KeyValuePair<TKey, TPriority>>
    public void Add(KeyValuePair<TKey, TPriority> item); // an alias for 'Enqueue'
    public bool Contains(KeyValuePair<TKey, TPriority> item);
    public void CopyTo(KeyValuePair<TKey key, TPriority>[] array, int arrayIndex);
    public IEnumerator<KeyValuePair<TKey key, TPriority priority>> GetEnumerator();
    public bool Remove(KeyValuePair<TKey, TPriority> item);

    public struct Enumerator : IEnumerator<KeyValuePair<TKey key, TPriority priority>> {} // enumerates the collection in arbitrary order, but with the least element first
    {
        // member declarations omitted for brevity
    }
}
```

### Example usage

```csharp
var work = new PriorityQueue<WorkItem, int>();
WorkItem workItem = new WorkItem();
work.Enqueue(workItem, 5); //adds it with an initial priority
WorkItem nonUrgentWorkItem = new WorkItem();
work.Enqueue(nonUrgentworkItem, 6); //adds it with an initial priority
KeyValuePair workAndPriority = work.Peek(); // sees item with priority 5 at the front
work[workItem] = 7; //updates priority, now its less urgent
WorkItem todoFirst = work.Dequeue(); //dequeues item with the lowest priority, 6
if (work.TryDequeue(out var todoNext, out var priority)) // true, gets item with priority 7
{
...
```

## Add Time Complexities
| Operation         | Complexity    | Notes    |
| ----------------- | --------------| -------- |
| Construct         |    Θ(1)       ||
| Construct Using IEnumerable   | Θ(n)        ||
| Enqueue           | 	Θ(log n)    ||
| Peek              | 	  Θ(1)      ||
| Dequeue           | 	Θ(log n)    ||
| UpdatePriority (of one item)    | 	O(log n)    ||
| Count             |     Θ(1)      ||
| Clear             |     O(n)      ||
| CopyTo            |     Θ(n)      | Uses Array.Copy, actual complexity may be lower |
| ToArray           |     Θ(n)      | Uses Array.Copy, actual complexity may be lower |
| GetEnumerator     |     O(1)      ||
| Enumerator.MoveNext   |   O(1)    ||

## General Notes

* When `IComparer<T>` or `IEqualityComparer<T>` is not provided, `Comparer<T>.Default` or `IEqualityComparer<T>.Default` would be used.
* `TryPeek` and `TryDequeue` is modelled on the Queue class behavior, returning false if used when the collection is empty.
* `Peek` and `Dequeue` throw an exception if used when the collection is empty.
* `Add` behavior for PriorityQueue is modelled on Collection.Add behavior and must either succeed or throw
* `Add` and `Enqueue` behavior for PriorityQueue is modelled on `Dictionary<,>.Add()`, not `Queue<T>.Enqueue()`, because PriorityQueue keys have a uniqueness constraint.
* `Remove` returns false when the item to remove is not found, modelled on `Collection<>` and `Dictionary<,>` behavior

## FAQ 
* Question: Why are the type parameters different for the two optional comparers of `PriorityQueue<TKey, TPriority>` - `IEqualityComparer<TKey>` but with `IComparer<TPriority>`?
* Answer: Because they serve different purposes. Equality comparison is needed for locating unique items (keys) in the collection and updating their priority. Relative priority comparison with `IComparer<TPriority>` is needed so that items can be arranged by priority internally, for dequeueing in priority order.

* Question: Why no new interfaces? Don't we need an interface since there could be different implementations of priority queue / priority dictionary? 
* Answer: it would probably be *OK* to have a general priority queue interface for allowing developers to write algorithms that can be reused with different implementations of priority queue. But we doubt the framework will support multiple implementations of priority queues that developers will choose between with significantly different characteristics AND still have exactly the same interface. So this is cut for now.

* Question: Why smallest priority first?
* Answer: its more convenient for shortest-path algorithms.

* Question: Why doesn't the enumerator return the elements in sorted order?
* Answer: so that it doesn't have to modify or copy the collection. Sort the collection with OrderBy etc if you need to iterate it in sorted order, or use a SortedSet.

* Question: Why isn't it thread-safe or concurrency optimized?
* Answer: like many standard collections, this class is optimized for simplicity and simple single-threaded applications

* Question: Why not call it 'heap'? Or priority set? Or priority dictionary?
* Answer: this opitimizes people's ability to find the API. Priority queue is a popular *and* specific term to search for. Heap has another common meaning in programming, making it harder to search for.

* Question: Why isn't it implementing an IQueue interface (type)?
* Answer: there's no such interface today, and it didn't seem valuable enough to retrofit one.
