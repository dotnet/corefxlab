# System.Threading.Tasks.Channels

The System.Threading.Tasks.Channels library provides a set of synchronization data structures 
for passing data between producers and consumers.  Whereas the existing System.Threading.Tasks.Dataflow 
library is  focused on pipelining and connecting together dataflow "blocks" which encapsulate both storage and 
processing,  System.Threading.Tasks.Channels is focused purely on the storage aspect, with data structures used
to provide the hand-offs between participants explicitly coded to use the storage. The library is designed to be 
used with async/await in C#.

## Interfaces

The library is centered around the ```IChannel<T>``` interface, which represents a data structure
that can have instances of ```T``` written to it and read from it.  The interface is actually the
combination of a few other interfaces that represent the reading and writing halves:
```C#
public interface IChannel<T> : IChannel<T, T> { }

public interface IChannel<TInput, TOutput> : IWritableChannel<TInput>, IReadableChannel<TOutput> { }

public interface IReadableChannel<T>
{
    ValueAwaiter<T> GetAwaiter();
    bool TryRead(out T item);
    ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task Completion { get; }
}

public interface IWritableChannel<in T>
{
    bool TryWrite(T item);
    Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));
    bool TryComplete(Exception error = null);
}
```
The ```IReadableChannel<T>``` and ```IWritableChannel<T>``` types represent these two halves,
with the former allowing data to be read from it and the latter allowing data to be written to it.
The interfaces mirror each other, with operations exposed on each that are counterparts of those
on the other:
- ```TryRead```/```TryWrite```: Attempt to read or write an item synchronously, returning whether the read or write was successful.
- ```ReadAsync```/```WriteAsync```: Read or write an item asynchronously.
- ```WaitToReadAsync```/```WaitToWriteAsync```: Return a ```Task<bool>``` that will complete when reading or writing can be attempted.  If 
the task completes with a ```true``` result, at that moment the channel was available for reading or writing, though 
because these channels may be used concurrently, it's possible the status changed the moment after the operation completed.
If the task completes with a ```false``` result, the channel has been completed and will not be able to satisfy a read or write.
- ```TryComplete```/```Completion```: Channels may be completed, such that no additional items may be written; such channels will "complete" when
marked as completed and all existing data in the channel has been consumed.  Channels may also be marked as faulted by passing
an optional ```Exception``` to Complete; this exception will emerge when awaiting on the Completion Task, as well as when trying
to ```ReadAsync``` from an empty completed collection.

```ReadAsync``` is defined to return a ```ValueTask<T>```, a struct type that's a  discriminated 
union of a ```T``` and a ```Task<T>```, making it allocation-free for ```ReadAsync<T>``` to synchronously return a ```T``` value it has 
available (in contrast to using ```Task.FromResult<T>```, which needs to allocate a ```Task<T>``` instance).   ```ValueTask<T>``` is awaitable, 
so most consumption of instances will be indistinguishable from with a ```Task<T>```.  If a ```Task<T>``` is needed, one can be retrieved using 
```ValueTask<T>```'s ```AsTask()``` method, which will either return the ```Task<T>``` it stores internally or will return an instance created
from the ```T``` it stores.

```IReadableChannel<T>``` also exposes a ```GetAwaiter``` method, making it directly awaiting for consuming an element from a channel, e.g.
```C#
var result = await c;
```
```GetAwaiter``` returns a ```ValueAwaiter<T>```, a struct type that's a discriminated union of a ```T``` and some other async operation,
either a ```Task<T>``` or an ```IAwaiter<T>```.  This allows channels to synchronously return data without allocating when data is
available synchronously.  It also enables channels to provide optimized implementations for asynchronous completion, but using a custom
```IAwaiter<T>``` as the underlying awaiter implementation.

## Core Channels

