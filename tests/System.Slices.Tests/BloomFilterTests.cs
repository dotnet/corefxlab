// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime;
using Xunit;

namespace System.Slices.Tests
{
    public class BloomFilterTests
    {
        
        [Fact]
        public void AddMayContain()
        {
            for (int p = 0; p < 30; p++) {
                var array = new object[30];
                for (int i = 0; i < array.Length; i++) {
                    array[i] = new object();
                }
                
                var b = new BloomFilter();
                
                for (int i = 0; i < array.Length; i++) {
                    b.Add(array[i]);
                    for (int j = 0; j <= i; j++) {
                        Assert.False(b.DoesNotContain(array[j]));
                    }
                }
            }
        }

        [Fact]
        public void ClearTest()
        {
            for (int p = 0; p < 30; p++) {
                var array = new object[30];
                for (int i = 0; i < array.Length; i++) {
                    array[i] = new object();
                }
                
                var b = new BloomFilter();
                
                for (int i = 0; i < array.Length; i++) {
                    b.Add(array[i]);
                }

                b.Clear();

                for (int i = 0; i < array.Length; i++) {
                    Assert.True(b.DoesNotContain(array[i]));
                }
            }
        }
    }
}