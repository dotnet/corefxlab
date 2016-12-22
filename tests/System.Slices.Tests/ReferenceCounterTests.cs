// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Slices.Tests
{
    public class ReferenceCounterTests
    {
        [Fact]
        public void BasicSingleThreadedCounts()
        {
            var obj = new object();
            var counter = new ReferenceCounter();
            Assert.False(counter.HasReference(obj));
            counter.AddReference(obj);
            Assert.True(counter.HasReference(obj));
            counter.AddReference(obj);
            Assert.True(counter.HasReference(obj));
            counter.Release(obj);
            Assert.True(counter.HasReference(obj));
            counter.Release(obj);
            Assert.False(counter.HasReference(obj));
        }

        [Fact]
        public void BasicMultiThreadedCounts()
        {
            var obj = new object();
            var counter = new ReferenceCounter();

            var t1 = Task.Run(() => {
                for (int i = 0; i < 100000; i++)
                {
                    counter.AddReference(obj);
                    counter.AddReference(obj);
                    counter.Release(obj);
                    counter.Release(obj);
                }
            });

            var t2 = Task.Run(() => {
                for (int i = 0; i < 100000; i++)
                {
                    counter.AddReference(obj);
                    counter.AddReference(obj);
                    counter.Release(obj);
                    counter.Release(obj);
                }
            });

            Task.WaitAll(t1, t2);
            Assert.False(counter.HasReference(obj));
        }

        [Fact]
        public void ResizingThreadTableWorks()
        {
            var defaultThreadTableSize = Environment.ProcessorCount;
            var threads = new List<WaitHandle>();
            var counter = new ReferenceCounter();
            var obj = new object();
            for (int threadNumber = 0; threadNumber < defaultThreadTableSize * 2; threadNumber++)
            {
                var handle = new AutoResetEvent(false);
                threads.Add(handle);
                var thread = new Thread(new ThreadStart(() =>
                {
                    for (int itteration = 0; itteration < 100; itteration++)
                    {
                        counter.AddReference(obj);
                        Thread.Sleep(10);
                        counter.Release(obj);
                    }
                    handle.Set();
                }));
                thread.Start();
            }
            WaitHandle.WaitAll(threads.ToArray());
            Assert.False(counter.HasReference(obj));
        }

        [Fact]
        public void ResizingObjectTableWorks()
        {
            var defaultObjectTableSize = 16;
            var objects = new List<object>();
            var counter = new ReferenceCounter();
            for (int objectNumber = 0; objectNumber < defaultObjectTableSize * 2; objectNumber++)
            {
                var obj = new object();
                counter.AddReference(obj);
                objects.Add(obj);
            }
            foreach(var obj in objects)
            {
                counter.Release(obj);
            }
            foreach (var obj in objects)
            {
                Assert.False(counter.HasReference(obj));
            }
        }
    }
}
