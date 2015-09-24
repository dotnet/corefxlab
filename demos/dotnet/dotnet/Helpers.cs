// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace dotnet
{
    internal static class Helpers
    {
        internal static void CopyAllFiles(string sourceFolder, string destinationFolder)
        {
            foreach (var sourceFilePath in Directory.EnumerateFiles(sourceFolder))
            {
                var sourceFileName = Path.GetFileName(sourceFilePath);
                if (sourceFileName == null) continue;
                var destinationFilePath = Path.Combine(destinationFolder, sourceFileName);
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                File.Copy(sourceFilePath, destinationFilePath);
            }
        }
    }
}