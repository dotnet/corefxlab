# Memory\<T\>

## Introduction

Memory\<T\> is a type complementing
[Span\<T\>](https://github.com/dotnet/corefxlab/blob/master/docs/Span.md). As
discussed in its design document, Span\<T\> is a stack-only type. The stack-only
nature of Span\<T\> makes it unsuitable for many scenarios that require storing
references to buffers (represented with Span\<T\>) on the heap, e.g. for
routines doing asynchronous calls.

```c#
async Task DoSomethingAsync(Span<byte> buffer) {
    buffer[0] = 0;
    await Something(); // Oops! The stack unwinds here, but the buffer bellow
                       // cannot survive the continuation.
    buffer[0] = 1;
}
```

To address this problem, we will provide a set of complementary types, intended
to be used as general purpose exchange types representing, just like Span\<T>, a
range of arbitrary memory, but unlike Span\<T> these types will not be
stack-only, at the cost of significant performance penalties for reading and
writing to the memory.

```c#
async Task DoSomethingAsync(Memory<byte> buffer) {
    buffer.Span[0] = 0;
    await Something(); // The stack unwinds here, but it's OK as Memory<T> is
                       // just like any other type.
    buffer.Span[0] = 1;
}
```
In the sample above, the Memory\<byte\> is used to represent the buffer. It is a
regular type and can be used in methods doing asynchronous calls. Its Span
property returns Span\<byte\>, but the returned value does not get stored on the
heap during asynchronous calls, but rather new values are produced from the
Memory\<T\> value. In a sense, Memory\<T\> is a factory of Span\<T\>.

## Design

A naive design of Memory\<T\> could look like the following:

```c#
public struct Memory<T> {
    void* _ptr;
    T[]   _array;
    int   _offset;
    int   _length;

    public Span<T> Span => _ptr == null
                                 ? new Span<T>(_array, _offset, _length)
                                 : new Span<T>(_ptr, _length);
}
```

As you might remember from the Span\<T\> design document, we need to be able to
use it to represent buffers rented from a pool, yet be sure that the buffers are
not used after they are returned to the pool.

```c#
byte[] array = _pool.Rent(size);
var buffer = new Span<byte>(array);
DoSomething(buffer);
_pool.Return(array); // we can return it safely; DoSomething could not store
                     // the buffer for later.
```

Span\<T\> makes it very easy to meet the requirement: once the stack unwinds
above where the Span\<T\> value was constructed, there cannot be any references
to the Span\<T\>. Memory\<T\> does not provide such guarantee.

```c#
byte[] array = _pool.Rent(size);
var buffer = new Memory<byte>(array);
DoSomething(buffer); // so DoSomething could store the buffer in a static, for example.
_pool.Return(array); // if we return it here, we risk use-after-free bugs.
```

We need a different mechanism (than stack-only restrictions) to be able to
manage the lifetime of buffers pointed to by Memory\<T\>.

### Lifetime

We will use indirection, to regain control over the lifetime of buffers
represented by Memory\<T\>:

```c#
public struct Memory<T> {
    OwnedMemory<T> _owned;

    public Memory(OwnedMemory<T> owned) { ... }
    public Span<T> Span => _owned.Span;
}

public class OwnedMemory<T> {
    void* _ptr;
    T[]   _array;
    int   _offset;
    int   _length;

    public Span<T> Span => _ptr == null
                                 ? new Span<T>(_array, _offset, _length)
                                 : new Span<T>(_ptr, _length);

    public void Dispose() {
        _ptr = null;
        _array = null;
    }
}
```

The design above improves the control we have over rented buffers. We at least
can revoke Memory\<T\>'s ability to get Span\<T> values.

```c#
byte[] array = _pool.Rent(size);
var owned = new OwnedMemory<byte>(array);
var buffer = new Memory<byte>(owned);
DoSomething(buffer); // so DoSomething can still store the buffer in a static
owned.Dispose(); // but a call Dispose makes such stored Memory<T> useless
_pool.Return(array); // // we can return it safely; calls to owned.Span will fail
```

## Basic API Surface
```c#
public struct Memory<T> : IEquatable<Memory<T>>, IEquatable<ReadOnlyMemory<T>> {
    public Span<T> Span { get; }
    public int Length { get; }

    public Memory<T> Slice(int index);
    public Memory<T> Slice(int index, int length);
    public static implicit operator ReadOnlyMemory<T> (Memory<T> memory);

    public void CopyTo(Memory<T> memory);
    public void CopyTo(Span<T> span);

    public T[] ToArray();

    public static Memory<T> Empty { get; }
    public bool IsEmpty { get; }
}
public struct ReadOnlyMemory<T> : IEquatable<Memory<T>>, IEquatable<ReadOnlyMemory<T>> {
    public ReadOnlySpan<T> Span { get; }
    public int Length { get; }

    public ReadOnlyMemory<T> Slice(int index);
    public ReadOnlyMemory<T> Slice(int index, int length);

    public void CopyTo(Memory<T> memory);
    public void CopyTo(Span<T> span);

    public T[] ToArray();

    public static ReadOnlyMemory<T> Empty { get; }
    public bool IsEmpty { get; }
}
public class OwnedMemory<T> : IDisposable {
    protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer=null);

    public Memory<T> Memory { get; }
    public Span<T> Span { get; }

    public void Dispose();
    public bool IsDisposed { get; }

    public int Length { get; }
    protected T[] Array { get; }
    protected int Offset { get; }
    protected IntPtr Pointer { get; }
}
```

## Bonus Features

The design based on OwnedMemory<T> has several limitations that we might want to
address:

1. A new OwnedMemory instance must be allocated for every revocation, i.e.
   although buffers can be pooled, the instances of OwnedMemory cannot. The
   instances cannot be pooled because they cannot be reused (resurrected). If
   OwnedMemory was resurrected, all previous Memory<T> values that were supposed
   to be invalidated would point to the new reused OwnedMemory.

2. It's unsafe to call OwnedMemory.Dispose while a separate thread operates on a
   Span\<T\> retrieved from the Memory\<T> value.

The following sections describe variants of the basic design addressing these
limitations.

### Pooling Owned Memory\<T\>
TBD

### Safe Dispose
TBD

## Issues/To-Design

1. Should OwnedMemory\<T\>.Reserve be virtual and allow mutating ReadOnlyMemory?
   See
   [here](https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/Buffers/OwnedMemory.cs#L114).
2. Should we add an indexer to Memory\<T\>?
