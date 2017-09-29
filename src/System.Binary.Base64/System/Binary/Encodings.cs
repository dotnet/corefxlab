using System;
using System.Collections.Generic;
using System.Text;

using utf8char = System.Byte; // until we get a proper utf8char type
using Utf8String = System.Object; // until we get a proper Utf8String type

namespace System.Whatever
{
    /*
     * CORE PIPELINING APIS
     */

    public interface IBinaryToStringEncoder
    {
        int GetMaxCharCount(int inputLengthInBytes);
        bool TryEncode(ReadOnlySpan<byte> input, bool isFinalChunk, Span<char> output, out int numBytesConsumed, out int numCharsWritten);
        bool TryGetCharCount(ReadOnlySpan<byte> input, bool isFinalChunk, Span<char> output, out int numBytesConsumed, out int numCharsOutput);
    }

    public interface IBinaryToUtf8StringEncoder
    {
        int GetMaxCharCount(int inputLengthInBytes);
        bool TryEncode(ReadOnlySpan<byte> input, bool isFinalChunk, Span<utf8char> output, out int numBytesConsumed, out int numCharsWritten);
        bool TryGetCharCount(ReadOnlySpan<byte> input, bool isFinalChunk, Span<utf8char> output, out int numBytesConsumed, out int numCharsOutput);
    }

    public interface IStringToBinaryDecoder
    {
        int GetMaxByteCount(int inputLengthInChars);
        bool TryDecode(ReadOnlySpan<char> input, bool isFinalChunk, Span<byte> output, out int numCharsConsumed, out int numBytesWritten);
        bool TryGetByteCount(ReadOnlySpan<char> input, bool isFinalChunk, Span<byte> output, out int numCharsConsumed, out int numBytesOutput);
    }

    public interface IUtf8StringToBinaryDecoder
    {
        int GetMaxByteCount(int inputLengthInChars);
        bool TryDecode(ReadOnlySpan<utf8char> input, bool isFinalChunk, Span<byte> output, out int numCharsConsumed, out int numBytesWritten);
        bool TryGetByteCount(ReadOnlySpan<utf8char> input, bool isFinalChunk, Span<byte> output, out int numCharsConsumed, out int numBytesOutput);
    }

    /*
     * AND A GAZILLION EXTENSION METHODS
     */

    public static class EncodingExtensions
    {
        public static byte[] Decode(this IStringToBinaryDecoder encoder, ReadOnlySpan<char> input);
        public static byte[] Decode(this IUtf8StringToBinaryDecoder encoder, ReadOnlySpan<utf8char> input);
        public static void DecodeToBuffer(this IStringToBinaryDecoder encoder, ReadOnlySpan<char> input, Span<byte> output, out int numBytesWritten);
        public static void DecodeToBuffer(this IUtf8StringToBinaryDecoder encoder, ReadOnlySpan<utf8char> input, Span<byte> output, out int numBytesWritten);
        public static string Encode(this IBinaryToStringEncoder encoder, ReadOnlySpan<byte> input);
        public static Utf8String EncodeAsUtf8String(this IBinaryToUtf8StringEncoder encoder, ReadOnlySpan<byte> input);
        public static void EncodeToBuffer(this IBinaryToStringEncoder encoder, ReadOnlySpan<byte> input, Span<char> output, out int numCharsWritten);
        public static void EncodeToBuffer(this IBinaryToUtf8StringEncoder encoder, ReadOnlySpan<byte> input, Span<utf8char> output, out int numCharsWritten);
        public static int GetDecodedByteCount(this IStringToBinaryDecoder decoder, ReadOnlySpan<char> input);
        public static int GetDecodedByteCount(this IUtf8StringToBinaryDecoder decoder, ReadOnlySpan<utf8char> input);
        public static int GetEncodedCharCount(this IBinaryToStringEncoder encoder, ReadOnlySpan<byte> input);
        public static int GetEncodedUtf8CharCount(this IBinaryToUtf8StringEncoder encoder, ReadOnlySpan<byte> input);
    }

    static class Encodings
    {
        public static Base64Encoding Base64 { get; }
        public static Base64UrlEncoding Base64Url { get; }
        public static HexEncoding Hex { get; } // defaults to uppercase
    }

    public class Base64Encoding : IBinaryToStringEncoder, IBinaryToUtf8StringEncoder, IStringToBinaryDecoder, IUtf8StringToBinaryDecoder
    {
    }

    public class Base64UrlEncoding : IBinaryToStringEncoder, IBinaryToUtf8StringEncoder, IStringToBinaryDecoder, IUtf8StringToBinaryDecoder
    {
    }

    public class HexEncoding : IBinaryToStringEncoder, IBinaryToUtf8StringEncoder, IStringToBinaryDecoder, IUtf8StringToBinaryDecoder
    {
        public HexEncoding(bool useLowercase)
        {
            // if useLowercase, then generates [0-9a-f], otherwise generates [0-9A-F]
            // always decodes mixed case regardless
        }

        // This class can also contain useful public static methods.

        // ['0' - '9'] => [0 - 9]
        // ['A' - 'F'] => [10 - 15]
        // ['a' - 'f'] => [10 - 15]
        // all else => failure
        public static int ParseHexCharacter(char hexChar);
        public static bool TryParseHexCharacter(char hexChar, out int value);
    }

    /*
     * SAMPLE USAGE
     */

    public static class Samples
    {
        public static void Main()
        {
            byte[] binaryData = { 1, 2, 3, 4, 5 };

            string asBase64 = Encodings.Base64.Encode(binaryData);
            string asHex = Encodings.Hex.Encode(binaryData);

            byte[] decoded = Encodings.Hex.Decode(asHex);
        }
    }
}
