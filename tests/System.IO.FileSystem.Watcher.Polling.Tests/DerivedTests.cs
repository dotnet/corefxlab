// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.IO.Enumeration;
using Xunit;


public partial class PollingFileSystemWatcherDerivedTests
{
    [Fact]
    public static void ShouldIncludeEntryTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string subDirectory = new DirectoryInfo(currentDir).CreateSubdirectory("sub").FullName;

        var watcher2 = new DerivedWatcher(currentDir)
        {
            PollingInterval = 1
        };
        watcher2.Start();

        try
        {
            Assert.True(watcher2.ShouldIncludeEntryCalled);
            Assert.False(watcher2.ShouldRecurseIntoEntryCalled);
        }
        finally
        {
            Directory.Delete(currentDir, true);
            watcher2.Dispose();
        }
    }

    [Fact]
    public static void ShouldRecurseIntoEntryTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string subDirectory = new DirectoryInfo(currentDir).CreateSubdirectory("sub").FullName;

        var watcher2 = new DerivedWatcher(currentDir, options: new EnumerationOptions { RecurseSubdirectories = true })
        {
            PollingInterval = 1
        };
        watcher2.Start();

        try
        {
            Assert.True(watcher2.ShouldIncludeEntryCalled);
            Assert.True(watcher2.ShouldRecurseIntoEntryCalled);
        }
        finally
        {
            Directory.Delete(currentDir, true);
            watcher2.Dispose();
        }
    }
}

public class DerivedWatcher : PollingFileSystemWatcher
{
    public DerivedWatcher(string path, string filter = "*.*", EnumerationOptions options = null) : base(path, filter, options)
    {
    }

    public bool ShouldIncludeEntryOverride { get; set; }
    public bool ShouldRecurseIntoEntryOverride { get; set; }

    public bool ShouldIncludeEntryCalled { get; set; }
    public bool ShouldRecurseIntoEntryCalled { get; set; }

    protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
    {
        ShouldIncludeEntryCalled = true;

        return base.ShouldIncludeEntry(ref entry);
    }

    protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
    {
        ShouldRecurseIntoEntryCalled = true;

        return base.ShouldRecurseIntoEntry(ref entry);
    }
}
