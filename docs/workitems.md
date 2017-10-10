# Work Items

## Span

* [done] Remove Span<T> and ReadOnlySpan<T> from corefxlab
* (pri 0) Find APIs that we added in `Span<T>` in CoreFxLab we haven't reviewed and/or
  scrutinized.
* (pri 0) Create adaptive package for System.Memory (i.e. fast and slow span)
* (pri 0) support bulk copy
* (pri 1) copyto should work for overlapping ranges
* (pri 1) review SpanExtensions and move some to System.Memory

## Parsing

* (pri 0) performance tests and improvement
* (pri 0) support fast lower case and upper case hexadecimal parsing
* (pri 1) remove old APIs
* (pri 1) remove `EncodingData(byte[][] symbols, TextEncoding encoding,
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

* (pri 0) Finish design and prototype of `Memory<T>`
* (pri 0) Unify buffer pooling abstractions

## Infrastructure

* [done] (pri 0) Enable performance test runs
* (pri 0) Remove IL rewriting from corefxlab
* (pri 1) Enable allocation tests and gates
* (pri 1) Enable C#7

## Architecture/Design/Other

* Remove `System.IO.Pipeline.Text.Primitives`
* Prototype `TextReader`-like APIs
* Refactor `System.IO.Pipeline.File` into low level and pipleline-specific APIs
* Refactor `System.IO.Pipeline.Compression` into low level and pipeline-specific
 APIs
* Design proper Web.Encoding APIs
* Drive language integration for spans
* Write programming guides/docs
* Write solid set of benchmarks
* Explore consumedBytes to be passed by-ref
* Can we return `Span<T>` from `IOutput` (and other APIs)
* Make sure `EncodingData` supports currency and date time formatting
* Decide what to do with Sequences
* How does memory relate to MSR project
* UTF8 string representation
* UTF8 literals

## Other to think about

* Refactor networking channels
* Productize Base64 encoding
* SLL
* Non-allocating compression
* Built-in cultures
* UtcDateTime
* SSL

## Later

* XML
* OData
* HTML
