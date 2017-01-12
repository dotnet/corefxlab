# System.Threading.Tasks.Channels

The System.Threading.Tasks.Channels library provides a set of synchronization data structures 
for passing data between producers and consumers.  Whereas the existing System.Threading.Tasks.Dataflow 
library is focused on pipelining and connecting together dataflow "blocks" which encapsulate both storage and 
processing,  System.Threading.Tasks.Channels is focused purely on the storage aspect, with data structures used
to provide the hand-offs between participants explicitly coded to use the storage. The library is designed to be 
used with async/await in C#.

## Base Classes

The library is centered around the ```Channel<T>``` abstract base class, which represents a data structure
that can have instances of ```T``` written to it and read from it.  The class is actually the
combination of a few classes that represent the reading and writing halves:
```C#
public abstract class Channel<T> : Channel<TWrite, TRead> { }

public abstract class Channel<TWrite, TRead>
{
	public abstract ReadableChannel<TRead> In { get; }
	public abstract WritableChannel<TWrite> Out { get; }
	...
}

public abstract class ReadableChannel<T>
{
    public abstract bool TryRead(out T item);
    public abstract ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    public abstract Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    public abstract Task Completion { get; }
    public virtual ValueAwaiter<T> GetAwaiter();
	...
}

public interface WritableChannel<in T>
{
    public abstract bool TryWrite(T item);
    public abstract Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken));
    public abstract Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));
    public abstract bool TryComplete(Exception error = null);
	public virtual ValueAwaiter<bool> GetAwaiter();
	...
}
```
The ```ReadableChannel<T>``` and ```WritableChannel<T>``` types represent the two halves of a channel,
exposed as ```In``` and ```Out``` properties on the ```Channel<T>```, with the former allowing data to be read from it
and the latter allowing data to be written to it. The readable and writable types mirror each other, with operations exposed
on each that are counterparts of those on the other:
- ```TryRead```/```TryWrite```: Attempt to read or write an item synchronously, returning whether the read or write was successful.
- ```ReadAsync```/```WriteAsync```: Read or write an item asynchronously.  These will complete synchronously if data/space is already available.
- ```WaitToReadAsync```/```WaitToWriteAsync```: Return a ```Task<bool>``` that will complete when reading or writing can be attempted.  If 
the task completes with a ```true``` result, at that moment the channel was available for reading or writing, though 
because these channels may be used concurrently, it's possible the status changed the moment after the operation completed.
If the task completes with a ```false``` result, the channel has been completed and will not be able to satisfy a read or write.
- ```TryComplete```/```Completion```: Channels may be completed, such that no additional items may be written; such channels will "complete" when
marked as completed *and* all existing data in the channel has been consumed.  Channels may also be marked as faulted by passing
an optional ```Exception``` to Complete; this exception will emerge when awaiting on the Completion Task, as well as when trying
to ```ReadAsync``` from an empty completed collection.

```ReadAsync``` is defined to return a ```ValueTask<T>```, a struct type that's a  discriminated 
union of a ```T``` and a ```Task<T>```, making it allocation-free for ```ReadAsync<T>``` to synchronously return a ```T``` value it has 
available (in contrast to using ```Task.FromResult<T>```, which needs to allocate a ```Task<T>``` instance).   ```ValueTask<T>``` is awaitable, 
so most consumption of instances will be indistinguishable from with a ```Task<T>```.  If a ```Task<T>``` is needed, one can be retrieved using 
```ValueTask<T>```'s ```AsTask()``` method, which will either return the ```Task<T>``` it stores internally or will return an instance created
from the ```T``` it stores.

```ReadableChannel<T>``` also exposes a ```GetAwaiter``` method, making it directly awaitable for consuming an element from a channel, e.g.
```C#
T result = await c.In;
```
```GetAwaiter``` returns a ```ValueAwaiter<T>```, a struct type that's a discriminated union of a ```T``` and some other async operation,
either a ```Task<T>``` or an ```IAwaiter<T>```.  This allows channels to synchronously return data without allocating when data is
available synchronously.  It also enables channels to provide optimized implementations for asynchronous completion, but using a custom
```IAwaiter<T>``` as the underlying awaiter implementation.

Similarly, ```WritableChannel<T>``` exposes a ```GetAwaiter```, making it directly awaitable for determining whether writing can be
performed, as with ```WaitForWriteAsync```, e.g.
```C#
bool canWrite = await c.Out;
```

## Core Channels

