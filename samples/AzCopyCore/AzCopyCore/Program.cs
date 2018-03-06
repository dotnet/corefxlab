// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Azure.Authentication;
using System.Azure.Storage;
using System.Azure.Storage.Requests;
using System.Buffers;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

static class Program
{
    internal static TraceSource Log = new TraceSource("AzCopyCore");

    static void PrintUsage()
    {
        Console.WriteLine("dotnet AzCopyCore.dll /Source:<source> /Dest:<destination> [Options]");
        Console.WriteLine("\tOptions:");
        Console.WriteLine("\t - /DestKey:<key> (or /@:<Path to ResponseFile.txt> with the key)");
        Console.WriteLine("\t - /SourceKey:<key> (or /@:<Path to ResponseFile.txt> with the key)");
    }

    static void Main(string[] args)
    {
        Log.Listeners.Add(new ConsoleTraceListener());
        Log.Switch.Level = SourceLevels.Error;

        long before = GC.GetAllocatedBytesForCurrentThread();

        var options = new CommandLine(args);
        ReadOnlySpan<char> source = options.GetSpan("/Source:");
        ReadOnlySpan<char> destination = options.GetSpan("/Dest:");

        // transfer from file system to storage
        if (destination.StartsWith("http://"))
        {
            var sw = new Stopwatch();
            sw.Start();
            TransferDirectoryToStorage(source, destination, options);
            sw.Stop();
            Console.WriteLine("Elapsed time: " + sw.ElapsedMilliseconds + " ms");
        }

        // transfer from storage to file system
        else if (source.StartsWith("http://"))
        {
            TransferStorageFileToDirectory(source, destination, options);
        }

        else { PrintUsage(); }

        long after = GC.GetAllocatedBytesForCurrentThread();
        Console.WriteLine($"GC Allocations: {after - before} bytes");

        if (Debugger.IsAttached)
        {
            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();
        }
    }

    static void TransferDirectoryToStorage(ReadOnlySpan<char> localDirectory, ReadOnlySpan<char> storageDirectory, CommandLine options)
    {
        var directoryPath = new string(localDirectory);
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Source directory not found.");
            return;
        }

        byte[] keyBytes;
        if (options.Contains("/DestKey:"))
        {
            keyBytes = options["/DestKey:"].ComputeKeyBytes();
        }
        else if (options.Contains("/@:"))
        {
            if (!TryGetKey(options.GetString("/@:"), out ReadOnlySpan<char> line)) return;
            keyBytes = line.ComputeKeyBytes();
        }
        else
        {
            Console.WriteLine("Did not find the /DestKey option nor the /@ option (to pass the response file).");
            return;
        }

        ReadOnlySpan<char> storageFullPath = storageDirectory.Slice("http://".Length);
        int accEnd = storageFullPath.IndexOf('.');
        ReadOnlySpan<char> account = storageFullPath.Slice(0, accEnd);
        int pathStart = storageFullPath.Slice(accEnd).IndexOf('/') + accEnd;
        ReadOnlySpan<char> host = storageFullPath.Slice(0, pathStart);
        ReadOnlySpan<char> path = storageFullPath.Slice(pathStart + 1);

        using (var client = new StorageClient(keyBytes, account, host))
        {
            client.Log = Log;
            foreach (string filepath in Directory.EnumerateFiles(directoryPath))
            {
                // TODO: Use Path.Join when it becomes available
                // https://github.com/dotnet/corefx/issues/25536
                // https://github.com/dotnet/corefx/issues/25539
                var storagePath = new string(path) + "/" + Path.GetFileName(filepath);

                // TODO (pri 3): this loop keeps going through all files, even if the key is wrong
                if (CopyLocalFileToStorageFile(client, filepath, storagePath).Result)
                {
                    Console.WriteLine($"Uploaded {filepath} to {storagePath}");
                }
            }
        }
    }

    private static bool TryGetKey(string filename, out ReadOnlySpan<char> line)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("Could not find the response file. Please specify the full path.");
            return false;
        }

        using (FileStream fileStream = File.OpenRead(filename))
        using (var streamReader = new StreamReader(fileStream))
        {
            string firstLine;
            if ((firstLine = streamReader.ReadLine()) != null)
            {
                line = firstLine.AsReadOnlySpan();
            }
            else
            {
                Console.WriteLine("Could not read the key from the specified file.");
                return false;
            }
        }
        return true;
    }

    static void TransferStorageFileToDirectory(ReadOnlySpan<char> storageFile, ReadOnlySpan<char> localDirectory, CommandLine options)
    {
        var directory = new string(localDirectory);

        byte[] keyBytes;
        if (options.Contains("/SourceKey:"))
        {
            keyBytes = options["/SourceKey:"].ComputeKeyBytes();
        }
        else if (options.Contains("/@:"))
        {
            if (!TryGetKey(options.GetString("/@:"), out ReadOnlySpan<char> line)) return;
            keyBytes = line.ComputeKeyBytes();
        }
        else
        {
            Console.WriteLine("Did not find the /SourceKey option nor the /@ option (to pass the response file).");
            return;
        }

        ReadOnlySpan<char> storageFullPath = storageFile.Slice("http://".Length);
        int accEnd = storageFullPath.IndexOf('.');
        ReadOnlySpan<char> account = storageFullPath.Slice(0, accEnd);
        int pathStart = storageFullPath.Slice(accEnd).IndexOf('/') + accEnd;
        ReadOnlySpan<char> host = storageFullPath.Slice(0, pathStart);
        ReadOnlySpan<char> storagePath = storageFullPath.Slice(pathStart + 1);
        ReadOnlySpan<char> file = storagePath.Slice(storagePath.LastIndexOf('/') + 1);

        // TODO (pri 3): use the new directory APIs once they become avaliable     
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string destinationPath = directory + "\\" + new string(file);
        using (var client = new StorageClient(keyBytes, account, host))
        {
            string storagePathStr = new string(storagePath);
            if (CopyStorageFileToLocalFile(client, storagePathStr, destinationPath).Result)
            {
                Console.WriteLine($"Downloaded {storagePathStr} to {destinationPath}");
            }
        }
    }

    static async ValueTask<bool> CopyLocalFileToStorageFile(StorageClient client, string localFilePath, string storagePath)
    {
        // TODO (pri 3): make file i/o more efficient
        FileInfo fileInfo = new FileInfo(localFilePath);

        var createRequest = new CreateFileRequest(storagePath, fileInfo.Length);
        StorageResponse response = await client.SendRequest(createRequest).ConfigureAwait(false);

        if (response.StatusCode == 201)
        {
            using (var bytes = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                var putRequest = new PutRangeRequest(storagePath, bytes);
                response = await client.SendRequest(putRequest).ConfigureAwait(false);
                if (response.StatusCode == 201) return true;
            }
        }

        Log.TraceEvent(TraceEventType.Error, 0, "Response Status Code {0}", response.StatusCode);
        return false;
    }

    static async ValueTask<bool> CopyStorageFileToLocalFile(StorageClient client, string storagePath, string localFilePath)
    {
        var request = new GetFileRequest(storagePath);
        StorageResponse response = await client.SendRequest(request).ConfigureAwait(false);

        if (response.StatusCode != 200)
        {
            Log.TraceEvent(TraceEventType.Error, 0, "Response Status Code {0}", response.StatusCode);
            return false;
        }

        ulong bytesToRead = response.ContentLength;
        using (var file = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
        {
            await file.WriteAsync(response.Body, bytesToRead);
        }

        return true;
    }
}


