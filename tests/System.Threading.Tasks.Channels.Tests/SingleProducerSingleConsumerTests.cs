// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class SingleProducerSingleConsumertests : TestBase
    {
        [Fact]
        public void Enqueue_Dequeue_IsEmpty()
        {
            var spscq = new SingleProducerSingleConsumerQueue<int>();
            Assert.True(spscq.IsEmpty);

            for (int i = 0; i < 10; i++)
            {
                spscq.Enqueue(i);
                Assert.False(spscq.IsEmpty);

                int j;
                Assert.True(spscq.TryDequeue(out j));
                Assert.Equal(i, j);
                Assert.True(spscq.IsEmpty);
            }

            const int Count = 100000;

            for (int i = 0; i < Count; i++)
            {
                spscq.Enqueue(i);
                Assert.False(spscq.IsEmpty);
            }

            for (int i = 0; i < Count; i++)
            {
                Assert.False(spscq.IsEmpty);
                int j;
                Assert.True(spscq.TryDequeue(out j));
                Assert.Equal(i, j);
            }

            Assert.True(spscq.IsEmpty);
        }

        [Fact]
        public void GetEnumerator()
        {
            var spscq = new SingleProducerSingleConsumerQueue<int>();

            const int Count = 100000;
            for (int i = 1; i <= Count; i++)
            {
                spscq.Enqueue(i);
            }

            int j = 1;
            foreach (int item in spscq)
            {
                Assert.Equal(j++, item);
            }
        }

        [Fact]
        public void ValidateDebuggerAttributes()
        {
            var spscq = new SingleProducerSingleConsumerQueue<int>();
            DebuggerAttributes.ValidateDebuggerDisplayReferences(spscq);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(spscq);

            for (int i = 0; i < 10; i++)
            {
                spscq.Enqueue(i);
            }
            DebuggerAttributes.ValidateDebuggerDisplayReferences(spscq);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(spscq);
        }
    }
}
