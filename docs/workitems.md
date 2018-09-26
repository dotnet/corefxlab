# Work Items

## Span

* [done] Remove Span<T> and ReadOnlySpan<T> from corefxlab
* [done] Find APIs that we added in `Span<T>` in CoreFxLab we haven't reviewed and/or
  scrutinized.
* [done] Create adaptive package for System.Memory (i.e. fast and slow span)
* [done] support bulk copy
* [done] copyto should work for overlapping ranges
* [done] review SpanExtensions and move some to System.Memory

## Parsing

* [done] performance tests and improvement
* [done] support fast lower case and upper case hexadecimal parsing
* [done] remove old APIs
* [done] remove `EncodingData(byte[][] symbols, TextEncoding encoding,
  Tuple<byte, int>[] parsingTrie)`
* (pri 1) add all primitive parser to PrimitiveParser (e.g. GUID, URI, date
  time, etc.)
* (pri 1) add support for multi-span parsing
* (pri 2) support ASCII
* (pri 2) clean up code
* (pri 2) full test coverage
* (pri 2) support all formats
* (pri 2) provide EncodingData.InvariantUtf16BE
* (pri 2) write custom EncodingData generation tool

## Formatting

* (pri 0) fast path formatting APIs for default and hex formats and for UTF8
* (pri 0) rearrange TryFormat parameters to match TryParse order
* (pri 0) performance tests and improvement
* (pri 2) support ASCII
* (pri 2) do API cleanup (e.g. rename formattingData to encodingData)
* (pri 2) clean up code
* (pri 2) full test coverage

## UTF8

* (pri 0) design proper encoding APIs
* (pri 0) performance tests and improvement
* [done] (pri 1) move encoding APIs from System.Text.Utf8 to System.Text.Primitives
* (pri 2) full test coverage

## Binary Reader/Writer

* (pri 0) performance tests and improvement
* (pri 1) design binary reader/writer

## JSON

* (pri 0) design reader/writer API operating on multi-spans
* (pri 0) performance tests and improvement
* (pri 1) cleanup DOM API
* (pri 1) design and implement UTF8 JSON serializer
* (pri 2) full test coverage

## HTTP Reader/Writer

* (pri 0) design reader/writer API operating on multi-spans
* (pri 0) performance tests and improvement
* (pri 2) full test coverage

## Memory

* [done] Finish design and prototype of `Memory<T>`
* [done] Unify buffer pooling abstractions

## Infrastructure

* [done] (pri 0) Enable performance test runs
* [done] Remove IL rewriting from corefxlab
* [done] Enable allocation tests and gates
* [done] Enable C#7

## Architecture/Design/Other

* [done] Remove `System.IO.Pipeline.Text.Primitives`
* [done] Prototype `TextReader`-like APIs
* Refactor `System.IO.Pipeline.File` into low level and pipleline-specific APIs
* Refactor `System.IO.Pipeline.Compression` into low level and pipeline-specific
 APIs
* Design proper Web.Encoding APIs
* [done] Drive language integration for spans
* Write programming guides/docs
* [done] Write solid set of benchmarks
* Explore consumedBytes to be passed by-ref
* Can we return `Span<T>` from `IOutput` (and other APIs)
* Make sure `EncodingData` supports currency and date time formatting
* [done] Decide what to do with Sequences
* [done] How does memory relate to MSR project
* UTF8 string representation
* UTF8 literals

## Other to think about

* Refactor networking channels
* [done] Productize Base64 encoding
* SLL
* [done] Non-allocating compression
* Built-in cultures
* UtcDateTime
* SSL

## Later

* XML
* OData
* HTML