A type may be derived from any one of these classes to be considered a channel; if it derives from ```Channel<T>``` or
```Channel<TWrite,TRead>```, then it's usable for both reading and writing, whereas if it derives from ```ReadableChannel<T>```
or from ```WritableChannel<TWrite>```, it's only usable for reading or writing, respectively.  Several core channels are built-in
to the library and represent the most commonly needed concrete implementations; more may be added in time.  These channels are created
via ```Create*``` factory methods on the static ```Channel``` type:
```C#
public static class Channel
{
    public static Channel<T> CreateUnbounded<T>();
    public static Channel<T> CreateUnbuffered<T>();
    public static Channel<T> CreateBounded<T>(int bufferedCapacity, BoundedChannelFullMode mode);
	...
}
```
- ```CreateUnbounded<T>```: Used to create a buffered, unbounded channel.  The channel may be used concurrently
by any number of readers and writers, and is unbounded in size, limited only by available memory. As such, as long as The
channel has not been marked for completion, writes will always complete synchronously and successfully, e.g. ```TryWrite``` will
return true, ```WaitForWriteAsync``` will synchronously return a ```Task<bool>``` with a result of true, ```WriteAsync``` will
synchronously return a completed task, etc.
- ```CreateBounded<T>```: Used to create a buffered channel that stores at most a specified number of items.  The channel
may be used concurrently by any number of reads and writers.  Attempts to write to the channel when it contains the maximum
allowed number of items results in behavior according to the ```mode``` specified when the channel is created, and can include
waiting for space to be available, dropping the oldest item (the one written longest ago still stored in the channel), or dropping
the newest item (the one written most recently).
- ```CreateUnbuffered<T>```: Used to create an unbuffered channel.  Such a channel has no internal storage for ```T``` items,
instead pairing up writers and readers to rendezvous and directly hand off their data from one to the other.  ```TryWrite``` operations
will only succeed if there's currently an outstanding ```ReadAsync``` operation, and conversely ```TryRead``` operations will only succeed if there's
currently an outstanding ```WriteAsync``` operation.  ```ReadAsync``` and ```WriteAsync``` operations will complete once the counterpart operation
arrives to satisfy the request.

These ```Create``` methods may also have overloads accepting a ```ChannelOptimizations``` type, which provides options
around optimizations applied to these channels.  These optimizations come with tradeoffs, which is why they're not just always
applied.  For example, the ```SingleReader``` option may be set to true if the developer can guarantee that there will only ever
be one read operation in flight on the channel at a time; in exchange, the implementation returned from the ```Create``` method
may provide faster throughput, taking advantage of the "single reader" constraint.  As another example, the ```AllowSynchronousContinuations```
option enables operations performed on a channel to synchronously (rather than the default of asynchronously) invoke continuations subscribed
to notifications of pending async operations, e.g. if a ```TryWrite``` on a channel may synchronously invoke a continuation off
a task previously returned from a ```ReadAsync``` call. This can provide measurable throughput improvements by avoiding
scheduling additional work items; however, it may come at the cost of reduced parallelism, as for example a producer
may then be the one to execute work associated with a consumer, and if not done thoughtfully, this can lead to unexpected interactions.

## Example Producer/Consumer Patterns

As an example, a producer that wrote N integers to a channel once a second and then marked the channel as being completed
might look like this:
```C#
private static async Task ProduceRange(WritableChannel<int> c, int count)
{
    for (int i = 0; i < count; i++)
	{
	    await c.WriteAsync(i);
	}
	c.Complete();
}
```
That will wait for each write to complete successfully before moving on to the next write.  Alternatively, ```TryWrite``` could
be used if the developer wants to do something instead of writing if the item can't be immediately transferred to the channel:
```C#
private static async Task ProduceRange(WritableChannel<int> c, int count)
{
    for (int i = 0; i < count; i++)
	{
	    bool success = c.TryWrite(i);
		if (!success) { ... }
	}
	c.Complete();
}
```
Or if the developer wants to simply wait for the channel to be ready to accept another write, a loop like the following
could be employed:
```C#
private static async Task ProduceRange(WritableChannel<int> c, int count)
{
    for (int i = 0; i < count; i++)
	{
	    while (await c.WaitForWriteAsync())
		{
			if (c.TryWrite(i)) break;
		}
	}
	c.Complete();
}
```
And if it's expected that most writes will succeed synchronously and we may only need to wait every once in a while,
it may also be advantageous to loop on the ```TryWrite```, e.g.
```C#
private static async Task ProduceRange(WritableChannel<int> c, int count)
{
	int i = 0;
	while (i < count && await c.WaitForWriteAsync())
	{
	    while (i < count && c.TryWrite(i)) i++;
	}
	c.Complete();
}
```
Finally, if no asynchronous waiting was desired and instead the developer wanted to spin, a loop like the following
could be employed:
```C#
private static async Task ProduceRange(WritableChannel<int> c, int count)
{
    var sw = new SpinWait();
	for (int i = 0; i < count; i++)
	{
		while (!c.TryWrite(i)) sw.SpinOnce();
	}
	c.Complete();
}
```

