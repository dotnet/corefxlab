## Efficient I/O

As part of the high performance work, we need new abstractions for doing
efficient reading and writing to and from the network, pipes, files etc. The
pattern we have settled on so far in pipelines is a model where buffers are
pushed by a producer (`IPipelineWriter`) to a consumer (`IPipelineReader`).

```C#
public interface IPipelineReader
{
    ReadableBufferAwaitable ReadAsync();
    void Advance(ReadCursor consumed, ReadCursor examined);
    void CancelPendingRead();
    void Complete(Exception exception = null);
}
```

```C#
public interface IPipelineWriter
{
    Task Writing { get; }
    WritableBuffer Alloc(int minimumSize = 0);
    void Complete(Exception exception = null);
}
```

The producer gets a handle on a `WritableBuffer` and the consumer gets a handle
on a `ReadableBuffer` to do writes and reads respectively.

Memory in pipelines is managed as a linked list of [OwnedMemory\<T\>](https://github.com/dotnet/corefxlab/blob/master/docs/specs/memory.md):

```C#
internal class BufferSegment
{
    // The backing memory, usually leased from a pool
    OwnedMemory<byte> Data { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    // The next segment
    BufferSegment Next { get; set; }

    // The segment cannot be written to
    public bool ReadOnly { get; set; }

    public int ReadableBytes => End - Start;
    public int WritableBytes => Data.Length - End;
}
```

A `BufferSegment` represents a link in a contiguous chain of buffers. New
segments are added if the requested memory is less than the writable bytes in
the current segment. What's interesting here is that there are 2 views to a
`BufferSegment`, the ReadableBytes and the WritableBytes. The writing and
reading primitives in pipelines build on this concept.

### Writing

```C#
private async Task Produce(IPipelineWriter writer)
{
   while (true)
   {
       // Allocate a buffer from the writer
       WritableBuffer buffer = writer.Alloc();

       var bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());

       new Span<byte>(bytes).CopyTo(buffer.Memory.Span);

       buffer.Advance(bytes.Length);

       // Tell the producer we have data
       await buffer.FlushAsync();

       // Wait a second before writing again
       await Task.Delay(1000);
   }
}
```

In the above example, `Produce` calls `Alloc` on the `IPipelineWriter` to get
access to the `WritableBuffer`, then it calls `FlushAsync` to signal to the
consumer that data is ready. A `WritableBuffer` is a struct that allows writing
to an *ever* growing sink of bytes.

```C#
public struct WritableBuffer
{
   public Memory<byte> Memory { get; }
   public void Ensure(int minimumBytes = 0);
   public void Advance(int bytesWritten);
   public void Commit();
   public Task FlushAsync();
}
```

There are extension methods like `Write(ReadOnlySpan<byte>)` which automate
writing to the available memory and advancing the buffer.

```C#
public static void Write(this WritableBuffer buffer, ReadOnlySpan<byte> source)
{
    if (buffer.Memory.IsEmpty)
    {
        buffer.Ensure();
    }

    // Fast path, try copying to the available memory directly
    if (source.Length <= buffer.Memory.Length)
    {
        source.CopyTo(buffer.Memory.Span);
        buffer.Advance(source.Length);
        return;
    }

    var remaining = source.Length;
    var offset = 0;

    while (remaining > 0)
    {
        var writable = Math.Min(remaining, buffer.Memory.Length);

        buffer.Ensure(writable);

        if (writable == 0)
        {
            continue;
        }

        source.Slice(offset, writable).CopyTo(buffer.Memory.Span);

        remaining -= writable;
        offset += writable;

        buffer.Advance(writable);
    }
}
```

### Reading

The consumer calls `IPipelineReader.ReadAsync()` which returns with a
`ReadableBuffer` when the producer says that data is ready.

```C#
private async Task Consume(IPipelineReader reader)
{
    while (true)
    {
        ReadResult result = await reader.ReadAsync();
        ReadableBuffer buffer = result.Buffer;
        if (buffer.IsEmpty && result.IsCompleted)
        {
            break;
        }

        // Process the buffer
        Consume(ref buffer);

        // Advance the reader past the entire buffer
        reader.Advance(buffer.End, buffer.End);
    }
}
```

A `ReadableBuffer` is a struct that is a view over one or many `BufferSegments`:

```C#
public struct ReadCursor
{
   public int Index { get; }
   public BufferSegment Segment { get; }
}

public struct ReadableBuffer : ISequence<Memory<byte>>
{
   public ReadCursor Start { get; }
   public ReadCursor End { get; }
   public int Length { get; }

   public byte Peek();
   public ReadableBuffer Slice(ReadCursor start, ReadCursor end);
   public PreservedBuffer Preserve();
}

public struct PreservedBuffer : IDisposable
{
   public ReadableBuffer Buffer { get; }
}
```

A `ReadCursor` represents a specific coordinate within a `BufferSegment` and a
`ReadableBuffer` represents all of the data between 2 `ReadCursors`, Start and
End. The `ReadableBuffer` is a point in time snapshot of the data the producer
made available.

After processing the buffer, consumers call `IPipelineReader.Advance(consumed,
observed)` indicate how much of the buffer was consumed and how much of the
buffer was observed. The distinction is very important as consumed data can be
returned to the system for re-use by another component. Marking observed data
lets the consumer control over where it should be notified again (to an extent).

### Threading

When the producer calls `FlushAsync()` on the `WritableBuffer` that will call
the consumer on the producer's thread.

### Buffer ownership

In the examples above, data is pushed from the producer to consumer. The
consumer has ownership of the buffer between the call to
`IPipelineReader.ReadAsync()` and `IPipelineReader.Advance()`. If the consumers
needs ownership of the buffer outside of this scope, an explicit call to
`ReadableBuffer.Preserve()` must be made before calling Advance. This will
transfer ownership of the buffer to the consumer. `PreservedBuffers` must be
disposed.

## Problems
1. When writing higher level protocol parsers, there is a need for efficient low
level routines that can grab a byte at a time, and possible several bytes at a
time. Further more, to build successful ecosystem of reusable parsing routines,
we'd like these types of parsers to be built on the same primitives. Is
`ReadableBuffer` that primitive or do we need something lower?
2. Multiple buffers. Today pipelines uses a linked list of buffers and provides
a single abstraction over them, the `ReadableBuffer`. This multi buffer
abstraction means all low level routines need to be implemented against a new
multi buffer primitives or duplicated in pipelines.
3. Structs or classes? Today `WritableBuffer` is tied to the underlying
`IPipelineWriter` it was allocated from, this is to enable passing it to other
methods without explicitly passing it by reference. This is very limiting as the
type is now tied to the underlying writer. `ReadableBuffer` has the same
problem. After peeking at bytes in the buffer, you have to make a new slice
today because there's no good way to *move* the `ReadableBuffer`. This leads to
lots of struct copies and `ReadableBuffer` is a big struct.
4. Are parsers asynchronous or synchronous? In the model above, parsers are
always stateful and synchronous. The code calling into the parser does the
awaiting and calls the parser with more data when available. It means that in
the future, we'd recommend people write synchronous stateful parsers
(JSON.NET etc) and have asynchrony layered on top.
5. We need the primitives for reading and writing to be decoupled from
pipelines. It should be possible to write to arbitrary scratch buffers of
pooled/unpooled memory.
6. Overlapping writes are a harder problem because writing isn't a single atomic
call (Alloc(), Write(), Commit()). This isn't usually a *huge* problem because
pipelines are single producer single consumer.
