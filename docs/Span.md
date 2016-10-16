#Span\<T\>

## Introduction

Span\<T\> is a new type we are adding to the platform to represent contiguous regions of arbitrary memory, with performance characteristics on par with T[]. Its APIs are similar to the array, but unlike arrays, it can point to either managed or native memory, or to memory allocated on the stack. 

```c#
// managed memory
var arrayMemory = new byte[100];
var arraySpan = new Span<byte>(arrayMemory);

// native memory
var nativeMemory = Marshal.AllocHGlobal(100);
Span<byte> nativeSpan;
unsafe {
    nativeSpan = new Span<byte>(nativeMemory.ToPointer(), 100);
}
SafeSum(nativeSpan);
Marshal.FreeHGlobal(nativeMemory);

// stack memory
Span<byte> stackSpan;
Unsafe {
    byte* stackMemory = stackalloc byte[100];
    stackSpan = new Span<byte>(stackMemory, 100);
}
SafeSum(stackSpan);
```

As such, Span\<T\> is an abstraction over all types of memory available to .NET programs.

```c#
// this method does not care what kind of memory it works on
static ulong SafeSum(Span<byte> bytes) { 
    ulong sum = 0;
    for(int i=0; i < bytes.Length; i++) {
        sum += bytes[i];
    }
    return sum;
}
```
When wrapping an array, Span\<T\> is not limited to pointing to the first element of the array. It can point to any sub-range. In other words, it supports slicing.
```c#
var array = new byte[] { 1, 2, 3 };
var slice = new Span<byte>(array, start:1, length:2); 
Console.WriteLine(slice[0]); // prints 2
```
##API Surface
The full API surface of Span\<T\> is not yet finalized, but the main APIs we will expose are the following:   

```c#
public struct Span<T> {
    public Span(T[] array)
    public Span(T[] array, int index)
    public Span(T[] array, int index, int length)
    public unsafe Span(void* memory, int length)

    public static implicit operator Span<T> (ArraySegment<T> arraySegment);
    public static implicit operator Span<T> (T[] array);

    public int Length { get; }
    public ref T this[int index] { get; }

    public Span<T> Slice(int index);
    public Span<T> Slice(int index, int length);
    public bool TryCopyTo(T[] destination);
    public bool TryCopyTo(Span<T> destination);

    public T[] ToArray();
}
```
In addition, we will provide a read-only version of Span\<T\>. The ReadOnlySpan\<T\> is required to represent slices of immutable and read-only structures, e.g. System.String slices. This is discussed below.

## Scenarios
Span\<T\> is a small, but critical, building block for a much larger effort to provide .NET APIs to enable development of high scalability server applications.

The .NET Framework design philosophy has focused almost solely on productivity for developers writing application software. In addition, many of the Framework’s design decisions were made assuming Windows client-server applications circa 1999. This design philosophy is big part of .NET’s success as .NET is universally viewed as a very high productivity platform.

But the landscape has shifted since our platform was conceived almost 20 years ago. We now target non-Windows operating systems, our developers write cloud hosted services demanding different tradeoffs than client-server applications, the state of art patterns have moved away from once popular technologies like XML, UTF16, SOAP (to name a few), and the hardware running today’s software is very different than what was available 20 years ago. 

