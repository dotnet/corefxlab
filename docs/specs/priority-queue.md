# Priority queue proposal

## User data

Before anything is proposed, let's start with what can be prioritized...

### Input types

We want to be able to support all of the following configurations:

1. User data *is separate* from the priority (two physical instances).
2. User data *contains* the priority (as one or more properties).
3. User data *is* the priority (implements `IComparable<T>`).
4. Rare case: priority is obtainable via some other logic (resides in an object different from the user data).

In this entire document, I will refer to the cases above with **(1)**, **(2)**, **(3)**, and **(4)**.

### Ideal solution

Obviously, our solution should be flexible enough to (respectively):

1. Simply accept two separate instances. The user should not be forced to create a wrapper class for the two types only because of our API limitations.
2. Accept an element that already has the priority in it, without duplication (no copying).
3. Be able to use `IComparable<T>` and don't expect an additional priority.
4. Be able to execute some additional logic that retrieves the priority for a given element.

### Our approach

In order to be able to consume all of that, we need two types of priority queues:

* `PriorityQueue<T>`,
* `PriorityQueue<TElement, TPriority>`.

## `PriorityQueue<T>`

```csharp
public class PriorityQueue<T> : IQueue<T>
{
    public PriorityQueue();
    public PriorityQueue(IComparer<T> comparer);
    public PriorityQueue(IEnumerable<T> collection);
    public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer);

    public IComparer<T> Comparer { get; }
    public int Count { get; }

    public bool IsEmpty();
    public void Clear();
    public bool Contains(T element);

    public void Enqueue(T element);

    public T Peek();
    public T Dequeue();
    public void Remove(T element);

    public bool TryPeek(out T element);
    public bool TryDequeue(out T element);
    public bool TryRemove(T element);

    public IEnumerator<T> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator();
}
```

### Scenarios

#### (2)

Custom class with a priority inside:

```csharp
public class MyClass
{
	public double Priority { get; }
}
```

The user defines their own comparer, for example:

```csharp
var comparer = Comparer<MyClass>.Create((a, b) =>
{
    return a.Priority.CompareTo(b.Priority);
});
```

And simply uses our priority queue:

```csharp
var queue = new PriorityQueue<MyClass>(comparer);

queue.Enqueue(new MyClass());
```

#### (3)

Already comparable type:

```csharp
public class MyClass : IComparable<MyClass>
{
    public int CompareTo(MyClass other) => /* some logic */
}
```

Then simply call the default constructor (`Comparer<T>.Default` is assumed):

```csharp
var queue = new PriorityQueue<MyClass>();
```

#### (4)

Priority for `MyClass` is obtainable from some other objects, for example a dictionary. It is done analogically to **(2)**, simply by some custom logic in the comparer.

## `PriorityQueue<TElement, TPriority>`

```csharp
public class PriorityQueue<TElement, TPriority>
    : IEnumerable,
    IEnumerable<(TElement element, TPriority priority)>,
    IReadOnlyCollection<(TElement element, TPriority priority)>
{
    public PriorityQueue();
    public PriorityQueue(IComparer<TPriority> comparer);
    public PriorityQueue(IEnumerable<(TElement, TPriority)> collection);
    public PriorityQueue(IEnumerable<(TElement, TPriority)> collection, IComparer<TPriority> comparer);

    public IComparer<TPriority> Comparer { get; }
    public int Count { get; }

    public bool IsEmpty();
    public void Clear();
    public bool Contains(TElement element);
    public bool Contains(TElement element, out TPriority priority);

    public void Enqueue(TElement element, TPriority priority);

    public (TElement element, TPriority priority) Peek();
    public bool TryPeek(out TElement element, out TPriority priority);
    public bool TryPeek(out TElement element);

    public (TElement element, TPriority priority) Dequeue();
    public bool TryDequeue(out TElement element, out TPriority priority);
    public bool TryDequeue(out TElement element);

    public bool Remove(TElement element);
    public bool Remove(TElement element, out TPriority priority);
    public bool TryRemove(TElement element);
    public bool TryRemove(TElement element, out TPriority priority);

    public IEnumerator<(TElement element, TPriority priority)> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator();

    public IEnumerable<TElement> Elements { get; }
    public IEnumerable<TPriority> Priorities { get; }
}
```

### Scenario

The user has some data and priority separated — **(1)**:

```csharp
var userData = "this is a string";
var priority = 5;
```

So simply:

```csharp
var queue = new PriorityQueue<string, int>();

queue.Enqueue(userData, priority);
```

## Notes

To both priority queues:

* If the `IComparer<T>` is not delivered, `Comparer<T>.Default` is summoned.
* `Peek` and `Dequeue` throw an exception if the collection is empty.
* `TryPeek` and `TryDequeue` only return false.
* `Remove` throws an exception if the element to remove is not found.
* `TryRemove` only returns false.
* `Remove` and `TryRemove` remove only the first occurrence of the specified element.

## `IQueue<T>`

With the design above, we can address some voices regarding the introduction of `IQueue<T>`:

```csharp
public interface IQueue<T> :
    IEnumerable,
    IEnumerable<T>,
    IReadOnlyCollection<T>
{
    int Count { get; }

    void Clear();
    bool IsEmpty();

    void Enqueue(T element);

    T Peek();
    T Dequeue();

    bool TryPeek(out T element);
    bool TryDequeue(out T element);
}
```

### Notes

* Only `PriorityQueue<T>` would implement this interface.
* `IsEmpty` needs to be added to `Queue<T>`.
* Do we really need such an interface?

