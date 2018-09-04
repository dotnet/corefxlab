// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;
using Xunit.Abstractions;

public class Tests
{
    private readonly ITestOutputHelper output;

    public Tests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void TwoSpansCreatedOverSameIntArrayAreEqual()
    {
        for (int i = 0; i < 2; i++)
        {
            var ints = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Try out two ways of creating a slice:
            Span<int> slice;
            if (i == 0)
            {
                slice = new Span<int>(ints);
            }
            else
            {
                slice = ints.AsSpan();
            }
            Assert.Equal(ints.Length, slice.Length);
            for (int j = 0; j < ints.Length; j++)
            {
                Assert.Equal(ints[j], slice[j]);
            }
        }
    }

    [Fact]
    public void TwoReadOnlySpansCreatedOverSameIntArrayAreEqual()
    {
        for (int i = 0; i < 2; i++)
        {
            var ints = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Try out two ways of creating a slice:
            ReadOnlySpan<int> slice;
            if (i == 0)
            {
                slice = new ReadOnlySpan<int>(ints);
            }
            else
            {
                slice = ints.AsSpan();
            }
            Assert.Equal(ints.Length, slice.Length);
            for (int j = 0; j < ints.Length; j++)
            {
                Assert.Equal(ints[j], slice[j]);
            }
        }
    }

    [Fact]
    public void TwoSpansCreatedOverSameStringsAreEqual()
    {
        var str = "Hello, Slice!";
        ReadOnlySpan<char> slice = str.AsSpan();
        Assert.Equal(str.Length, slice.Length);

        for (int j = 0; j < str.Length; j++)
        {
            Assert.Equal(str[j], slice[j]);
        }
    }

    [Fact]
    public void TwoSpansCreatedOverSameByteArayAreEqual()
    {
        unsafe
        {
            byte* buffer = stackalloc byte[256];
            for (int i = 0; i < 256; i++) { buffer[i] = (byte)i; }
            Span<byte> slice = new Span<byte>(buffer, 256);
            Assert.Equal(256, slice.Length);

            for (int j = 0; j < slice.Length; j++)
            {
                Assert.Equal(buffer[j], slice[j]);
            }
        }
    }

    [Fact]
    public void TwoReadOnlySpansCreatedOverSameByteArayAreEqual()
    {
        unsafe
        {
            byte* buffer = stackalloc byte[256];
            for (int i = 0; i < 256; i++) { buffer[i] = (byte)i; }
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(buffer, 256);
            Assert.Equal(256, slice.Length);

            for (int j = 0; j < slice.Length; j++)
            {
                Assert.Equal(buffer[j], slice[j]);
            }
        }
    }

    [Fact]
    public void TwoSpansCreatedOverSameIntSubarrayAreEqual()
    {
        var slice = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsSpan();

        // First a simple subslice over the whole array, using start.
        {
            var subslice1 = slice.Slice(0);
            Assert.Equal(slice.Length, subslice1.Length);
            for (int i = 0; i < slice.Length; i++)
            {
                Assert.Equal(slice[i], subslice1[i]);
            }
        }

        // Next a simple subslice over the whole array, using start and end.
        {
            var subslice2 = slice.Slice(0, slice.Length);
            Assert.Equal(slice.Length, subslice2.Length);
            for (int i = 0; i < slice.Length; i++)
            {
                Assert.Equal(slice[i], subslice2[i]);
            }
        }

        // Now do something more interesting; just take half the array.
        {
            int mid = slice.Length / 2;
            var subslice3 = slice.Slice(mid);
            Assert.Equal(mid, subslice3.Length);
            for (int i = mid, j = 0; i < slice.Length; i++, j++)
            {
                Assert.Equal(slice[i], subslice3[j]);
            }
        }

        // Now take a hunk out of the middle.
        {
            int st = 3;
            int ed = 7;
            var subslice4 = slice.Slice(st, 4);
            Assert.Equal(ed - st, subslice4.Length);
            for (int i = ed, j = 0; i < ed; i++, j++)
            {
                Assert.Equal(slice[i], subslice4[j]);
            }
        }
    }

    [Fact]
    public bool TestPerfLoop()
    {
        var ints = new int[10000];
        Random r = new Random(1234);
        for (int i = 0; i < ints.Length; i++) { ints[i] = r.Next(); }

        Tester.CleanUpMemory();

        var sw = System.Diagnostics.Stopwatch.StartNew();
        int x = 0;
        for (int i = 0; i < 10000; i++)
        {
            for (int j = 0; j < ints.Length; j++)
            {
                x += ints[i];
            }
        }
        sw.Stop();
        output.WriteLine("    - ints : {0}", sw.Elapsed);

        Tester.CleanUpMemory();

        var slice = ints.AsSpan();
        sw.Reset();
        sw.Start();
        int y = 0;
        for (int i = 0; i < 10000; i++)
        {
            for (int j = 0; j < slice.Length; j++)
            {
                y += slice[i];
            }
        }
        sw.Stop();
        output.WriteLine("    - slice: {0}", sw.Elapsed);

        Assert.Equal(x, y);
        return true;
    }
}

