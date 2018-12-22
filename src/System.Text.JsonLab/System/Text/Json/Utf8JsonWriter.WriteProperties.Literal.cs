// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        public void WriteNull(string propertyName, bool suppressEscaping = false)
            => WriteNull(propertyName.AsSpan(), suppressEscaping);

        public void WriteNull(ReadOnlySpan<char> propertyName, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (!suppressEscaping)
                WriteNullSuppressFalse(ref propertyName);
            else
                WriteNullByOptions(ref propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        public void WriteNull(ReadOnlySpan<byte> propertyName, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (!suppressEscaping)
                WriteNullSuppressFalse(ref propertyName);
            else
                WriteNullByOptions(ref propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullSuppressFalse(ref ReadOnlySpan<char> propertyName)
        {
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            if (propertyIdx != -1)
            {
                WriteNullEscapeProperty(ref propertyName, propertyIdx);
            }
            else
            {
                WriteNullByOptions(ref propertyName);
            }
        }

        private void WriteNullSuppressFalse(ref ReadOnlySpan<byte> propertyName)
        {
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            if (propertyIdx != -1)
            {
                WriteNullEscapeProperty(ref propertyName, propertyIdx);
            }
            else
            {
                WriteNullByOptions(ref propertyName);
            }
        }

        private void WriteNullEscapeProperty(ref ReadOnlySpan<char> propertyName, int firstEscapeIndexProp)
        {
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            char[] propertyArray = null;

            int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
            Span<char> span;
            if (length > 256)
            {
                propertyArray = ArrayPool<char>.Shared.Rent(length);
                span = propertyArray;
            }
            else
            {
                // Cannot create a span directly since the span gets exposed outside this method.
                unsafe
                {
                    char* ptr = stackalloc char[length];
                    span = new Span<char>(ptr, length);
                }
            }
            JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int written);
            propertyName = span.Slice(0, written);

            WriteNullByOptions(ref propertyName);

            if (propertyArray != null)
                ArrayPool<char>.Shared.Return(propertyArray);
        }

        private void WriteNullEscapeProperty(ref ReadOnlySpan<byte> propertyName, int firstEscapeIndexProp)
        {
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            byte[] propertyArray = null;

            int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
            Span<byte> span;
            if (length > 256)
            {
                propertyArray = ArrayPool<byte>.Shared.Rent(length);
                span = propertyArray;
            }
            else
            {
                // Cannot create a span directly since the span gets exposed outside this method.
                unsafe
                {
                    byte* ptr = stackalloc byte[length];
                    span = new Span<byte>(ptr, length);
                }
            }
            JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int written);
            propertyName = span.Slice(0, written);

            WriteNullByOptions(ref propertyName);

            if (propertyArray != null)
                ArrayPool<byte>.Shared.Return(propertyArray);
        }

        private void WriteNullByOptions(ref ReadOnlySpan<char> propertyName)
        {
            int idx;

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameIndented(ref propertyName);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameMinimized(ref propertyName);
            }

            if (JsonConstants.NullValue.Length > _buffer.Length - idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullByOptions(ref ReadOnlySpan<byte> propertyName)
        {
            int idx;
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameIndented(ref propertyName);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameMinimized(ref propertyName);
            }

            if (JsonConstants.NullValue.Length > _buffer.Length - idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        public void WriteBoolean(string propertyName, bool value, bool suppressEscaping = false)
            => WriteBoolean(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (!suppressEscaping)
                WriteBooleanSuppressFalse(ref propertyName, value);
            else
                WriteBooleanByOptions(ref propertyName, value);

            _currentDepth |= 1 << 31;
        }

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (!suppressEscaping)
                WriteBooleanSuppressFalse(ref propertyName, value);
            else
                WriteBooleanByOptions(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteBooleanSuppressFalse(ref ReadOnlySpan<char> propertyName, bool value)
        {
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            if (propertyIdx != -1)
            {
                WriteBooleanEscapeProperty(ref propertyName, value, propertyIdx);
            }
            else
            {
                WriteBooleanByOptions(ref propertyName, value);
            }
        }

        private void WriteBooleanSuppressFalse(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            if (propertyIdx != -1)
            {
                WriteBooleanEscapeProperty(ref propertyName, value, propertyIdx);
            }
            else
            {
                WriteBooleanByOptions(ref propertyName, value);
            }
        }

        private void WriteBooleanEscapeProperty(ref ReadOnlySpan<char> propertyName, bool value, int firstEscapeIndexProp)
        {
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            char[] propertyArray = null;

            int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
            Span<char> span;
            if (length > 256)
            {
                propertyArray = ArrayPool<char>.Shared.Rent(length);
                span = propertyArray;
            }
            else
            {
                // Cannot create a span directly since the span gets exposed outside this method.
                unsafe
                {
                    char* ptr = stackalloc char[length];
                    span = new Span<char>(ptr, length);
                }
            }
            JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int written);
            propertyName = span.Slice(0, written);

            WriteBooleanByOptions(ref propertyName, value);

            if (propertyArray != null)
                ArrayPool<char>.Shared.Return(propertyArray);
        }

        private void WriteBooleanEscapeProperty(ref ReadOnlySpan<byte> propertyName, bool value, int firstEscapeIndexProp)
        {
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            byte[] propertyArray = null;

            int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
            Span<byte> span;
            if (length > 256)
            {
                propertyArray = ArrayPool<byte>.Shared.Rent(length);
                span = propertyArray;
            }
            else
            {
                // Cannot create a span directly since the span gets exposed outside this method.
                unsafe
                {
                    byte* ptr = stackalloc byte[length];
                    span = new Span<byte>(ptr, length);
                }
            }
            JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int written);
            propertyName = span.Slice(0, written);

            WriteBooleanByOptions(ref propertyName, value);

            if (propertyArray != null)
                ArrayPool<byte>.Shared.Return(propertyArray);
        }

        private void WriteBooleanByOptions(ref ReadOnlySpan<char> propertyName, bool value)
        {
            int idx;
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameIndented(ref propertyName);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameMinimized(ref propertyName);
            }

            if (value)
            {
                if (JsonConstants.TrueValue.Length > _buffer.Length - idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }

                JsonConstants.TrueValue.CopyTo(_buffer.Slice(idx));
                idx += JsonConstants.TrueValue.Length;
                _tokenType = JsonTokenType.True;
            }
            else
            {
                if (JsonConstants.FalseValue.Length > _buffer.Length - idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }

                JsonConstants.FalseValue.CopyTo(_buffer.Slice(idx));
                idx += JsonConstants.FalseValue.Length;
                _tokenType = JsonTokenType.False;
            }

            Advance(idx);
        }

        private void WriteBooleanByOptions(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            int idx;
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameIndented(ref propertyName);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                idx = WritePropertyNameMinimized(ref propertyName);
            }

            if (value)
            {
                if (JsonConstants.TrueValue.Length > _buffer.Length - idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }

                JsonConstants.TrueValue.CopyTo(_buffer.Slice(idx));
                idx += JsonConstants.TrueValue.Length;
                _tokenType = JsonTokenType.True;
            }
            else
            {
                if (JsonConstants.FalseValue.Length > _buffer.Length - idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }

                JsonConstants.FalseValue.CopyTo(_buffer.Slice(idx));
                idx += JsonConstants.FalseValue.Length;
                _tokenType = JsonTokenType.False;
            }

            Advance(idx);
        }
    }
}
