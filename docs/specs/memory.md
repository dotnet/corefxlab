# `Memory<T>`

## Introduction

`Memory<T>` is a type complementing [Span\<T>](span.md). As discussed in its
design document, `Span<T>` is a stack-only type. The stack-only nature of
`Span<T>` makes it unsuitable for many scenarios that require storing references
to buffers (represented with `Span<T>`) on the heap, e.g. for routines doing
asynchronous calls.

```c#
async Task DoSomethingAsync(Span<byte> buffer) {
    buffer[0] = 0;
    await Something(); // Oops! The stack unwinds here, but the buffer below
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
`Memory<T>` value. In a sense, `Memory<T>` is a factory of `Span<T>`.

## Design

A naive design of `Memory<T>` could look like the following:

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

As you might remember from the `Span<T>` design document, we need to be able to
use it to represent buffers rented from a pool, yet be sure that the buffers are
not used after they are returned to the pool.

```c#
byte[] array = _pool.Rent(size);
var buffer = new Span<byte>(array);
DoSomething(buffer);
_pool.Return(array); // we can return it safely; DoSomething could not store
                     // the buffer for later.
```

`Span<T>` makes it very easy to meet the requirement: once the stack unwinds
above where the `Span<T>` value was constructed, there cannot be any references
to the `Span<T>`. `Memory<T>` does not provide such guarantee.

```c#
byte[] array = _pool.Rent(size);
var buffer = new Memory<byte>(array);
DoSomething(buffer); // so DoSomething could store the buffer in a static, for
                     // example.
_pool.Return(array); // if we return it here, we risk use-after-free bugs.
```

We need a different mechanism (than stack-only restrictions) to be able to
manage the lifetime of buffers pointed to by `Memory<T>`.

### Lifetime

We will use indirection, to regain control over the lifetime of buffers
represented by `Memory<T>`:

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
can revoke `Memory<T>`'s ability to get Span\<T> values.

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

The design based on OwnedMemory\<T\> has several limitations that we might want
to address. The limitations and potential solutions are described in this
section.

### Pooling OwnedMemory\<T\>

A new OwnedMemory\<T\> instance must be allocated for every revocation, i.e.
although buffers can be pooled, the instances of OwnedMemory\<T\> cannot. The
instances cannot be pooled because they cannot be reused (resurrected). If
OwnedMemory\<T\> was resurrected, all previous Memory\<T\> values that were
supposed to be invalidated when the memory was returned to the pool,
would point to the new reused OwnedMemory\<T\>.
```c#
// pseudocode
OwnedMemory<byte> owned1 = s_stack.Pop(); // rent from pool
Memory<byte> memory = owned.Memory;
owned.Dispose();
s_stack.Push(owned); // return to pool
OwnedMemory<byte> owned2 = s_stack.Pop().Resurrect(); // rent again
memory.Span[0] = 0; // this should fail; owned1 was returned to the pool
```
We could address this limitation by storing a unique ID in OwnedMemory\<T\> when
it's rented from a pool, and giving this ID to all Memory\<T\> instances created
from this OwnedMemory\<T\>. All Memory\<T\> instances would have to pass this ID
to OwnedMemory\<T\> when requesting operations that access the memory.
The IDs would have to match for the operation to succeed.
The ID in OwnedMemory\<T\> would be changed when OwnedMemory\<T\> is disposed
and returned to the pool, at which point all existing Memory\<T\> instances
could not access it anymore.
```C#
public struct Memory<T> {
    OwnedMemory<T> _owner;
    long _id;
    int _index;
    int _length;

    public Span<T> Span => _owner.GetSpanInternal(_id).Slice(_index, _length);
}
public class OwnedMemory<T> {
    int _id = GenerateNewId();

    public Memory<T> Memory => new Memory<T>(this, Id);