On the consumer end, there are similarly multiple ways to consume a channel, and which one is chosen will depend on the
exact needs of the situation.  The channel can be awaited directly to read from it, in which case if the channel ends up
being marked as completed, an exception will be thrown when no more reads are possible, indicating the closure:
```C#
private static async Task Consume(ReadableChannel<int> c)
{
    try
	{
	     while (true)
		 {
		     int item = await c;
			 ...
		 }
	}
	catch (ChannelClosedException) {}
}
```
Alternatively, ReadAsync may be used instead of awaiting the channel (this behaves similarly to the above await, albeit
with a few differences in options and performance.  ```ReadAsync``` is more flexible, in that it accepts an optional
```CancellationToken```, making the read operation cancelable, and returns a ```ValueTask<T>```, which may be used in
situations where either a ```ValueTask<T>``` or a ```Task<T>``` is needed, such as in waiting for multiple operations
with a ```Task.WhenAll```.  However, some implementations may have a more optimized implementation of ```GetAwaiter```
that takes advantage of the constraints associated with it):
```C#
private static async Task Consume(ReadableChannel<int> c)
{
    try
	{
	     while (true)
		 {
		     int item = await c.ReadAsync();
			 ...
		 }
	}
	catch (ChannelClosedException) {}
}
```
```WaitForReadAsync``` and ```TryRead``` may also be used.  This avoids the use of an exception to indicate if/when the channel
has been closed, e.g.
```C#
private static async Task Consume(ReadableChannel<int> c)
{
    while (await c.WaitForReadAsync())
	{
		if (c.TryRead(out int item))
		{
		    ...
		}
    }
}
```
As with the writing example, if it's expected that waiting will be relatively rare, a nested loop can be used to
optimize for reads by looping over ```TryRead``` as well:
```C#
private static async Task Consume(ReadableChannel<int> c)
{
    while (await c.WaitForReadAsync())
	{
		while (c.TryRead(out int item))
		{
		    ...
		}
    }
}
```
And for circumstances where spinning instead of waiting is desired, as with writing that can be done as well:
```C#
private static async Task Consume(ReadableChannel<int> c)
{
    var sw = new SpinWait();
	Task completion = c.Completion;
	while (!completion.IsCompleted)
	{
	    while (c.TryRead(out int item))
		{
		    ...
		}
		sw.SpinOnce();
	}
}
```
As noted earlier, a channel is not considered to be completed (meaning its ```Completion``` task won't transition
to a completed state) until the channel has both been marked for completion by the writer and has no more data
available, making the above polling/spinning loop safe from a concurrency perspective.

## Additional Support for Reading

In addition to reading from a channel via the ```TryRead```/```ReadAsync```/```WaitForReadAsync``` / ```GetAwaiter``` methods,
the library also includes a prototype for an ```IAsyncEnumerator<T>```, which can be used to asynchronously
read all of the data out of a channel:
```C#
IAsyncEnumerator<T> e = channel.GetAsyncEnumerator();
while (await e.MoveNextAsync())
{
     Use(e.Current);
}
```
If C# were to gain support for such async enumerators (e.g. https://github.com/dotnet/roslyn/issues/261), 
such iteration could be performed using those language features, e.g. with hypothetical syntax:
```C#
foreach (await T item in channel)
{
    Use(item);
}
```

### Integration With Existing Types

```ReadableChannel<T>``` and ```WritableChannel<T>``` also provide virtual methods for interop between channels and observables / observers:
```C#
public abstract class ReadableChannel<T>
{
    public virtual IObservable<T> AsObservable<T>();
	...
}
public abstract class WritableChannel<T>
{}
    public virtual IObserver<T> AsObserver<T>();
	...
}
```
This allows for subscribing a writable channel to an observable as an observer, and subscribing other observers 
to a readable channel as an observable.  With this support, IObservable-based LINQ queries can be written against
data in channels.

