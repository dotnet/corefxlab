// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO.Pipelines.Testing;
using System.Linq;
using System.Text;

namespace System.IO.Pipelines.Tests
{
    internal abstract class TestBufferFactory
    {
        public static TestBufferFactory Array { get; } = new ArrayTestBufferFactory();
        public static TestBufferFactory SingleSegment { get; } = new SingleSegmentTestBufferFactory();
        public static TestBufferFactory SegmentPerByte { get; } = new BytePerSegmentTestBufferFactory();

        public abstract ReadableBuffer CreateOfSize(int size);
        public abstract ReadableBuffer CreateWithContent(byte[] data);

        public ReadableBuffer CreateWithContent(string data)
        {
            return CreateWithContent(Encoding.ASCII.GetBytes(data));
        }

        internal class ArrayTestBufferFactory : TestBufferFactory
        {
            public override ReadableBuffer CreateOfSize(int size)
            {
                return ReadableBuffer.Create(new byte[size + 20], 10, size);
            }

            public override ReadableBuffer CreateWithContent(byte[] data)
            {
                var startSegment = new byte[data.Length + 20];
                System.Array.Copy(data, 0, startSegment, 10, data.Length);
                return ReadableBuffer.Create(startSegment, 10, data.Length);
            }
        }

        internal class SingleSegmentTestBufferFactory: TestBufferFactory
        {
            public override ReadableBuffer CreateOfSize(int size)
            {
                return BufferUtilities.CreateBuffer(size);
            }

            public override ReadableBuffer CreateWithContent(byte[] data)
            {
                return BufferUtilities.CreateBuffer(data);
            }
        }

        internal class BytePerSegmentTestBufferFactory: TestBufferFactory
        {
            public override ReadableBuffer CreateOfSize(int size)
            {
                return BufferUtilities.CreateBuffer(Enumerable.Repeat(1, size).ToArray());
            }

            public override ReadableBuffer CreateWithContent(byte[] data)
            {
                var segments = new List<byte[]>();

                segments.Add(System.Array.Empty<byte>());
                foreach (var b in data)
                {
                    segments.Add(new [] { b });
                    segments.Add(System.Array.Empty<byte>());
                }

                return BufferUtilities.CreateBuffer(segments.ToArray());
            }
        }
    }
}