    internal Span<T> GetSpanInternal(long id) {
        if (_id != id) ThrowIdHelper();
            return Span;
        }
    }

    public void Dispose() => _id = int.MinValue;

    public void Resurrect() => _id = GenerateNewId();
}
```
The main negatives of this approach are additional complexity and larger size of
Memory<T> instances.  The current prototype in the corfxlab repo implements this feature: https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/Buffers/Memory.cs#L14

### IOwnedMemory\<T\>

OwnedMemory\<T\> is a wrapper over actual memory buffers.
When wrapping T[], System.String, and other reference type buffers,
we end up with two managed objects for each instance of OwnedMemory\<T\>.
If OwnedMemory\<T\> were an interface, the interface could be implemented
directly by types representing memory buffers, and such types could directly
interoperate with Memory\<T\> (and so Span\<T\>).
```c#
// pseudocode
public class String : IOwnedMemory<char> {
   Span<T> IOwnedMemory<char>.GetSpan() { ... } // this is called by Memory\<T\>
}
...
string str = "Hello World";
ReadOnlySpan<char> substring = str.Memory.Slice(5, 10);
```
A proof of concept of such feature is implemented in https://github.com/KrzysztofCwalina/corefxlab/blob/Utf8String2/src/System.Text.Utf8/System/Text/Utf8/Utf8String2.cs

This feature has the following tradeoffs:

1. (good) It allows reference types that cannot inherit from OwnedMemory\<T\> to
directly interoperate with Memory\<T\> without the need to allocate wrappers.
2. (bad) This feature does not work with types that need object pooling support,
as there is no indirection between Memory\<T\> and IOwnedMemory\<T\> instances.
3. (ugly) The feature makes some of the hot path method calls (in particular
IOwnedMemory\<T\>.GetSpan) virtual/interface dispatch. We have some anecdotal
evidence this is unacceptable in some performance critical scenarios.

### Safe Dispose

It's unsafe to call OwnedMemory.Dispose while a separate thread operates on a
`Span<T>` retrieved from the Memory\<T\> value.
```c#
var owned = new OwnedNativeMemory(1024);
var memory = owned.Memory;
var span1 = memory.Span; // of course works as it should
Task.Run(()=>{   
    var span2 = memory.Span;
    // the following line is unsafe, if the Dispose call (below) executes at
    // this point.
    span2[0] = 0;
});
owned.Dispose();
var span3 = memory.Span; // fails as it should because the memory instance is
                         // now pointing to Disposed OwnedMemory\<T\>.
```
In other words, the design is subject to a race condition when one thread uses a
Span\<T\> created before its OwnedMemory\<\T> is disposed, but accessed after it
is disposed.

We could address this issue with reference counting Span\<T\> when it is created
from Memory\<T\>.  When OwnedMemory\<T\>.Dispose is called, we would check the
reference count, and fail the call if the count was positive.

But reference counting comes with all its drawbacks:

1. It significantly impacts performance. In our case, every Memory\<T\>.Span
call would result in reference counting.
2. Unless we implement automatic reference counting, we would be risking leaks
resulting from hand written code not releasing the counts.

To illustrate, let's consider the following method that formats an int into a
memory buffer:
```c#
public static bool Append(this Formatter formatter, int value) {
    Memory<T> memory = formatter.Buffer;
    int bytesWritten;
    while(!PrimitiveFormatter.TryFormat(memory.Span, value, out bytesWritten) {
        formatter.Resize();
    }
    formatter.Advance(bytesWritten);
}
```
A real example of such routine can be found at:
https://github.com/dotnet/corefxlab/blob/master/src/System.Text.Formatting/System/Text/Formatting/IOutputExtensions.cs#L152

If we reference counted all calls to retrieve Span\<T\> from Memory\<T\>, the
routine would look like the following:
```c#
public static bool Append(this Formatter formatter, int value) {
    Memory<T> memory = formatter.Buffer;
    int bytesWritten;
    using(var reservation = memory.AddReference()) {
        while(!PrimitiveFormatter.TryFormat(reservation.Span, value, out bytesWritten) {
            formatter.Resize();
        }
    } // releases reference
    formatter.Advance(bytesWritten);
}
```
This would result in expensive reference count manipulation for every integer
being written.

Alternatively, formatters (and other types that need to access Span\<T\>) could
increment the reference when they are instantiated, but then users of such
formatters would have to ensure that the formatters be disposed properly or
risk memory leaks, or we would need C# to support ARC at least for some special
types (e.g. ref-like/stack-only types).

## Issues/To-Design
1. Do we want the "pooling ID"? (see section above)
2. Reference Counting (see section above)
3. IOwnedMemory\<T\>? (see section above)
4. Should Owned`Memory<T>`.Reserve be virtual and allow mutating ReadOnlyMemory?
   See [here](../../src/System.Slices/System/Buffers/OwnedMemory.cs#L114).
5. Should we add an indexer to `Memory<T>`?
