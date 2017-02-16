// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime;
using Xunit;

namespace System.Slices.Tests {
    class DisposeCount : IDisposable {
        public int disposedCount = 0;

        public DisposeCount () { }

        public void Dispose () {
            disposedCount++;
        }
    }

    public class DelayedDisposerTests {

        [Fact]
        public void BasicTest () {
            DisposeCount[] array = new DisposeCount[1000];

            for (int i = 0; i < array.Length; i++) {
                array[i] = new DisposeCount ();
            }

            for (int i = 0; i < array.Length; i++) {
                DelayedDisposer.Add (array[i]);
            }

            // Make sure there is something that hasn't been 
            // disposed, so we can test flush.
            bool anyLeft = false;
            for (int i = 0; i < array.Length; i++) {
                anyLeft |= array[i].disposedCount == 0;
            }

            // Add something, so that we can check flush
            int n = 0;
            while (!anyLeft) {
                if (anyLeft) break;
                array[0] = new DisposeCount ();
                DelayedDisposer.Add (array[0]);
                anyLeft = array[0].disposedCount == 0;
                n++;
                if (n == 100) {
                    // Clearly not very Delayed
                    break;
                }
            }

            DelayedDisposer.Flush ();

            for (int i = 0; i < array.Length; i++) {
                Assert.True (array[i].disposedCount == 1, "Failed! Index " + i);
            }
        }

        [Fact]
        public void ReferencedTest () {
            DisposeCount[] array = new DisposeCount[1000];

            for (int i = 0; i < array.Length; i++) {
                array[i] = new DisposeCount ();
                ReferenceCounter.AddReference (array[i]);
            }

            for (int i = 0; i < array.Length; i++) {
                DelayedDisposer.Add (array[i]);
            }

            //Nothing should be removed
            for (int i = 0; i < array.Length; i++) {
                Assert.True (array[i].disposedCount == 0, "Element should not have been reclaimed: index " + i);
            }

            for (int i = 1; i < array.Length; i += 2) {
                ReferenceCounter.Release (array[i]);
            }

            DelayedDisposer.Flush ();

            for (int i = 0; i < array.Length; i++) {
                if (i % 2 == 0) {
                    Assert.True (array[i].disposedCount == 0, "Element should not have been reclaimed: index " + i);
                }
            }

            for (int i = 0; i < array.Length; i += 2) {
                ReferenceCounter.Release (array[i]);
            }

            DelayedDisposer.Flush ();

            for (int i = 0; i < array.Length; i++) {
                Assert.True (array[i].disposedCount == 1, "Element should have been reclaimed: index " + i);
            }
        }
    }
}