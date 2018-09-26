// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Tests;
using System.Collections.Generic;

namespace System.IO.Pipelines.Tests
{
    public abstract class ReadOnlyBufferFactory<T> where T : struct, IEquatable<T>
    {
        public static ReadOnlyBufferFactory<T> Array { get; } = new ArrayTestBufferFactory();
        public static ReadOnlyBufferFactory<T> OwnedMemory { get; } = new MemoryTestBufferFactory();
        public static ReadOnlyBufferFactory<T> SingleSegment { get; } = new SingleSegmentTestBufferFactory();
        public static ReadOnlyBufferFactory<T> SegmentPerPosition { get; } = new SegmentPerPositionTestBufferFactory();

        public abstract ReadOnlySequence<T> CreateOfSize(int size);
        public abstract ReadOnlySequence<T> CreateWithContent(T[] data);

        internal class ArrayTestBufferFactory : ReadOnlyBufferFactory<T>
        {
            public override ReadOnlySequence<T> CreateOfSize(int size)
            {
                return new ReadOnlySequence<T>(new T[size + 20], 10, size);
            }

            public override ReadOnlySequence<T> CreateWithContent(T[] data)
            {
                var startSegment = new T[data.Length + 20];
                System.Array.Copy(data, 0, startSegment, 10, data.Length);
                return new ReadOnlySequence<T>(startSegment, 10, data.Length);
            }
        }

        internal class MemoryTestBufferFactory : ReadOnlyBufferFactory<T>
        {
            public override ReadOnlySequence<T> CreateOfSize(int size)
            {
                return CreateWithContent(new T[size]);
            }

            public override ReadOnlySequence<T> CreateWithContent(T[] data)
            {
                var startSegment = new T[data.Length + 20];
                System.Array.Copy(data, 0, startSegment, 10, data.Length);
                return new ReadOnlySequence<T>(new Memory<T>(startSegment, 10, data.Length));
            }
        }

        internal class SingleSegmentTestBufferFactory : ReadOnlyBufferFactory<T>
        {
            public override ReadOnlySequence<T> CreateOfSize(int size)
            {
                return BufferUtilities.CreateBuffer<T>(size);
            }

            public override ReadOnlySequence<T> CreateWithContent(T[] data)
            {
                return BufferUtilities.CreateBuffer<T>(data);
            }
        }

        internal class SegmentPerPositionTestBufferFactory : ReadOnlyBufferFactory<T>
        {
            public override ReadOnlySequence<T> CreateOfSize(int size)
            {
                return CreateWithContent(new T[size]);
            }

            public override ReadOnlySequence<T> CreateWithContent(T[] data)
            {
                var segments = new List<T[]>();

                segments.Add(System.Array.Empty<T>());
                foreach (var b in data)
                {
                    segments.Add(new[] { b });
                    segments.Add(System.Array.Empty<T>());
                }

                return BufferUtilities.CreateBuffer<T>(segments.ToArray());
            }
        }
    }
}
