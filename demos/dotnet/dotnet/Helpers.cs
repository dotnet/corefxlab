// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace dotnet
{
    internal static class Helpers
    {
        internal static bool CopyFile(string file, string destinationFolder, Log log)
        {
            try
            {
                var sourceFileName = Path.GetFileName(file);
                if (sourceFileName == null || !File.Exists(file))
                {
                    log.Error("File not found {0}", file);
                    return false;
                }
                var destinationFilePath = Path.Combine(destinationFolder, sourceFileName);
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                File.Copy(file, destinationFilePath);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }
    }
}