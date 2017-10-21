# Encoding

The existing .NET encoding APIs (System.Text.Encoding) don't work super well in
non-allocating data pipelines:

1. They allocate output arrays (as opposed to taking output buffers are
  parameters).
2. They don't work with Span\<T\>
3. They have the overhead of virtual calls (which might be significant to
  transcoding small slices)

The requirements for the new APIs are as follows:

1. Transcode bytes contained in ReadOnlySpan<byte> into a passed in Span<byte>
2. The API needs to handle running out of space in the output buffer and then
continuing when an additional output buffer is passed in.
3. The API needs to be fast (on par with native code implementations)
4. Needs to do this with zero GC allocations
5. Needs to be stateless (multithreaded)
6. We need to support transcoding between UTF8, UTF16LE, ISO8859-1, and be able
 to support other encodings in the future.

This document will describe these issues in detail and propose new APIs better
suited for data pipelines.