Any type may implement one or more of these interfaces to be considered a channel.  However, several core channels are built-in
to the library.  These channels are created via factory methods on the ```Channel``` type:
```C#
public static class Channel
{
    public static IChannel<T> CreateUnbounded<T>();
    public static IChannel<T> CreateUnboundedSpsc<T>();
    public static IChannel<T> CreateUnbuffered<T>();
    public static IChannel<T> CreateBounded<T>(int bufferedCapacity, BoundedChannelFullMode mode);
	...
}
```
- ```CreateUnbounded<T>```: Used to create a buffered, unbounded channel.  The channel may be used concurrently
by any number of readers and writers, and is unbounded in size, limited only by available memory.
- ```CreateUnboundedSpsc<T>```: Like ```CreateUnbounded<T>```, except it returns a channel that is usable by only a single Writer
and a single reader at a time.  This constraint enables a more efficient implementation, but it is up to the code using the channel
to guarantee that the constraint is met.
- ```CreateUnbuffered<T>```: Used to create an unbuffered channel.  Such a channel has no internal storage for ```T``` items,
instead pairing up writers and readers to rendezvous and directly hand off their data from one to the other.  TryWrite operations
will only succeed if there's currently an outstanding ReadAsync operation, and conversely TryRead operations will only succeed if there's
currently an outstanding WriteAsync operation.  ReadAsync and WriteAsync operations will complete once the counterpart operation
arrives to satisfy the request.
- ```CreateBounded<T>```: Used to create a buffered channel that stores at most a specified number of items.  The Channel
may be used concurrently by any number of reads and writers.  Attempts to write to the channel when it contains the maximum
allowed number of items results in behavior according to the ```mode``` specified when the channel is created, and can include
waiting for space to be available, dropping the oldest item (the one written longest ago still stored in the channel), or dropping
the newest item (the one written most recently).

### Integration With Existing Types

```ChannelExtensions``` provides helper methods for working with channels, including support for interop between channels and observables / observers:
```C#
public static class ChannelExtensions
{
    public static IObservable<T> AsObservable<T>(this IReadableChannel<T> source);
    public static IObserver<T> AsObserver<T>(this IWritableChannel<T> target);
	...
}
```
This allows for subscribing a writeable channel to an observable as an observer, and subscribing other observers 
to a readable channel as an observable.  With this support, IObservable-based LINQ queries can be written against
data in channels.

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

## Selecting

```Channel``` also serves as an entry point for building up case-select constructs, where pairs of channels
and associated delegates are provided, with the implementation asynchronously waiting for data or space to
be available in the associated channel and then executing the associated delegate.
```C#
public static class Channel
{
    public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action);
    public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func);
    public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action);
    public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func);
	...
}
```
The ```CaseBuilder``` that's returned provides additional operations that can be chained on, providing a fluent interface:
```C#
public sealed class CaseBuilder
{
    public CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action)
    public CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func)

    public CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action)
    public CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func)

    public CaseBuilder CaseDefault(Action action)
    public CaseBuilder CaseDefault(Func<Task> func)

    public Task<bool>  SelectAsync(CancellationToken cancellationToken = default(CancellationToken))
    public Task<int>   SelectUntilAsync(Func<int, bool> conditionFunc, CancellationToken cancellationToken = default(CancellationToken))
}
```
For example, we can build up a case-select that will read from the first of three channels that has data available,
executing just one of their delegates, and leaving the other channels intact:
```C#
IChannel<int>    c1 = Channel.Create<int>();
IChannel<string> c2 = Channel.CreateUnbuffered<string>();
IChannel<double> c3 = Channel.Create<double>();
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

The ```BufferBlock<T>``` type in System.Threading.Tasks.Dataflow provides functionality similar to that of ```Channel.Create<T>()```.
However, ```BufferBlock<T>``` is designed and optimized for use as part of dataflow chains with other dataflow blocks.  The Channels
library is more focused on the specific scenario of handing data off between open-coded producers and consumers, and is optimized
for that scenario, further expanding on the kinds of such buffers available and with implementations geared towards the relevant
consumption models.

If these Channel interfaces were to become part of corefx, System.Threading.Tasks.Dataflow would likely take a dependency on 
System.Threading.Tasks.Channels as a lower-level set of abstractions, and several of the blocks in System.Threading.Tasks.Dataflow 
would likely be modified to implement them as well, enabling direct integration between the libraries.  For example, ```BufferBlock<T>``` 
would  likely implement ```IChannel<T>```, ```ActionBlock<T>``` would likely implement ```IWritableChannel<T>```, 
```TransformBlock<TInput,TOutput>``` would likely implement ```IChannel<TInput, TOutput>```, etc.
