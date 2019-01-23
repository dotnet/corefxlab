// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#define STACKBUFFER
#define TRACKRENT

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    // This file is a copy from CoreFX/CoreCLR (made public) with simple tweaks.
    // As much as possible code has been left the same to allow for easier comparison.

    public unsafe ref partial struct ValueStringBuilder
    {
        private char[] _arrayToReturnToPool;
        private Span<char> _chars;
        private int _pos;

        // Stringbuilder has a default size of 16. This is an experiment to
        // see the impact of reserving a small amount of stack space.
        //
        // It does make a noticeable difference over renting the smallest buffer
        // (16 chars), but copying is *very* costly so the max buffer size tracking may
        // be more impactful. Still gathering data.

#if STACKBUFFER
        private fixed char _default[DefaultBufferSize];
        private const int DefaultBufferSize = 16;
#endif

#if TRACKRENT
        // The initial rent we'll do- we'll grow and contract this based
        // on our rental history when we dispose of the builder.
        [ThreadStatic]
        private static int t_InitialMinRent = 0;

        // The largest initial rent we'll do based on history.
        private const int MaxInitialRent = 1024;

        // The rent size decrement if we don't use the entire buffer. ArrayPool bucketing
        // (memory is given out in ^2 chunks) should allow us to gradually "age" down into
        // a smaller bucket size should we repeatedly not use the full bucket.
        private const int InitialRentDecrement = 64;
#endif

        public ValueStringBuilder(Span<char> initialBuffer)
        {
            _arrayToReturnToPool = null;
            _chars = initialBuffer;
            _pos = 0;
        }

        public ValueStringBuilder(int initialCapacity)
        {
            _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _arrayToReturnToPool;
            _pos = 0;
        }

        public int Length
        {
            get => _pos;
            set
            {
                Debug.Assert(value >= 0);
                Debug.Assert(value <= _chars.Length);
                _pos = value;
            }
        }

        public int Capacity => _chars.Length;

        public void EnsureCapacity(int capacity)
        {
            if (capacity > _chars.Length)
                Grow(capacity - _chars.Length);
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
        public ref char GetPinnableReference(bool terminate = false)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }
            return ref MemoryMarshal.GetReference(_chars);
        }

        public ref char this[int index]
        {
            get
            {
                Debug.Assert(index < _pos);
                return ref _chars[index];
            }
        }

        public override string ToString()
        {
            return _chars.Slice(0, _pos).ToString();
        }

        /// <summary>Returns the underlying storage of the builder.</summary>
        public Span<char> RawChars => _chars;

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
        public ReadOnlySpan<char> AsSpan(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }
            return _chars.Slice(0, _pos);
        }

        public ReadOnlySpan<char> AsSpan() => _chars.Slice(0, _pos);
        public ReadOnlySpan<char> AsSpan(int start) => _chars.Slice(start, _pos - start);
        public ReadOnlySpan<char> AsSpan(int start, int length) => _chars.Slice(start, length);

        public bool TryCopyTo(Span<char> destination, out int charsWritten)
        {
            if (_chars.Slice(0, _pos).TryCopyTo(destination))
            {
                charsWritten = _pos;
                Dispose();
                return true;
            }
            else
            {
                charsWritten = 0;
                Dispose();
                return false;
            }
        }

        public void Insert(int index, char value, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }

            int remaining = _pos - index;
            _chars.Slice(index, remaining).CopyTo(_chars.Slice(index + count));
            _chars.Slice(index, count).Fill(value);
            _pos += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            int pos = _pos;
            if ((uint)pos < (uint)_chars.Length)
            {
                _chars[pos] = c;
                _pos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string s)
        {
            int pos = _pos;
            if (s.Length == 1 && (uint)pos < (uint)_chars.Length) // very common case, e.g. appending strings from NumberFormatInfo like separators, percent symbols, etc.
            {
                _chars[pos] = s[0];
                _pos = pos + 1;
            }
            else
            {
                AppendSlow(s);
            }
        }

        private void AppendSlow(string s)
        {
            int pos = _pos;
            if (pos > _chars.Length - s.Length)
            {
                Grow(s.Length);
            }

            s.AsSpan().CopyTo(_chars.Slice(pos));
            _pos += s.Length;
        }

        public void Append(char c, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }

            Span<char> dst = _chars.Slice(_pos, count);
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = c;
            }
            _pos += count;
        }

        public unsafe void Append(char* value, int length)
        {
            int pos = _pos;
            if (pos > _chars.Length - length)
            {
                Grow(length);
            }

            Span<char> dst = _chars.Slice(_pos, length);
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = *value++;
            }
            _pos += length;
        }

        public void Append(ReadOnlySpan<char> value)
        {
            int pos = _pos;
            if (pos > _chars.Length - value.Length)
            {
                Grow(value.Length);
            }

            value.CopyTo(_chars.Slice(_pos));
            _pos += value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            int origPos = _pos;
            if (origPos > _chars.Length - length)
            {
                Grow(length);
            }

            _pos = origPos + length;
            return _chars.Slice(origPos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int requiredAdditionalCapacity)
        {
            Debug.Assert(requiredAdditionalCapacity > 0);

            int requestedSize = _pos + requiredAdditionalCapacity;

#if STACKBUFFER
            if (requestedSize <= DefaultBufferSize
#if TRACKRENT
                && t_InitialMinRent <= requestedSize
#endif
                )
            {
                fixed (char* c = _default)
                {
                    _chars = new Span<char>(c, DefaultBufferSize);
                }
                return;
            }
#endif

#if TRACKRENT
            // We'll always grab the last max rent if it's bigger on the assumption that we can
            // avoid extra copying / multiple grows. The ArrayPool will eventually flush unused
            // buffers and we'll decrement our max rent size if we don't utilize the full buffer.

            // If doubling up is greater, we'll go for that, up to 4K.

            int rentSize = Math.Max(Math.Max(requestedSize, t_InitialMinRent), Math.Min(_chars.Length * 2, 4096));
#else
            int rentSize = Math.Max(requestedSize, Math.Min(_chars.Length * 2, 4096));
#endif

            // At least double the size
            // TODO: This should be smarter about large buffers- we don't want to grow too much.)
            char[] poolArray = ArrayPool<char>.Shared.Rent(rentSize);

            if (_chars.Length > 0)
                _chars.CopyTo(poolArray);

            char[] toReturn = _arrayToReturnToPool;
            _chars = _arrayToReturnToPool = poolArray;
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            char[] toReturn = _arrayToReturnToPool;
            int length = _pos;

            this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
            if (toReturn != null)
            {
#if TRACKRENT
                // Remember our usage to hint our next rent
                if (length > t_InitialMinRent)
                {
                    t_InitialMinRent = Math.Min(length, MaxInitialRent);
                }
                else if (length < t_InitialMinRent && t_InitialMinRent > InitialRentDecrement)
                {
                    t_InitialMinRent -= InitialRentDecrement;
                }
#endif
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }
    }
}
