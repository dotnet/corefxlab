// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Buffers.Tests
{
    public class ReferenceCounterTests
    {
        [Fact]
        public void BasicSingleThreadedCounts()
        {
            var obj = new object();
            Assert.False(ReferenceCounter.HasReference(obj));
            ReferenceCounter.AddReference(obj);
            Assert.True(ReferenceCounter.HasReference(obj));
            ReferenceCounter.AddReference(obj);
            Assert.True(ReferenceCounter.HasReference(obj));
            ReferenceCounter.Release(obj);
            Assert.True(ReferenceCounter.HasReference(obj));
            ReferenceCounter.Release(obj);
            Assert.False(ReferenceCounter.HasReference(obj));
        }

        [Fact]
        public void BasicMultiThreadedCounts()
        {
            var obj = new object();

            var t1 = Task.Run(() => {
                for (int i = 0; i < 100000; i++)
                {
                    ReferenceCounter.AddReference(obj);
                    ReferenceCounter.AddReference(obj);
                    ReferenceCounter.Release(obj);
                    ReferenceCounter.Release(obj);
                }
            });

            var t2 = Task.Run(() => {
                for (int i = 0; i < 100000; i++)
                {
                    ReferenceCounter.AddReference(obj);
                    ReferenceCounter.AddReference(obj);
                    ReferenceCounter.Release(obj);
                    ReferenceCounter.Release(obj);
                }
            });

            Task.WaitAll(t1, t2);
            Assert.False(ReferenceCounter.HasReference(obj));
        }

        [Fact]
        public void ResizingThreadTableWorks()
        {
            var defaultThreadTableSize = Environment.ProcessorCount;
            var threads = new List<WaitHandle>();

            var obj = new object();
            for (int threadNumber = 0; threadNumber < defaultThreadTableSize * 2; threadNumber++)
            {
                var handle = new AutoResetEvent(false);
                threads.Add(handle);
                var thread = new Thread(new ThreadStart(() =>
                {
                    for (int itteration = 0; itteration < 100; itteration++)
                    {
                        ReferenceCounter.AddReference(obj);
                        Thread.Sleep(10);
                        ReferenceCounter.Release(obj);
                    }
                    handle.Set();
                }));
                thread.Start();
            }
            WaitHandle.WaitAll(threads.ToArray());
            Assert.False(ReferenceCounter.HasReference(obj));
        }

        [Fact]
        public void ResizingObjectTableWorks()
        {
            var defaultObjectTableSize = 16;
            var objects = new List<object>();
            for (int objectNumber = 0; objectNumber < defaultObjectTableSize * 2; objectNumber++)
            {
                var obj = new object();
                ReferenceCounter.AddReference(obj);
                objects.Add(obj);
            }
            foreach(var obj in objects)
            {
                ReferenceCounter.Release(obj);
            }
            foreach (var obj in objects)
            {
                Assert.False(ReferenceCounter.HasReference(obj));
            }
        }
    }
}
