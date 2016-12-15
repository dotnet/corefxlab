# Encoding

The existing .NET encoding APIs (System.Text.Encoding) don't work super well in non-allocating data pipelines:

1. They allocate output arrays (as opposed to taking output buffers are parameters).
2. They don't work with Span\<T\>
3. They have the overhead of virtual calls (which might be significan to transcoding small slices)

This document will describe these issues in detail and propose new APIs better suited for data pipelines.
