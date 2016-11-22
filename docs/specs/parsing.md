# Parsing

The purpose of this document is to put some stakes in the ground for a set of
low-allocation parsing APIs.

The current prototype is [here](https://github.com/dotnet/corefxlab/tree/master/src/System.Text.Primitives/System/Text/Parsing).

## Goals

* Parses the following types with minimal allocations, ideally none:
    - `Boolean`
    - `Byte`, `SByte`
    - `Int16`, `UInt16`
    - `Int32`, `UInt32`
    - `Int64`, `UInt64`
    - `Single`, `Double`
    - `Decimal`
    - `DateTime`, `DateTimeOffset`
    - `TimeSpan`
    - `Guid`
    - `Uri`
* Parses different cultures
    - e.g. English and German dates
    - Needs to handle all cultures .NET Core can handle
* Parse any outputs produced by [formatting](formatting.md)
    - This ensures parsing & formatting behave symmetric
* Parse from these encodings
    - UTF8
    - ASCII (required for other standard, such as ISO-8859-1)
    - UTF16-BE
    - UTF16-LE
* Parses from existing buffers without knowing up-front how many bytes the
  parsing needs to consume

## Focus on UTF8

While we generally need to handle parsing for more than just UTF8 we belive it's
beneficial to have an implementation that is hand-optimized for UTF8 instead of
having an implementation that is going through some abstraction.

That's because UTF8 is the dominant encoding for cloud-based apps. We'd support
other encodings by either converting them to the UTF8 before or by having an
implementation that can parse different encodings through an abstraction.

## Low-Level API Shape

We generally want the APIs to be static methods and being able to operate on
raw data, which could either be native memory or a managed (byte) array.

Using the [Span\<T>.md] feature we can express this quite easily using
`ReadOnlySpan<T>` (`Xyz` indicates the name of the type being parsed):

```C#
public static bool TryParseXyz(ReadOnlySpan<byte> text,
                               out Xyz value,
                               out int bytesConsumed)
```

This allows the parsing to happen from a wide variety of data sources:

* **Native memory**, by creating a span from the pointer + lengh.
* **`byte[]`**, via an implicit conversion
* **`Span<byte>`**, via an implicit conversion

Depending on how we express encodings and cultures, it's likely what we need to
create additional arguments, defaulted to being UTF8 and invariant.

The result of the method indicates whether parsing was successful. Since the
entire point of these APIs is performance, we don't plan on adding APIs that
throw exceptions when parsing cannot succeed.

## High-Level API Shape

The problem with the low-level API shape is that it doesn't deal with situations
where the data spans multiple buffers. Thus, we need a way to for the parsing
methods to "pull" the next buffer. We currently have something like
`ISequence<T>`, which is basically a more efficient `IEnumerable<T>`:

```C#
public static bool TryParseXyz<T>(this T memorySequence,
                                  out Xyz value,
                                  out int bytesConsumed)
    where T : ISequence<ReadOnlyMemory<byte>>;
```

We'll need to decide if that's the right approach or not. Alternatively, we
could

* only support UTF8 and have a hand-optimized version of that and support other
  encodings by converting that representation to UTF8
* change the shape of the low-level API to allow the method to indicate whether
  more data could be consumed.
* don't offer these methods and leave it for a higher-layer, such as
  [pipelines](pipelines.md), to deal with that.

## Work Items

* (pri 0) performance tests and improvement
* (pri 0) support fast lower case and upper case hexadecimal parsing
* (pri 1) remove old APIs
* (pri 1) remove `EncodingData(byte[][] symbols, TextEncoding encoding, Tuple<byte, int>[] parsingTrie)`
* (pri 1) add all primitive parser to PrimitiveParser (e.g. GUID, URI, date time, etc.)
* (pri 1) add support for multi-span parsing
* (pri 2) support ASCII
* (pri 2) clean up code
* (pri 2) full test coverage
* (pri 2) support all formats
* (pri 2) provide EncodingData.InvariantUtf16BE
* (pri 2) write custom EncodingData generation tool