## Selecting

```Channel``` also serves as an entry point for building up case-select constructs, where pairs of channels
and associated delegates are provided, with the implementation asynchronously waiting for data or space to
be available in the associated channel and then executing the associated delegate.
```C#
public static class Channel
{
    public static CaseBuilder CaseRead<T>(ReadableChannel<T> channel, Action<T> action);
    public static CaseBuilder CaseRead<T>(ReadableChannel<T> channel, Func<T, Task> func);
    public static CaseBuilder CaseWrite<T>(WritableChannel<T> channel, T item, Action action);
    public static CaseBuilder CaseWrite<T>(WritableChannel<T> channel, T item, Func<Task> func);
	...
}
```
The ```CaseBuilder``` that's returned provides additional operations that can be chained on, providing a fluent interface:
```C#
public sealed class CaseBuilder
{
    public CaseBuilder CaseRead<T>(ReadableChannel<T> channel, Action<T> action)
    public CaseBuilder CaseRead<T>(ReadableChannel<T> channel, Func<T, Task> func)

    public CaseBuilder CaseWrite<T>(WritableChannel<T> channel, T item, Action action)
    public CaseBuilder CaseWrite<T>(WritableChannel<T> channel, T item, Func<Task> func)

    public CaseBuilder CaseDefault(Action action)
    public CaseBuilder CaseDefault(Func<Task> func)

    public Task<bool>  SelectAsync(CancellationToken cancellationToken = default(CancellationToken))
    public Task<int>   SelectUntilAsync(Func<int, bool> conditionFunc, CancellationToken cancellationToken = default(CancellationToken))
}
```
For example, we can build up a case-select that will read from the first of three channels that has data available,
executing just one of their delegates, and leaving the other channels intact:
```C#
Channel<int>    c1 = Channel.Create<int>();
Channel<string> c2 = Channel.CreateUnbuffered<string>();
Channel<double> c3 = Channel.Create<double>();
...
await Channel
    .CaseRead(c1, i => HandleInt32(i))
	.CaseRead(c2, s => HandleString(s))
	.CaseRead(c3, d => HandleDouble(d))
	.SelectAsync();
```
A ```CaseDefault``` may be added for cases where some default action should be performed if the other cases 
aren't satisfiable immediately.

Additionally, as it's potentially desirable to want to select in a loop (a ```CaseBuilder``` may be reused
for multiple select operations), a ```SelectUntilAsync``` is provided that performs such a loop internally
until a particular condition is met.  For example, this will read all of the data out of all of the previously
instantiated channels until all of the channels are completed:
```C#
await Channel
    .CaseRead(c1, i => HandleInt32(i))
	.CaseRead(c2, s => HandleString(s))
	.CaseRead(c3, d => HandleDouble(d))
	.SelectUntilAsync(_ => true);
```
whereas this will stop after 5 items have been processed:
```C#
await Channel
    .CaseRead(c1, i => HandleInt32(i))
	.CaseRead(c2, s => HandleString(s))
	.CaseRead(c3, d => HandleDouble(d))
	.SelectUntilAsync(completions => completions < 5);
```

## Relationship with TPL Dataflow

The ```BufferBlock<T>``` type in System.Threading.Tasks.Dataflow provides functionality similar to that of ```Channel.CreateUnbounded<T><T>()```
and ```Channel.CreateBounded<T>(...)``` (based on whether the ```BufferBlock<T>``` is constructed with a bound).
However, ```BufferBlock<T>``` is designed and optimized for use as part of dataflow chains with other dataflow blocks.  The Channels
library is more focused on the specific scenario of handing data off between open-coded producers and consumers, and is optimized
for that scenario, further expanding on the kinds of such buffers available and with implementations geared towards the relevant
consumption models.

When these Channel interfaces become part of corefx, System.Threading.Tasks.Dataflow may take a dependency on 
System.Threading.Tasks.Channels as a lower-level set of abstractions, replacing internal implementation details with these channels.
It's also possible that several of the blocks in System.Threading.Tasks.Dataflow could be modified to relate to channels in the public API,
enabling direct integration between the libraries.  For example, ```BufferBlock<T>```  could derive from ```Channel<T>```,
```ActionBlock<T>``` could derive from ```WritableChannel<T>```, etc., and/or methods could be added to support linking dataflow blocks
to channels.