// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Formatting;

namespace System.Text.Utf8
{
    public struct Utf8String : IBufferFormattable, IEquatable<Utf8String>
    {
        ReadOnlySpan<byte> _bytes;

        public Utf8String(params byte[] utf8Bytes)
        {
            _bytes = utf8Bytes;
        }
        public Utf8String(ReadOnlySpan<byte> utf8Bytes)
        {
            _bytes = utf8Bytes;
        }

        public Utf8String(string str)
        {
            _bytes = Encoding.UTF8.GetBytes(str);
        }

        public static Utf8String Empty
        {
            get { return new Utf8String(); }
        }

        public int Length
        {
            get { return _bytes.Length; }
        }

        public ReadOnlySpan<byte> Bytes
        {
            get
            {
                return _bytes;
            }
        }

        public bool Equals(ReadOnlySpan<byte> utf8Bytes)
        {
            if (_bytes.Length != utf8Bytes.Length) return false;
            for (int index = 0; index < _bytes.Length; index++)
            {
                if (_bytes[index] != utf8Bytes[index]) return false;
            }
            return true;
        }

        public bool Equals(Utf8String other)
        {
            return this.Equals(other._bytes);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int written)
        {
            if (buffer.Length < _bytes.Length)
            {
                written = 0;
                return false;
            }

            // TODO: this needs to check if formattingData is utf8 

            buffer.Set(_bytes);
            written = _bytes.Length;
            return true;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

