// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        #region Int32
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, default))
            {
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }

        public void Write(int value, TransformationFormat format)
        {
            int written;
            while (true)
            {
                Span<byte> buffer = Buffer;
                while (!Utf8Formatter.TryFormat(value, Buffer, out written, format.Format))
                {
                    Enlarge();
                }
                if (format.TryTransform(buffer, ref written))
                {
                    Advance(written);
                    return;
                }
                Enlarge();
            }
        }
        #endregion

        #region UInt64
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }
        #endregion
    }
}
