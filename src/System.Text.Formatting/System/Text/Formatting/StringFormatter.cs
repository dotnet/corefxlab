// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Text.Formatting
{
    public class StringFormatter : ITextOutput, IDisposable
    {
        ResizableArray<byte> _buffer;
        ArrayPool<byte> _pool;
        public EncodingData Encoding { get; set; } = EncodingData.InvariantUtf16;
        ResizableArray<KeyValuePair<ReadOnlySpan<char>, int>> _copyQueue;


        public StringFormatter(int characterCapacity = 32, ArrayPool<byte> pool = null)
        {
            if (pool == null) _pool = ArrayPool<byte>.Shared;
            else _pool = pool;
            _buffer = new ResizableArray<byte>(_pool.Rent(characterCapacity * 2));
        }

        public void Dispose()
        {
            _pool.Return(_buffer.Items);
            _buffer.Count = 0;
        }

        public void Append(char character) {
            _buffer.Add((byte)character);
            _buffer.Add((byte)(character >> 8));
        }

        //TODO: this should use Span<byte>
        public void Append(string text)
        {
            // Strings are immutable. There is no difference between
            // queueing a string to be copied later from eagerly appending a string.
            QueueForCopy(text);
        }

        //TODO: this should use Span<byte>
        public void Append(ReadOnlySpan<char> substring)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                Append(substring[i]);
            }
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public void QueueForCopy(string text)
        {
            QueueForCopy(text.Slice());
        }

        public void QueueForCopy(ReadOnlySpan<char> substring)
        {
            // Instead of copying buffers to our buffer, and then re-copying our buffer again
            // during ToString, we can do something different. When QueueForCopy is called,
            // we record the buffer to be copied & the index we're currently at, and add them
            // to a queue. When ToString is called, we copy from our buffer up to that index,
            // copy from the queued buffer, advance to that index in the destination,
            // & continue copying from our buffer until the next queued buffer.

            _copyQueue.Add(new KeyValuePair<ReadOnlySpan<char>, int>(substring, _buffer.Count));
        }

        public unsafe override string ToString()
        {
            // NOTE: We are violating string's immutability here.
            // We allocate a 0-inited string buffer, then write our contents directly to it.
            // This is what Encoding.GetString does under the hood.
            string result = new string('\0', GetCount());

            fixed (char* pResult = result)
            {
                int thisIndex = 0; // Index in our buffer we're copying from.

                for (int i = 0; i < _copyQueue.Count; i++)
                {
                    var pair = _copyQueue[i];

                    // Copy up to the given index in the pair.
                    int queuedIndex = pair.Value;

                    fixed (byte* pSource = &_buffer.Items[thisIndex])
                    {
                        Unsafe.CopyBlock(pResult, pSource, (uint)(queuedIndex - thisIndex));
                    }

                    thisIndex = queuedIndex;
                    
                    // Copy the queued buffer.
                    // Note: This does not compile yet...
                    fixed (char* pBuffer = pair.Key)
                    {
                        Unsafe.CopyBlock(pResult, pBuffer, (uint)pair.Key.Length);
                    }
                }
            }
        }

        private int GetCount()
        {
            // Sum up the count in our buffer + the count of queued buffers.

            int result = _buffer.Count;

            for (int i = 0; i < _copyQueue.Count; i++)
            {
                result += _copyQueue[i].Key.Length;
            }

            return result;
        }

        Span<byte> IOutput.Buffer => _buffer.Free.Slice();

        void IOutput.Enlarge(int desiredBufferLength)
        {
            if (desiredBufferLength < 1) desiredBufferLength = 1;
            var doubleCount = _buffer.Free.Count * 2;
            int newSize = desiredBufferLength > doubleCount ? desiredBufferLength : doubleCount;
            var newArray = _pool.Rent(newSize + _buffer.Count);
            var oldArray = _buffer.Resize(newArray);
            _pool.Return(oldArray);
        }

        void IOutput.Advance(int bytes)
        {
            _buffer.Count += bytes;
        }
    }
}
