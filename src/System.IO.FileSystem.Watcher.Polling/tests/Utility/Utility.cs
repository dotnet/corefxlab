// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;
using Xunit;
using System.IO.FileSystem;

public static class Utility
{
    // events are reported asynchronously by the OS, so allow an amount of time for
    // them to arrive before testing an assertion.
    public const int Timeout = 500;

    public static TemporaryTestFile CreateTestFile([CallerMemberName] string path = null)
    {
        if (String.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return new TemporaryTestFile(path);
    }

    public static TemporaryTestDirectory CreateTestDirectory([CallerMemberName] string path = null)
    {
        if (String.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(path);
        }

        return new TemporaryTestDirectory(path);
    }

    public static void EnsureDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
