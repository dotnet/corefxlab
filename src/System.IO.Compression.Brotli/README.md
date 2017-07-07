# System.IO.Compression.Brotli

## Introduction

Brotli is a generic-purpose lossless compression algorithm that compresses data
using a combination of a modern variant of the LZ77 algorithm, Huffman coding
and 2nd order context modeling, with a compression ratio comparable to the best
currently available general-purpose compression methods. It is similar in speed
with deflate but offers more dense compression.

The specification of the Brotli Compressed Data Format is defined in [RFC 7932](https://www.ietf.org/rfc/rfc7932.txt).

Brotli encoding is supported by most web browsers, major web servers, and some CDNs.

## Brotli Class
```C#
public static class Brotli {
        public static TransformationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref Brotli.State state);
        public static TransformationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref Brotli.State state);
        public static TransformationStatus FlushEncoder(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref Brotli.State state, bool isFinished=true);
        public static int GetMaximumCompressedSize(int inputSize);
        public struct State : IDisposable {
            public bool CompressMode { get; }
            public void Dispose();
            public void SetQuality();
            public void SetQuality(uint quality);
            public void SetWindow();
            public void SetWindow(uint window);
        }
    }
```
```out bytesConsumed``` - number of bytes from source, which used in this call
```out bytesWritten``` - number of written to destination bytes 

The ```Compress``` performs data compression. It sets ```source``` data should be compressed and return compressed data in ```destination```. ```state.CompressMode``` should be ```true``` .


The ```FlushEncoder``` return compressed data, which have sent to ```state``` using Compress. 
S
The ```Decompress``` Decompresses the data in ```source``` into ```destination```. 

```State``` uses in both compress and decompress mode to save a temporary status and data of process.

```CompressMode =  true``` shows that ```State``` in compress mode, ```false``` in decompress mode. 

```SetQuality``` alows you to set quality of compression ```0 to 11```. The higher quality means higher compression ratio. 

```SetWindow``` - Logarithm of Recommended sliding LZ77 window size. Encoder may reduce this value, e.g. if input is much smaller than window size. Window size is (1 << value) - 16. Possible values: ```11 to 24```

### Examples
Simple method to compress bytes to file.
```C#

 static void CompressSimple(byte[] bytes, string OutFile)
 {
    Brotli.State state = new Brotli.State();
    byte[] compressed = new byte[Brotli.GetMaximumCompressedSize(bytes.Length)];
    state.SetQuality();
    TransformationStatus result = Brotli.Compress(bytes, compressed, out int consumed, out int written, ref state);
    while (result != TransformationStatus.Done)
    {
        result = Brotli.Compress(bytes, compressed, out consumed, out written, ref state);
        bytes = bytes.AsSpan().Slice(consumed).ToArray();
    }
    result = Brotli.FlushEncoder(Array.Empty<byte>(), compressed, out consumed, out written, ref state);
    byte[] ans = new byte[written];
    Array.Copy(compressed, ans, written);
    File.WriteAllBytes(OutFile, ans);
 }
```
Simple method to decompress bytes to file. If out data is larger than destination ```result``` be ```TransformationStatus.DestinationTooSmall``` and it's available to call Decompress again to collect rest of data.
```C#
 static void DecompSimple(byte[] bytes, string outFile, int decompressedLength)
 {
    Brotli.State state = new Brotli.State();
    byte[] data = ms.ToArray();
    byte[] decompressed = new byte[decLen];
    TransformationStatus result = Brotli.Decompress(data, decompressed, out int consumed, out int written, ref state);
    File.WriteAllBytes(outFile, decompressed);
 }

```

## BrotliStream
```C#
 public class BrotliStream : Stream {
    public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen=false, int bufferSize=65520);
    public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen, int bufferSize, CompressionLevel quality);
    public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen, int bufferSize, CompressionLevel quality, uint windowSize);
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public override long Length { get; }
    public override long Position { get; set; }
    public override int ReadTimeout { get; set; }
    public override int WriteTimeout { get; set; }
    protected override void Dispose(bool disposing);
    public override void Flush();
    protected virtual void FlushEncoder(bool finished);
    public override int Read(byte[] buffer, int offset, int count);
    public override long Seek(long offset, SeekOrigin origin);
    public override void SetLength(long value);
    public override void Write(byte[] buffer, int offset, int count);
}
```

```Write``` write and compress bytes to stream, can only be called in Compress mode

```Read``` read bytes from stream and write them to buffer, can only be called in Decompress mode

Default ```CompressionLevel``` is ```Optimal```(max compression ratio).

### Examples
#### Compress
```C#
public void CompressData(byte[] data, MemoryStream compressed)
{
    using (var compressor = new BrotliStream(compressed, CompressionMode.Compress, true))
    {
        compressor.Write(data, i, chunkSize);
        compressor.Dispose();
    }
}
```

After calling method compressed data will be in ```MemoryStream compressed``` .

#### Decompress
Decompress and write data to file.
```C#
private void DecompressDataToFile(byte[] compressedData, string outFile)
{
    MemoryStream compressed = new MemoryStream(compressedData);
    FileStream fs = File.Create(outFile);
    using (BrotliStream decompressBrotli = new BrotliStream(compressed, CompressionMode.Decompress))
    {
        decompressBrotli.CopyTo(fs);
    }
    fs.Dispose();
}
```

#### Dynamic Web Compress
You can add code like this to you Global.asax.cs in ASP.NET aplication to support dynamic compress
```
public static bool IsBrotliSupported()
{
    string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
    if (!string.IsNullOrEmpty(AcceptEncoding) &&
            (AcceptEncoding.Contains("brotli") || AcceptEncoding.Contains("br")))
        return true;
    return false;
}

public static void BrotliEncodePage()
{
    HttpResponse Response = HttpContext.Current.Response;

    if (IsBrotliSupported())
    {
        string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
        Response.Filter = new System.IO.Compression.BrotliStream(Response.Filter,
        System.IO.Compression.CompressionMode.Compress);
        Response.Headers.Remove("Content-Encoding");
        Response.AppendHeader("Content-Encoding", "brotli");
    }
}

```
#### WebServer Request and Response
```C#
using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Examples.System.Net
{
    class WebRequestPostBrotli
    {
        static void Main()
        {
            //Send response
            WebRequest request = WebRequest.Create("http://www.contoso.com/PostAccepter.aspx");
            request.Method = "POST";
            string someData = "This data will compress and send to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(someData);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            BrotliStream compressionStream= new BrotliStream(dataStream, CompressionMode.Compress, true);
            compressionStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            //Get response

            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
            BrotliStream decompressStream = new BrotliStream(dataStream, CompressionMode.Decompress);
            StreamReader reader = new StreamReader();
            decompressStream.CopyTo(reader);

            string responseFromServer = reader.ReadToEnd();
            
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}

```
