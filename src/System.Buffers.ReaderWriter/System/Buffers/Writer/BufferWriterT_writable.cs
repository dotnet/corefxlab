// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        public void Write<TWritable>(TWritable value, TransformationFormat format) where TWritable : IWritable
        {
            int written;
            while (true)
            {
                while (!value.TryWrite(Buffer, out written, format.Format))
                {
                    Enlarge();
                }
                if (format.TryTransform(Buffer, ref written)) break;
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWritable>(TWritable value, StandardFormat format) where TWritable : IWritable
        {
            int written;
            while (!value.TryWrite(Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }
    }
}
