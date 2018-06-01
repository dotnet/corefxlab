// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref struct JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        private const int MaxDepth = sizeof(ulong) * 8;

        private readonly JsonEncoderState _encoderState;
        private readonly SymbolTable _symbolTable;

        private ReadOnlySpan<byte> _buffer;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        private int _depth;

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These next 2 properties are used to check for whether we can take the fast path
        // for invariant UTF-8 or UTF-16 processing. Otherwise, we need to go through the
        // slow path which makes use of the (possibly generic) encoder.
        private bool UseFastUtf8 => _encoderState == JsonEncoderState.UseFastUtf8;
        private bool UseFastUtf16 => _encoderState == JsonEncoderState.UseFastUtf16;

        // These properties are helpers for determining the current state of the reader
        private bool IsRoot => _depth == 1;
        private bool InArray => (_containerMask & 1) == 0 && (_depth > 0);
        private bool InObject => (_containerMask & 1) == 1;

        /// <summary>
        /// Gets the token type of the last processed token in the JSON stream.
        /// </summary>
        public JsonTokenType TokenType { get; private set; }

        /// <summary>
        /// Gets the value as a ReadOnlySpan<byte> of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />. Otherwise, this value should be set to
        /// <see cref="ReadOnlySpan{T}.Empty"/>.
        /// </summary>
        public ReadOnlySpan<byte> Value { get; private set; }

        /// <summary>
        /// Gets the JSON value type of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />.
        /// </summary>
        public JsonValueType ValueType { get; private set; }

        /// <summary>
        /// Gets the encoder instance used when the reader was constructed.
        /// </summary>
        public SymbolTable SymbolTable => _symbolTable;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="data">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="data"/> into characters.</param>
        public JsonReader(ReadOnlySpan<byte> data, SymbolTable symbolTable)
        {
            _buffer = data;
            _symbolTable = symbolTable;
            _depth = 0;
            _containerMask = 0;

            if (_symbolTable == SymbolTable.InvariantUtf8)
                _encoderState = JsonEncoderState.UseFastUtf8;
            else if (_symbolTable == SymbolTable.InvariantUtf16)
                _encoderState = JsonEncoderState.UseFastUtf16;
            else
                _encoderState = JsonEncoderState.UseFullEncoder;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            if (UseFastUtf8)
            {
                return ReadUtf8();
            }
            else if (UseFastUtf16)
            {
                return ReadUtf16();
            }
            else
            {
                JsonThrowHelper.ThrowNotImplementedException();
                return false;
            }
        }

        private bool ReadUtf8()
        {
            ref byte bytes = ref MemoryMarshal.GetReference(_buffer);
            int length = _buffer.Length;
            int skip = SkipWhiteSpaceUtf8(ref bytes, length);

            ref byte next = ref Unsafe.Add(ref bytes, skip);
            length -= skip;

            if (length < 1)
            {
                return false;
            }

            char ch = (char)next;
            int step = 1;

            switch (TokenType)
            {
                case JsonTokenType.None:
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                        step = ConsumePropertyNameUtf8(ref next, length);
                    break;

                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                        EndArray();
                    else
                        step = ConsumeValueUtf8(ch, step, ref next, length);
                    break;

                case JsonTokenType.PropertyName:
                    step = ConsumeValueUtf8(ch, step, ref next, length);
                    if (step == 0) return false;
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    step = ConsumeNextUtf8(ch, step, ref next, length);
                    if (step == 0) return false;
                    break;
            }

            _buffer = _buffer.Slice(skip + step);
            return true;
        }

        private bool ReadUtf16()
        {
            ref byte bytes = ref MemoryMarshal.GetReference(_buffer);
            int length = _buffer.Length;
            int skip = SkipWhiteSpaceUtf16(ref bytes, length);

            ref byte next = ref Unsafe.Add(ref bytes, skip);
            length -= skip;

            if (length < 2)
            {
                return false;
            }

            char ch = Unsafe.As<byte, char>(ref next);
            int step = 2;

            switch (TokenType)
            {
                case JsonTokenType.None:
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                        step = ConsumePropertyNameUtf16(ref next, length);
                    break;

                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                        EndArray();
                    else
                        step = ConsumeValueUtf16(ch, step, ref next, length);
                    break;

                case JsonTokenType.PropertyName:
                    step = ConsumeValueUtf16(ch, step, ref next, length);
                    if (step == 0) return false;
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    step = ConsumeNextUtf16(ch, step, ref next, length);
                    if (step == 0) return false;
                    break;
            }

            _buffer = _buffer.Slice(skip + step);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartObject()
        {
            if (_depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1) | 1;
            TokenType = JsonTokenType.StartObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndObject()
        {
            if (!InObject || _depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartArray()
        {
            if (_depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1);
            TokenType = JsonTokenType.StartArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndArray()
        {
            if (!InArray || _depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndArray;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private int ConsumeNextUtf16(char marker, int markerBytes, ref byte src, int length)
        {
            int skip = markerBytes;
            switch (marker)
            {
                case (char)JsonConstants.ListSeperator:
                    {
                        skip += SkipWhiteSpaceUtf16(ref Unsafe.Add(ref src, markerBytes), length - markerBytes);
                        length -= skip;
                        ref byte next = ref Unsafe.Add(ref src, skip);
                        if (InObject)
                            return skip + ConsumePropertyNameUtf16(ref next, length);
                        else if (InArray)
                        {
                            if (length < 2)
                            {
                                return 0;
                            }

                            char ch = Unsafe.As<byte, char>(ref next);
                            return skip + ConsumeValueUtf16(ch, 2, ref next, length);
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                            return default;
                        }
                    }

                case (char)JsonConstants.CloseBrace:
                    EndObject();
                    return skip;

                case (char)JsonConstants.CloseBracket:
                    EndArray();
                    return skip;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    return default;
            }
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private int ConsumeNextUtf8(char marker, int markerBytes, ref byte src, int length)
        {
            int skip = markerBytes;
            switch (marker)
            {
                case (char)JsonConstants.ListSeperator:
                    {
                        skip += SkipWhiteSpaceUtf8(ref Unsafe.Add(ref src, markerBytes), length - markerBytes);
                        length -= skip;
                        ref byte next = ref Unsafe.Add(ref src, skip);
                        if (InObject)
                            return skip + ConsumePropertyNameUtf8(ref next, length);
                        else if (InArray)
                        {
                            if (length < 1)
                            {
                                return 0;
                            }

                            char ch = (char)next;
                            return skip + ConsumeValueUtf8(ch, 1, ref next, length);
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                            return default;
                        }
                    }

                case (char)JsonConstants.CloseBrace:
                    EndObject();
                    return skip;

                case (char)JsonConstants.CloseBracket:
                    EndArray();
                    return skip;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    return default;
            }
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private int ConsumeValueUtf16(char marker, int markerBytes, ref byte src, int length)
        {
            TokenType = JsonTokenType.Value;

            switch (marker)
            {
                case (char)JsonConstants.Quote:
                    return ConsumeStringUtf16(ref src, length);

                case (char)JsonConstants.OpenBrace:
                    StartObject();
                    ValueType = JsonValueType.Object;
                    return markerBytes;

                case (char)JsonConstants.OpenBracket:
                    StartArray();
                    ValueType = JsonValueType.Array;
                    return markerBytes;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ConsumeNumberUtf16(ref src, length);

                case '-':
                    if (length < 2) JsonThrowHelper.ThrowJsonReaderException();
                    char ch = Unsafe.As<byte, char>(ref src);
                    return (ch == 'I')
                        ? ConsumeInfinityUtf16(ref src, length, true)
                        : ConsumeNumberUtf16(ref src, length);

                case 'f':
                    return ConsumeFalseUtf16(ref src, length);

                case 't':
                    return ConsumeTrueUtf16(ref src, length);

                case 'n':
                    return ConsumeNullUtf16(ref src, length);

                case 'u':
                    return ConsumeUndefinedUtf16(ref src, length);

                case 'N':
                    return ConsumeNaNUtf16(ref src, length);

                case 'I':
                    return ConsumeInfinityUtf16(ref src, length, false);

                case '/':
                    // TODO: Comments?
                    JsonThrowHelper.ThrowNotImplementedException();
                    return default;
            }

            return 0;
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private int ConsumeValueUtf8(char marker, int markerBytes, ref byte src, int length)
        {
            TokenType = JsonTokenType.Value;

            switch (marker)
            {
                case (char)JsonConstants.Quote:
                    return ConsumeStringUtf8(ref src, length);

                case (char)JsonConstants.OpenBrace:
                    StartObject();
                    ValueType = JsonValueType.Object;
                    return markerBytes;

                case (char)JsonConstants.OpenBracket:
                    StartArray();
                    ValueType = JsonValueType.Array;
                    return markerBytes;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ConsumeNumberUtf8(ref src, length);

                case '-':
                    if (length < 1) JsonThrowHelper.ThrowJsonReaderException();
                    char ch = (char)src;
                    return (ch == 'I')
                        ? ConsumeInfinityUtf8(ref src, length, true)
                        : ConsumeNumberUtf8(ref src, length);

                case 'f':
                    return ConsumeFalseUtf8(ref src, length);

                case 't':
                    return ConsumeTrueUtf8(ref src, length);

                case 'n':
                    return ConsumeNullUtf8(ref src, length);

                case 'u':
                    return ConsumeUndefinedUtf8(ref src, length);

                case 'N':
                    return ConsumeNaNUtf8(ref src, length);

                case 'I':
                    return ConsumeInfinityUtf8(ref src, length, false);

                case '/':
                    // TODO: Comments?
                    JsonThrowHelper.ThrowNotImplementedException();
                    return default;
            }

            return 0;
        }

        private int ConsumeNumberUtf8(ref byte src, int length)
        {
            int idx = 0;
            // Scan until we find a list separator, array end, or object end.
            while (idx < length)
            {
                ref byte b = ref Unsafe.Add(ref src, idx);
                // TODO: Fix terminating condition
                if (b == JsonConstants.ListSeperator || b == JsonConstants.CloseBrace || b == JsonConstants.CloseBracket || b == JsonConstants.CarriageReturn
                    || b == JsonConstants.LineFeed || b == JsonConstants.Space)
                    break;
                idx++;
            }

            // Calculate the real start of the number based on our current buffer location.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src);

            Value = _buffer.Slice(startIndex, idx);
            ValueType = JsonValueType.Number;
            return idx;
        }

        private int ConsumeNumberUtf16(ref byte src, int length)
        {
            ref char chars = ref Unsafe.As<byte, char>(ref src);
            int idx = 0;
            length >>= 1; // Each char is 2 bytes.

            // Scan until we find a list separator, array end, or object end.
            while (idx < length)
            {
                ref char b = ref Unsafe.Add(ref chars, idx);
                // TODO: Fix terminating condition
                if (b == JsonConstants.ListSeperator || b == JsonConstants.CloseBrace || b == JsonConstants.CloseBracket || b == JsonConstants.CarriageReturn
                    || b == JsonConstants.LineFeed || b == JsonConstants.Space)
                    break;
                idx++;
            }

            // Calculate the real start of the number based on our current buffer location.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src);

            // consumed is in characters, but our buffer is in bytes, so we need to double it for buffer slicing.
            int bytesConsumed = idx << 1;

            Value = _buffer.Slice(startIndex, bytesConsumed);
            ValueType = JsonValueType.Number;
            return bytesConsumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNaNUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            if (length < 3
                || Unsafe.Add(ref src, 0) != 'N'
                || Unsafe.Add(ref src, 1) != 'a'
                || Unsafe.Add(ref src, 2) != 'N')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNaNUtf16(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < 6
                || Unsafe.Add(ref chars, 0) != 'N'
                || Unsafe.Add(ref chars, 1) != 'a'
                || Unsafe.Add(ref chars, 2) != 'N')
            {
                throw new JsonReaderException();
            }

            return 6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNullUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Null;

            if (length < 4
                || Unsafe.Add(ref src, 0) != 'n'
                || Unsafe.Add(ref src, 1) != 'u'
                || Unsafe.Add(ref src, 2) != 'l'
                || Unsafe.Add(ref src, 3) != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNullUtf16(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Null;

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < 8
                || Unsafe.Add(ref chars, 0) != 'n'
                || Unsafe.Add(ref chars, 1) != 'u'
                || Unsafe.Add(ref chars, 2) != 'l'
                || Unsafe.Add(ref chars, 3) != 'l')
            {
                throw new JsonReaderException();
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeInfinityUtf8(ref byte src, int length, bool negative)
        {
            Value = ReadOnlySpan<byte>.Empty;

            ref byte idx = ref src;

            int answer = 8;
            if (negative)
            {
                answer = 9;
                ValueType = JsonValueType.NegativeInfinity;
                idx = Unsafe.Add(ref idx, 1);
            }
            else
            {
                ValueType = JsonValueType.Infinity;
            }

            if (length < answer
                || Unsafe.Add(ref idx, 0) != 'I'
                || Unsafe.Add(ref idx, 1) != 'n'
                || Unsafe.Add(ref idx, 2) != 'f'
                || Unsafe.Add(ref idx, 3) != 'i'
                || Unsafe.Add(ref idx, 4) != 'n'
                || Unsafe.Add(ref idx, 5) != 'i'
                || Unsafe.Add(ref idx, 6) != 't'
                || Unsafe.Add(ref idx, 7) != 'y')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            return answer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeInfinityUtf16(ref byte src, int length, bool negative)
        {
            Value = ReadOnlySpan<byte>.Empty;

            ref byte idx = ref src;

            int answer = 8;
            if (negative)
            {
                answer = 9;
                ValueType = JsonValueType.NegativeInfinity;
                idx = Unsafe.Add(ref idx, 2);
            }
            else
            {
                ValueType = JsonValueType.Infinity;
            }

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < answer * 2
                || Unsafe.Add(ref chars, 0) != 'I'
                || Unsafe.Add(ref chars, 1) != 'n'
                || Unsafe.Add(ref chars, 2) != 'f'
                || Unsafe.Add(ref chars, 3) != 'i'
                || Unsafe.Add(ref chars, 4) != 'n'
                || Unsafe.Add(ref chars, 5) != 'i'
                || Unsafe.Add(ref chars, 6) != 't'
                || Unsafe.Add(ref chars, 7) != 'y')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            return answer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeUndefinedUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            if (length < 9
                || Unsafe.Add(ref src, 0) != 'u'
                || Unsafe.Add(ref src, 1) != 'n'
                || Unsafe.Add(ref src, 2) != 'd'
                || Unsafe.Add(ref src, 3) != 'e'
                || Unsafe.Add(ref src, 4) != 'f'
                || Unsafe.Add(ref src, 5) != 'i'
                || Unsafe.Add(ref src, 6) != 'n'
                || Unsafe.Add(ref src, 7) != 'e'
                || Unsafe.Add(ref src, 8) != 'd')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeUndefinedUtf16(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < 18
                || Unsafe.Add(ref chars, 0) != 'u'
                || Unsafe.Add(ref chars, 1) != 'n'
                || Unsafe.Add(ref chars, 2) != 'd'
                || Unsafe.Add(ref chars, 3) != 'e'
                || Unsafe.Add(ref chars, 4) != 'f'
                || Unsafe.Add(ref chars, 5) != 'i'
                || Unsafe.Add(ref chars, 6) != 'n'
                || Unsafe.Add(ref chars, 7) != 'e'
                || Unsafe.Add(ref chars, 8) != 'd')
            {
                throw new JsonReaderException();
            }

            return 18;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeFalseUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.False;

            if (length < 5
                || Unsafe.Add(ref src, 0) != 'f'
                || Unsafe.Add(ref src, 1) != 'a'
                || Unsafe.Add(ref src, 2) != 'l'
                || Unsafe.Add(ref src, 3) != 's'
                || Unsafe.Add(ref src, 4) != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeFalseUtf16(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.False;

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < 10
                || Unsafe.Add(ref chars, 0) != 'f'
                || Unsafe.Add(ref chars, 1) != 'a'
                || Unsafe.Add(ref chars, 2) != 'l'
                || Unsafe.Add(ref chars, 3) != 's'
                || Unsafe.Add(ref chars, 4) != 'e')
            {
                throw new JsonReaderException();
            }

            return 10;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeTrueUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.True;

            if (length < 4
                || Unsafe.Add(ref src, 0) != 't'
                || Unsafe.Add(ref src, 1) != 'r'
                || Unsafe.Add(ref src, 2) != 'u'
                || Unsafe.Add(ref src, 3) != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeTrueUtf16(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.True;

            ref char chars = ref Unsafe.As<byte, char>(ref src);
            if (length < 8
                || Unsafe.Add(ref chars, 0) != 't'
                || Unsafe.Add(ref chars, 1) != 'r'
                || Unsafe.Add(ref chars, 2) != 'u'
                || Unsafe.Add(ref chars, 3) != 'e')
            {
                throw new JsonReaderException();
            }

            return 8;
        }

        private int ConsumePropertyNameUtf8(ref byte src, int length)
        {
            if (src != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
            int consumed = 1;
            while (consumed < length && Unsafe.Add(ref src, consumed++) != JsonConstants.Quote) ;

            if (consumed == length && Unsafe.Add(ref src, consumed - 1) != JsonConstants.Quote)
                JsonThrowHelper.ThrowJsonReaderException();

            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src) + 1;

            Value = _buffer.Slice(startIndex, consumed - 2); // -2 to exclude the quote characters.
            ValueType = JsonValueType.String;

            // SkipWhiteSpaceUtf8
            while (consumed < length)
            {
                byte val = Unsafe.Add(ref src, consumed);
                if (val == JsonConstants.Space || val == JsonConstants.CarriageReturn || val == JsonConstants.LineFeed || val == '\t')
                    consumed++;
                else
                    break;
            }
            if (consumed == length) JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (Unsafe.Add(ref src, consumed++) != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            return consumed;
        }

        private int ConsumePropertyNameUtf16(ref byte src, int length)
        {
            int consumed = ConsumeStringUtf16(ref src, length);
            if (consumed == 0) JsonThrowHelper.ThrowJsonReaderException();

            consumed += SkipWhiteSpaceUtf16(ref Unsafe.Add(ref src, consumed), length - consumed);
            if (consumed >= length) JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator
            if (Unsafe.As<byte, char>(ref Unsafe.Add(ref src, consumed)) != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            consumed += 2; // Skip the key / value seperator
            TokenType = JsonTokenType.PropertyName;
            return consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumePropertyNameSlow(ref byte src, int length)
        {
            JsonThrowHelper.ThrowNotImplementedException();
            return default;
        }

        private int ConsumeStringUtf8(ref byte src, int length)
        {
            // The first character MUST be a JSON string quote
            if (src != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();

            // If we are in this method, the first char is already known to be a JSON quote character.
            // Skip through the bytes until we find the closing JSON quote character.
            int idx = 1;
            while (idx < length && Unsafe.Add(ref src, idx++) != JsonConstants.Quote) ;

            // If we hit the end of the source and never saw an ending quote, then fail.
            if (idx == length && Unsafe.Add(ref src, idx - 1) != JsonConstants.Quote)
                JsonThrowHelper.ThrowJsonReaderException();

            // Calculate the real start of the property name based on our current buffer location.
            // Also, skip the opening JSON quote character.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src) + 1;

            Value = _buffer.Slice(startIndex, idx - 2); // -2 to exclude the quote characters.
            ValueType = JsonValueType.String;
            return idx;
        }

        private int ConsumeStringUtf16(ref byte src, int length)
        {
            ref char chars = ref Unsafe.As<byte, char>(ref src);
            length >>= 1; // sizeof(char) is 2, so we have half as many characters as count.

            // The first character MUST be a JSON string quote
            if (chars != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();

            // Skip through the bytes until we find the closing JSON quote character.
            int idx = 1;
            while (idx < length && Unsafe.Add(ref chars, idx++) != JsonConstants.Quote) ;

            // If we hit the end of the source and never saw an ending quote, then fail.
            if (idx == length && Unsafe.Add(ref chars, idx - 1) != JsonConstants.Quote)
                JsonThrowHelper.ThrowJsonReaderException();

            // Calculate the real start of the property name based on our current buffer location.
            // Also, skip the opening JSON quote character.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src) + 2;

            // idx is in characters, but our buffer is in bytes, so we need to double it for buffer slicing.
            // Also, remove the quote characters as well.
            length = (idx - 2) << 1;

            Value = _buffer.Slice(startIndex, length);
            ValueType = JsonValueType.String;
            return idx * sizeof(char);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeStringSlow(ref byte src, int length)
        {
            JsonThrowHelper.ThrowNotImplementedException();
            return default;
        }

        private int SkipWhiteSpaceUtf8(ref byte src, int length)
        {
            int idx = 0;
            while (idx < length)
            {
                byte val = Unsafe.Add(ref src, idx);
                if (val == JsonConstants.Space || val == JsonConstants.CarriageReturn || val == JsonConstants.LineFeed || val == '\t')
                    idx++;
                else
                    break;
            }

            return idx;
        }

        private int SkipWhiteSpaceUtf16(ref byte src, int length)
        {
            ref char chars = ref Unsafe.As<byte, char>(ref src);
            length >>= 1;

            int idx = 0;
            while (idx < length)
            {
                switch (Unsafe.Add(ref chars, idx))
                {
                    case (char)JsonConstants.Space:
                    case (char)JsonConstants.CarriageReturn:
                    case (char)JsonConstants.LineFeed:
                    case '\t':
                        idx++;
                        break;

                    default:
                        return idx * sizeof(char);
                }
            }

            return idx * sizeof(char);
        }

        private int SkipWhiteSpaceSlow(ref byte src, int length)
        {
            int idx = 0;
            while (idx < length)
            {
                if (length < 1)
                    break;
                char ch = (char)(Unsafe.Add(ref src, idx));
                switch (ch)
                {
                    case (char)JsonConstants.Space:
                    case (char)JsonConstants.CarriageReturn:
                    case (char)JsonConstants.LineFeed:
                    case '\t':
                        idx += 1;
                        break;

                    default:
                        return idx;
                }
            }

            return idx;
        }
    }
}
