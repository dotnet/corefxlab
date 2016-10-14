#Span\<T\>

## Introduction

Span\<T\> is a new type we are adding to the platform to represent contiguous regions of arbitrary memory, with perfromance characteristics on pair with T[]. Its APIs are similar to the array, but unlike arrays, it can point to either managed or native memory, or to memory allocated on the stack. 

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
    public ref T [int index] { get; }

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

When we analyze the gaps we have today and the requirements of today’s high sclae servers, we realize that we need to provide modern no-copy, low-allocation, and UTF8 data transformation APIs that are efficient, reliable, and easy to use. Prototypes of such APIs are available in [Corfxlab repo]( https://github.com/dotnet/corefxlab), and Span\<T\> is one of the main fundamental building blocks for these APIs.

###Data Pipelines
Modern servers are often designed as, often reactive, pipelines of components doing transformations on byte buffers. For example, such pipeline in a web server might consist of the following transformations: socket fills in a buffer -> HTTP parsing -> decompression -> Base 64 decoding -> routing -> HTML writing -> HTML escaping -> HTTP writing -> compression -> socket writing. 

Span\<byte\> is very useful for implementing transformation routines of such data pipelines. First, Span\<T\> allows the server to freely switch between managed and native buffers depending on situation/settings. For example, Windows RIO sockets work best with native buffers, and libuv Kestrel works best with pinned managed arrays. Secondly, it allows complicated transtormation algorights to be implementd in safe code without the need to resort to using raw pointers. Lastly, fact that Span\<T\> is slicable, allows the piepline to abstract the phisical chunks of buffers to logical chunks relevant to particular section of the pipeline.

The stack-only nature of spans (see more on this below), allows pooled memory to be safely returned to the pool after the transformations pipeline complete, and allows the pipeline to pass only the relevant slice of the buffer to each transformation routine/component. In other words, Span\<T\> aids in lifetime management of pooled buffers, so critical to perfromance of today's servers.