When we analyze the gaps we have today and the requirements of today’s high scale servers, we realize that we need to provide modern no-copy, low-allocation, and UTF8 data transformation APIs that are efficient, reliable, and easy to use. Prototypes of such APIs are available in [corefxlab repository]( https://github.com/dotnet/corefxlab), and Span\<T\> is one of the main fundamental building blocks for these APIs.

###Data Pipelines
Modern servers are often designed as, often reactive, pipelines of components doing transformations on byte buffers. For example, such pipeline in a web server might consist of the following transformations: socket fills in a buffer -> HTTP parsing -> decompression -> Base 64 decoding -> routing -> HTML writing -> HTML escaping -> HTTP writing -> compression -> socket writing. 

Span\<byte\> is very useful for implementing transformation routines of such data pipelines. First, Span\<T\> allows the server to freely switch between managed and native buffers depending on situation/settings. For example, Windows RIO sockets work best with native buffers, and libuv Kestrel works best with pinned managed arrays. Secondly, it allows complicated transtormation algorights to be implementd in safe code without the need to resort to using raw pointers. Lastly, fact that Span\<T\> is slicable, allows the piepline to abstract the phisical chunks of buffers to logical chunks relevant to particular section of the pipeline.

The stack-only nature of spans (see more on this below), allows pooled memory to be safely returned to the pool after the transformations pipeline complete, and allows the pipeline to pass only the relevant slice of the buffer to each transformation routine/component. In other words, Span\<T\> aids in lifetime management of pooled buffers, so critical to perfromance of today's servers.

####Discontinuous Buffers
As alluded to before, data pipelines often process data in chunks as they arrives at a socket. This creates problems for data transformation routines, e.g. parsing, which have to deal with processing data that can reside in two or more buffers. For example, there might be a need to parse an integer residing partially in one buffer and partially in another. Since spans can abstract stack memory, they can solve this problem in a very elegant and performant way as illustrated in the following routine from ASP.NET Channels pipeline ([full source](https://github.com/davidfowl/Channels/blob/master/src/Channels.Text.Primitives/ReadableBufferExtensions.cs#L81)):
```c#
public unsafe static uint GetUInt32(this ReadableBuffer buffer) {
    ReadOnlySpan<byte> textSpan;

    if (buffer.IsSingleSpan) { // if data in single buffer, it’s easy
        textSpan = buffer.First.Span;
    }
    else if (buffer.Length < 128) { // else, consider temp buffer on stack
        var data = stackalloc byte[128];
        var destination = new Span<byte>(data, 128);
        buffer.CopyTo(destination);
        textSpan = destination.Slice(0, buffer.Length);
    }
    else {
        // else pay the cost of allocating an array
        textSpan = new ReadOnlySpan<byte>(buffer.ToArray());
    }

    uint value;
    var utf8Buffer = new Utf8String(textSpan);
    // yet the actual parsing routine is always the same and simple
    if (!PrimitiveParser.TryParseUInt32(utf8Buffer, out value)) {
        throw new InvalidOperationException();
    }
    return value;
} 
```

###Non-Allocating Substring
Modern server protocols are more often than not text based, and so it's not surprising that such servers often create and manipulate lots of strings. 

One of the most common basic string operations is string slicing. Currently, System.String.Substring is the main .NET API for creating slices of a string, but the API is inefficient as it creates a new string to represent the slice and copies the characters from the original string to the new string slice. Because of this inefficiency, high performance servers shy away from using this API, where they can (in their internals), and pay the cost in the publicly facing APIs.

ReadOnlySpan\<char\> could be a much more efficient standard representation of a subsection of a string:
```c#
public struct ReadOnlySpan<T> {
    public ReadOnlySpan(T[] array)
    public ReadOnlySpan(T[] array, int index)
    public ReadOnlySpan(T[] array, int index, int length)
    public unsafe ReadOnlySpan(void* memory, int length)

    public int Length { get; }
    public T this[int index] { get; }

    public ReadOnlySpan <T> Slice(int index)
    public ReadOnlySpan <T> Slice(int index, int count)

    public bool TryCopyTo(T[] destination);
    public bool TryCopyTo(Span<T> destination);
}

ReadOnlySpan<char> lengthText = "content-length:123".Slice(15);
```
###Parsing
TODO

###Formatting
TODO

###Buffer Pooling
Span\<T\> can be used to pool memory from a large single buffer allocated on the native heap. This decreases [pointless] work the GC needs to perform to manage pooled buffers, which never get collected anyway, but often need to be permanently pinned, which is bad for the system. Also, the fact that native memory does not move, lowers the cost of interop and the cost of pool related error checking (e.g. checking if a buffer is already returned to the pool).

Separatelly, the stack-only nature of Span\<T\> makes lifetime management of pooled memry more relaible; it helps in avoiding use-after-free errors with pooled memory. Without Span\<T\>, it’s often not clear when a pooled buffer that was passed to a separate module can be returned to the pool, as the module could be holding to the buffer for later use. With Span\<T\>, the server pipeline can be sure that there are no more references to the buffer after the stack pops to the frame that first allocated the span and passed it down to other modules.

###Native code interop
Today, unmanaged buffers passed over unmanaged to managed boundary are frequently copied to byte[] to allow safe access from managed code. Span\<T\> can eliminate the need to copy on many such scenarios.

Secondly, a number of performance critical APIs in the Framework take unsafe pointers as input. Examples include Encoding.GetChars or Buffer.MemoryCopy. Over time, we should provide more safe APIs that use Span<T>, which will allow more code to compile as safe but still preserve its performance characteristics. 

#Requirements
To support the scenarios described above, Span\<T\> must meet the following requirements:

1.	Ability to wrap managed and native memory
2.	Performance characteristics on par with arrays
3.	Be memory safe

#Design/Representation
We will provide two different implementations of Span\<T\>: 
- Fast Span (avaliable on runtimes with special support for spans)
- Slow Span (avaliable on all current .NET runtimes, even existing ones, e.g. .NET 4.5) 

The fast implementation, will rely on "ref field" support and will look as follows:

```c#
public struct Span<T> {
    internal ref T _pointer;
    internal int _length;
}
```

A prototype of such fast Span\<T\> can be found at https://github.com/dotnet/coreclr/blob/SpanOfT/src/mscorlib/src/System/Span.cs. Through the magic of the ref field, it can support slicing without requiring a strong pointer to the root of the sliced object. The GC is able to trace the interior pointer, keep the root object alive, and update the interior pointer if the object is relocated during a collection.

A different representation will be implemented for platforms that don’t support ref fields (interior pointers):
```c#
public struct Span<T> {
    internal IntPtr _pointer;
    internal object _relocatableObject;
    internal int _length;
}
```
A prototype of this design can be found at https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/Span.cs.
In this representation, the Span\<T\>'s indexer will add the _pointer and the address of _relocatableObject before accessing items in the Span. This will make the accessor slower, but it will ensure that when the GC moves the sliced object (e.g. array) in memory, the indexer still accesses the right memory location. Note that if the Span wraps a managed object, the _pointer field will be the offset off the object's root to the objects data slice, but if the Span wraps a native memory, the _pointer will point to the memory and the _relocatableObject will be set to null (zero). In either case, adding the pointer and the address of the object (null == 0) results in the right "effective" address.

##Struct Tearing
Struct tearing is a threading issue that affects all structs larger than what can be atomically updated on the target processor architecture. For example, some 64-bit processors can only update one 64-bit aligned memory block atomically. This means that some processors won’t be able to update both the _pointer and the _length fields of the Span atomically. This in turn means that the following code, might result in another thread observing _pointer and _length fields belonging to two different spans (the original one and the one being assigned to the field):
```c#
interanl class Bufer {
    Span<byte> _memory = new byte[1024];

    public void Resize(int newSize) {
        _memory = new byte[newSize]; // this will not update atomically
    }

    public byte this[int index] => _memory[index]; // this might see partial update
}
```
For most structs, tearing is at most a correctness bug and can be dealt with by making the fields (typed as the tearable struct type) non-public and synchronizing access to them. But since Span needs to be as fast as the array, access to the field cannot be synchronized. Also, because of the fact that Span accesses (and writes to) memory directly, having the _pointer and the _length be out of sync could result in memory safety being compromised. 

The only other way (besides synchronizing access, which would be not practical) to avoid this issue is to make Span a stack-only type, i.e. its instances can reside only on the stack (which is accessed by one thread). 

##Span will be stack only
Span\<T\> will be a stack-only type; more precisely, it will be a by-ref type (just like its field in the fast implementation). This means that Spans cannot be boxed, cannot appear as a field of a non-stack-only type, and cannot be used as a generic argument. However, Span\<T\> can be used as a type of method arguments or return values. 

We chose to make span stack-only as it solves several problems:
- Efficient representation and access: Span\<T\> can be just managed pointer and length.
- Efficient GC tracking: limit number of interior pointers that the GC have to track. Tracking of interior pointers in the heap during GC would be pretty expensive.
- Safe concurrency (struct tearing discussed above): Span<T> assignment does not have to be atomic. Atomic assignment would be required for storing Span\<T\> on the heap to avoid data tearing issues.
- Safe lifetime: Safe code cannot create dangling pointers by storing it on the heap when Span\<T\> points to unmanaged memory or stack memory. The unsafe stack frame responsible for creating unsafe Span is responsible for ensuring that it won’t escape the scope.
- Reliable buffer pooling: buffers can be rented from a pool, wrapped in spans, the spans passed to user code, and when the stack unwinds, the program can reliably return the buffer to the pool as it can be sure that there are no outstanding references to the buffer.

The fast representation makes the type automatically stack-only, i.e. the constraint will be enforced by CLR type loader. This restriction should also be enforced by managed language compilers and/or analyzers for better developer experience. For the slow span, language compiler checks and/or analyzers is the only option (as the runtimes won't enforce the stack-only restriction).

##Memory\<T\>
As alluded to above, in the upcoming months, many data transformation components in .NET (e.g. Base64Encoding, compressions, formatting, parsing) will provide APIs operating on memory buffers. We will do this work to develop no-copy/low-allocation end-2-end data pipelines, like the ASP.NET Channels. These APIs will use a collection of types, including, but not limited to Span\<T\> to represent various data pipeline primitives and exchange types.

This new collection of types must be usable by two distinct sets of customers: 
- Productivity developers (99% case): these are the developers who use LINQ, async, lambdas, etc., and often for good reasons care more about productivity than squeezing the last cycles out of some low level transformation routines.  
- Low level developers (1% case): our library and framework authors for whom perf is a critical aspect of their work. 

Even though the goals are each group are different, they rely on each other to be successful. One is a necessary consumer of the other. 

A stack only type with the associated tradeoffs is great for low level developers writing data transformation routines. Productivity developers, writing apps, may not be so thrilled when they realize that when using stack-only types, they lose many of the language features they rely on to get their jobs done (e.g. async await). And so, a stack only type simply can’t be the primary exchange type we recommend for high level developers/scenarios/APIs.

For the whole platform to be successful, we must add an exchange type, currently called Memory\<T\>, that can be used with the full power of the language, i.e. it’s not stack-only. Memory\<T\> can be seen as a “promise” of a Span. It can be freely used in generics, stored on the heap, used with async await, and all the other language features we all love. When Memory\<T\> is finally ready to be manipulated by a data transformation routine, it will be temporarly converted to a span (the promise will be realized), which will provide much more efficient (remember "on par with array") access to the buffer's data.

See a prototype of Memory\<T\> at https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/Buffers/Memory.cs. Note that the prototype is curently not tearing safe. We will make it safe in the upcomming weeks.

#Other Random Thoughts

##Optimizations
We need to enable the existing array bounds check optimizations for Span – in both the static compiler and the JIT – to make its perfromance on par with arrays. Longer term, we should optimize struct passing and construction to make slicing operations on Spans more efficient. Today, we recommend that Spans are sliced only when a shorted span needs to be passed to a different routine. Within a single routine, code should do index arithmetic to access subranges of spans. 

##Conversions
Span\<T\> will support reinterpret cast conversions to Span\<byte\>. It will also support unsafe casts between arbitrary primitive types. The reason for this limitation is that some processors don’t support efficient unaligned memory access.

##Platform Support Plans
We want to ship Span\<T\> as a nugget fat package available for .NET Standard 1.1 and above. Runtimes that support by-ref fields and returns will get the fast two filed span. Other runtimes will get the slower three-field span. 

##Relationship to Array Slicing
Since Span\<T\> will be a stack only-type, it’s not well suited as the general representation of array slice. When an array is sliced, majority of our users expect the result to be either an array, or at least a type that is very similar to the array (e.g. ArraySegment\<T\>). We will design array slicing separately from Span\<T\>.

##Covariance
Unlike T[], Span<T> will not support covariant casts, i.e. cast Span\<Subtype\> to Span\<Basetype\>. Because of that, we won’t be doing covariance checks when storing references in spans.

