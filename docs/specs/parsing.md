# Parsing

The current .NET Framework parsing APIs (e.g. int.TryParse) can parse text
represented by System.String (UTF16).
```c#
string text = ...
int value;
if(int.TryParse(text, out value)){
   ...
}
```
These APIs work great in many scenarios, e.g. parsing text contained in GUI
application's text boxes. They are not suitable for processing modern network
protocols, which are often text (e.g. JSON, HTTP headers), but encoded with
UTF8/ASCII, not UTF16, and contained in byte buffers, not strings. Because of
this, all modern web servers written for the .NET platform either don't use the
current BCL parsing APIs or take a performance hit to transcode from UTF8 to
UTF16, and to copy from buffers to strings.
```c#
byte[] json = ...
string text = Encoding.UTF8.GetString(json); // this is very expensive: copy,
                                             // transformation, object
                                             // allocations
int value;
if(int.TryParse(text, out value)){
   ...
}
```
To address modern networking scenarios better, we will provide parsing APIs
that:

a) Can parse byte buffers without the need to have a string to parse
b) Can parse text encoded as UTF8 (and possibly other encodings)
c) Can parse without any GC heap allocations

This document spells out the details of the project.

## API Design
We will provide low level APIs that can parse byte\* and ReadOnlySpan\<byte\>
buffers (moral equivalent of int.TryParse), and higher level APIs to parse
sequences of such buffers (similar to BinaryReader for encoded text streams).

To familiarize yourself with ReadOnlySpan\<T\> refer to:
https://github.com/dotnet/corefxlab/blob/master/docs/specs/span.md

### Low Level APIs

The low level APIs are policy free (not specific to scenarios, higher level
frameworks, etc.) and optimized for speed, and will look like the following
(per data type being parsed; the sample shows Int32 APIs):
```c#
public static class PrimitiveParser {
    // most general "overload"
    public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed, EncodingData encoding=null, TextFormat format=null);

    // UTF8-invariant overloads
    public static class InvariantUtf8 {

        // We did some preliminary measurements, and the shorter overload is
        // non-trivially faster. We should double check though.
        public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value);
        public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed);

        public unsafe static bool TryParseInt32(byte* text, int length, out int value);
        public unsafe static bool TryParseInt32(byte* text, int length, out int value, out int bytesConsumed);

        // overloads optimized for hexadecimal numbers
        public static class Hex {
            public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value);
            public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed);

            public unsafe static bool TryParseInt32(byte* text, int length, out int value);
            public unsafe static bool TryParseInt32(byte* text, int length, out int value, out int bytesConsumed);
        }
    }

    // UTF16-invariant overloads
    public static class InvariantUtf16 {
        // same as InvariantUtf8, but using char* and ReadOnlySpan<char>
        ...
    }
}
```
The most general (and the slowest) overload allows callers to specify [format](https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx) of
the text (`TextFormat`), and its encoding and culture (`EncodingData`).

`TextFormat` is an efficient (non allocating, preparsed) representation of
format strings. It's used in both parsing and formatting APIs.
```c#
    public struct TextFormat : IEquatable<TextFormat> {

        public static TextFormat Parse(char format);
        public static TextFormat Parse(ReadOnlySpan<char> format);
        public static TextFormat Parse(string format);

        public static implicit operator TextFormat (char symbol);

        public TextFormat(char symbol, byte precision=(byte)255);

        public byte Precision { get; }
        public char Symbol { get; set; }

        // less interesting members follow:    
        public bool HasPrecision { get; }
        public bool IsDefault { get; }
        public bool IsHexadecimal { get; }

        public const byte NoPrecision = (byte)255;
        public static TextFormat HexLowercase;
        public static TextFormat HexUppercase;

        public static bool operator ==(TextFormat left, TextFormat right);      
        public static bool operator !=(TextFormat left, TextFormat right);
        public override bool Equals(object obj);
        public bool Equals(TextFormat other);
        public override int GetHashCode();
        public override string ToString();
    }
```
`EncodingData` providing encoding and culture data used by parsing and
formatting routines:
```c#
    public struct EncodingData : IEquatable<EncodingData> {

        public EncodingData(byte[][] symbols, TextEncoding encoding);

        public static EncodingData InvariantUtf16 { get; }
        public static EncodingData InvariantUtf8 { get; }

        // parsers call this
        public bool TryParseSymbol(ReadOnlySpan<byte> buffer, out Symbol symbol, out int bytesConsumed);

        // formatters call this
        public bool TryEncode(Symbol symbol, Span<byte> buffer, out int bytesWritten);

        // less interesting members follow:  
        public bool IsInvariantUtf16 { get; }
        public bool IsInvariantUtf8 { get; }
        public TextEncoding TextEncoding { get; }
        public override bool Equals(object obj);
        public bool Equals(EncodingData other);
        public override int GetHashCode();
        public static bool operator ==(EncodingData left, EncodingData right);
        public static bool operator !=(EncodingData left, EncodingData right);

        public enum Symbol : ushort {
            D0 = (ushort)0,
            D1 = (ushort)1,
            D2 = (ushort)2,
            D3 = (ushort)3,
            D4 = (ushort)4,
            D5 = (ushort)5,
            D6 = (ushort)6,
            D7 = (ushort)7,
            D8 = (ushort)8,
            D9 = (ushort)9,
            DecimalSeparator = (ushort)10,
            Exponent = (ushort)16,
            GroupSeparator = (ushort)11,
            InfinitySign = (ushort)12,
            MinusSign = (ushort)13,
            NaN = (ushort)15,
            PlusSign = (ushort)14,
        }
    }
    public enum TextEncoding : byte {
        Ascii = (byte)2,
        Utf16 = (byte)0,
        Utf8 = (byte)1,
    }
```
These APIs can be used as follows.
```c#
ReadOnlySpan<byte> utf8Text = ...
if(PrimitiveParser.TryParseInt32(utf8Text, out var value, out var consumedBytes, EncodingData.InvariantUtf8, 'G'){
    Console.WriteLine(value);
    utf8Text = utf8Text.Slice(consumedBytes);
}

// or optimized version:
if(PrimitiveParser.InvariantUtf8.TryParseInt32(utf8Text, out var value, out var consumedBytes){
    Console.WriteLine(value);
    utf8Text = utf8Text.Slice(consumedBytes);
}
```

