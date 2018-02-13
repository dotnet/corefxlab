using System;
using System.Azure.Authentication;
using System.Azure.Storage;
using System.Azure.Storage.Requests;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

static class Program
{
    internal static TraceSource Log = new TraceSource("AzCopyCore");

    static void PrintUsage()
    {
        Console.WriteLine("dotnet AzCopyCore.dll /Source:<source> /Dest:<destination> [Options]");
        Console.WriteLine("\tOptions:");
        Console.WriteLine("\t - /DestKey:<key>");
        Console.WriteLine("\t - /SourceKey:<key>");
    }

    static void Main(string[] args)
    {
        Log.Listeners.Add(new TextWriterTraceListener(Console.Out));
        Log.Switch.Level = SourceLevels.Error;

        var options = new CommandOptions(args);
        ReadOnlyMemory<char> source = options.Get("/Source:");
        ReadOnlyMemory<char> destination = options.Get("/Dest:");

        // transfer from file system to storage
        if (destination.Span.StartsWith("http://")) {
            TransferDirectoryToStorage(source, destination, options);
        }

        // transfer from storage to file system
        else if (source.Span.StartsWith("http://")) {
            TransferStorageFileToDirectory(source, destination, options);
        }

        else { PrintUsage(); }

        if (Debugger.IsAttached)
        {
            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();
        }
    }

    static void TransferDirectoryToStorage(ReadOnlyMemory<char> localDirectory, ReadOnlyMemory<char> storageDirectory, CommandOptions options)
    {
        var directoryPath = new string(localDirectory.Span);     
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Source directory not found.");
            return;
        }
        if (!options.Contains("/DestKey:"))
        {
            Console.WriteLine("/DestKey option not found.");
            return;
        }

        ReadOnlyMemory<char> key = options.Get("/DestKey:");
        byte[] keyBytes = key.Span.ComputeKeyBytes();

        ReadOnlyMemory<char> storageFullPath = storageDirectory.Slice(7);
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

    static void TransferStorageFileToDirectory(ReadOnlyMemory<char> storageFile, ReadOnlyMemory<char> localDirectory, CommandOptions options)
    {
        var directory = new string(localDirectory.Span);
        if (!options.Contains("/SourceKey:"))
        {
            Console.WriteLine("/SourceKey option not found.");
            return;
        }

        ReadOnlyMemory<char> key = options.Get("/SourceKey:");
        byte[] keyBytes = key.Span.ComputeKeyBytes();

        ReadOnlyMemory<char> storageFullPath = storageFile.Slice(7);
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
            if(CopyStorageFileToLocalFile(client, new string(storagePath.Span), destinationPath).Result)
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

        Log.WriteError($"Response Error {response.StatusCode}");
        return false;
    }

    static async ValueTask<bool> CopyStorageFileToLocalFile(StorageClient client, string storagePath, string localFilePath)
    {
        var request = new GetFileRequest(storagePath);
        var response = await client.SendRequest(request).ConfigureAwait(false);

        if (response.StatusCode != 200)
        {
            Log.WriteError($"Response Error {response.StatusCode}");
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


