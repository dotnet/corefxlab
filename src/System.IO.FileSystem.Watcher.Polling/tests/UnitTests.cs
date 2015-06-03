// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.FileSystem;
using System.Threading;
using Xunit;

public partial class PollingWatcherUnitTests
{
    [Fact]
    public static void FileSystemWatcher_Created_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();
        long changeCount = 0;

        var watcher = new PollingWatcher(currentDir, false, 100);

        watcher.Changed += () =>
        {
            Interlocked.Increment(ref changeCount);
        };

        using (var file = new TemporaryTestFile(fileName))
        {
            Thread.Sleep(1000);
        }

        Assert.Equal(1, Interlocked.Read(ref changeCount));
    }

    [Fact]
    public static void FileSystemWatcher_Deleted_File() 
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();
        long changeCount = 0;

        var watcher = new PollingWatcher(currentDir, false, 100);

        using (var file = new TemporaryTestFile(fileName))
        {
            Thread.Sleep(200);
            watcher.Changed += () =>
            {
                Interlocked.Increment(ref changeCount);
            };
            Thread.Sleep(200);
        }

        Thread.Sleep(200);
        Assert.Equal(2, Interlocked.Read(ref changeCount));
    }

    [Fact]
    public static void FileSystemWatcher_Changed_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();
        long changeCount = 0;

        var watcher = new PollingWatcher(currentDir, false, 100);

        using (var file = new TemporaryTestFile(fileName))
        {
            watcher.Changed += () =>
            {
                Interlocked.Increment(ref changeCount);
            };

            file.WriteByte(100);
            Thread.Sleep(1000);
            Assert.Equal(1, Interlocked.Read(ref changeCount));
        }
    }
}
