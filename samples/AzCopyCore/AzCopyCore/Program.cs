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
        Console.WriteLine("\t - /SourceKey:<key>");
    }

    static void Main(string[] args)
    {
        Log.Listeners.Add(new ConsoleTraceListener());
        Log.Switch.Level = SourceLevels.Error;

        var options = new CommandLine(args);
        ReadOnlyMemory<char> source = options.Get("/Source:");
        ReadOnlyMemory<char> destination = options.Get("/Dest:");

        // transfer from file system to storage
        if (destination.Span.StartsWith("http://"))
        {
            TransferDirectoryToStorage(source, destination, options);
        }

        // transfer from storage to file system
        else if (source.Span.StartsWith("http://"))
        {
            TransferStorageFileToDirectory(source, destination, options);
        }

        else { PrintUsage(); }

        if (Debugger.IsAttached)
        {
            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();
        }
    }

    static void TransferDirectoryToStorage(ReadOnlyMemory<char> localDirectory, ReadOnlyMemory<char> storageDirectory, CommandLine options)
    {
        var directoryPath = new string(localDirectory.Span);
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

        ReadOnlyMemory<char> storageFullPath = storageDirectory.Slice("http://".Length);
        int pathStart = storageFullPath.Span.IndexOf('/');
        ReadOnlyMemory<char> host = storageFullPath.Slice(0, pathStart);
        ReadOnlyMemory<char> path = storageFullPath.Slice(pathStart + 1);
        ReadOnlyMemory<char> account = storageFullPath.Slice(0, storageFullPath.Span.IndexOf('.'));

        using (var client = new StorageClient(keyBytes, account, host))
        {
            client.Log = Log;
            foreach (var filepath in Directory.EnumerateFiles(directoryPath))
            {
                var filename = Path.GetFileName(filepath);
                var storagePath = new string(path.Span) + "/" + filename;

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

        using (var fileStream = File.OpenRead(filename))
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

    static void TransferStorageFileToDirectory(ReadOnlyMemory<char> storageFile, ReadOnlyMemory<char> localDirectory, CommandLine options)
    {
        var directory = new string(localDirectory.Span);

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

        ReadOnlyMemory<char> storageFullPath = storageFile.Slice("http://".Length);
        int pathStart = storageFullPath.Span.IndexOf('/');
        ReadOnlyMemory<char> host = storageFullPath.Slice(0, pathStart);
        ReadOnlyMemory<char> storagePath = storageFullPath.Slice(pathStart + 1);
        ReadOnlyMemory<char> account = storageFullPath.Slice(0, storageFullPath.Span.IndexOf('.'));
        ReadOnlyMemory<char> file = storagePath.Slice(storagePath.Span.LastIndexOf('/') + 1);

        // TODO (pri 3): use the new directory APIs once they become avaliable     
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string destinationPath = directory + "\\" + new string(file.Span);
        using (var client = new StorageClient(keyBytes, account, host))
        {
            if (CopyStorageFileToLocalFile(client, new string(storagePath.Span), destinationPath).Result)
            {
                Console.WriteLine($"Downloaded {storagePath} to {destinationPath}");
            }
        }
    }

    static async ValueTask<bool> CopyLocalFileToStorageFile(StorageClient client, string localFilePath, string storagePath)
    {
        // TODO (pri 3): make file i/o more efficient
        FileInfo fileInfo = new FileInfo(localFilePath);

        var createRequest = new CreateFileRequest(storagePath, fileInfo.Length);
        var response = await client.SendRequest(createRequest).ConfigureAwait(false);

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
        var response = await client.SendRequest(request).ConfigureAwait(false);

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