The current prototype implementation is [here](https://github.com/dotnet/corefxlab/tree/master/src/System.Text.Primitives/System/Text/Parsing).

## Performance Results

Preliminary performance results (using the slow span) are very encouraging:
```c#
public class TestParsing
{
    public static byte[] utf8 = Encoding.UTF8.GetBytes("123");
    public static Span<byte> s_utf8 = utf8;

    [Benchmark]
    public static uint DotNet461()
    {
        uint result;
        uint.TryParse("123", NumberStyles.None, CultureInfo.InvariantCulture, out result);
        return result;
    }

    [Benchmark(Baseline = true)]
    public static uint DotNet461_Utf8()
    {
        uint result;
        uint.TryParse(Encoding.UTF8.GetString(utf8), NumberStyles.None, CultureInfo.InvariantCulture, out result);
        return result;
    }

    [Benchmark]
    public static uint Corefxlabs()
    {
        uint result;
        PrimitiveParser.InvariantUtf8.TryParseUInt32(s_utf8, out result);
        return result;
    }
}
```

32-bit
``` ini

BenchmarkDotNet=v0.10.1, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-6700 CPU 3.40GHz, ProcessorCount=8
Frequency=3328121 Hz, Resolution=300.4698 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.6.1586.0


```
|         Method |       Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
|--------------- |----------- |---------- |------- |-------------- |------- |---------- |
|      DotNet461 | 53.0820 ns | 0.1801 ns |   0.65 |          0.00 |      - |       0 B |
| DotNet461_Utf8 | 81.3866 ns | 0.2131 ns |   1.00 |          0.00 | 0.0015 |      20 B |
|     Corefxlabs |  7.2585 ns | 0.1338 ns |   0.09 |          0.00 |      - |       0 B |

64-bit
``` ini

BenchmarkDotNet=v0.10.1, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-6700 CPU 3.40GHz, ProcessorCount=8
Frequency=3328121 Hz, Resolution=300.4698 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
|         Method |       Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
|--------------- |----------- |---------- |------- |-------------- |------- |---------- |
|      DotNet461 | 48.3371 ns | 0.2238 ns |   0.60 |          0.00 |      - |       0 B |
| DotNet461_Utf8 | 81.1080 ns | 0.4500 ns |   1.00 |          0.00 | 0.0043 |      32 B |
|     Corefxlabs | 17.9962 ns | 0.0451 ns |   0.22 |          0.00 |      - |       0 B |

And these should get much better once we start using the [fast span](span.md#designrepresentation).

### Higher Level APIs
The low level APIs can parse text residing in a single contiguous memory buffer.
We will need higher level APIs to make it easier to parse text out of "streams"
of data. The details of these APIs are not yet clear, but the following method
illustrates early thinking: https://github.com/dotnet/corefxlab/blob/master/src/System.Text.Formatting/System/Text/Parsing/SequenceParser.cs#L15

NOTE: We should explore changing the API from a stateless static method to a
TextReader like API that can traverse sequences of buffers while remembering its
current position.

## Requirements
### Priority 1
- Parse text contained in byte buffers
- Support types commonly used in network protocols: integers,
DateTimeOffset/DateTime (HTTP Format), URI, GUID, Boolean, BigInteger
- No GC heap allocations
- UTF8 and ISO-8859-1 (required by networking protocols)
- Invariant culture
- Decimal and hexadecimal
- Round trip text output by [formatting](formatting.md) APIs
- Support 'G', 'D', 'R', and 'X' formats
- Compatible with .NET Framework APIs, where possible
- Parse without knowing up-front how many bytes encode the value (i.e. see
bytesConsumed parameters)
- Performance comparable to pinvoking to hand written C++ parsers, and 2x faster
than the existing .NET APIs (e.g. int.TryParse)

### Priority 2
* Parse the following types with minimal allocations: `Single`, `Double`,
`Decimal`, `TimeSpan`
* Parses different cultures, e.g. English and German
* Parse from these additional encodings: ASCII, UTF16-BE, UTF16-LE

### Note on Encodings

While we generally need to handle parsing for more than just UTF8 we believe
it's beneficial to optimize for UTF8 (both in speed and in project schedule).

That's because UTF8 is the dominant encoding for cloud-based apps. We'd support
other encodings by either converting them to the UTF8 before or by having an
implementation that can parse different encodings through an abstraction.

Having said that, when we design the APIs, we need to ensure that they will
scale to arbitrary encodings. And the best way to validate that is to implement
(possibly not ship) other encodings.

## ISSUES/QUESTIONS:
1. How are we going to support ISO-8859-1? Is it just validation? Or we need
specific EncodingData?
2. We need to measure if it's worth having overloads that don't take
bytesConsumed
3. We need detailed design of the higher level APIs (parsing multi-spans)
4. How do people add support for buffer parsing to their types? Is there an
abstraction "IParseable" so that high level reader can read 3rd party types?

## Workitems
Workitems related to this design document are traced in issue
https://github.com/dotnet/corefxlab/issues/1070
