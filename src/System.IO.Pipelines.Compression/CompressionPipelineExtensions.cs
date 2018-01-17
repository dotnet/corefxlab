// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Compression
{
    public static class CompressionPipelineExtensions
    {
        public static IPipeReader DeflateDecompress(this IPipeReader reader, PipeOptions options)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader DeflateCompress(this IPipeReader reader, PipeOptions options, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader GZipDecompress(this IPipeReader reader, PipeOptions options)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeWriter GZipCompress(this IPipeWriter writer, PipeOptions options, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(pipe.Reader, writer);
            return pipe.Writer;
        }

        public static IPipeReader GZipCompress(this IPipeReader reader, PipeOptions options, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateDeflateDecompressReader(PipeOptions options, IPipeReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateDeflateCompressReader(PipeOptions options, IPipeReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateGZipDecompressReader(PipeOptions options, IPipeReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeWriter CreateGZipCompressWriter(PipeOptions options, IPipeWriter writer, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(pipe.Reader, writer);
            return pipe.Writer;
        }

        public static IPipeReader CreateGZipCompressReader(PipeOptions options, IPipeReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = new Pipe(options);
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        private class WritableDeflateTransform
        {
            private readonly Deflater _deflater;

            public WritableDeflateTransform(CompressionLevel compressionLevel, int bits)
            {
                _deflater = new Deflater(compressionLevel, bits);
            }

            public async Task Execute(IPipeReader reader, IPipeWriter writer)
            {
                List<MemoryHandle> handles = new List<MemoryHandle>();

                while (true)
                {
                    var result = await reader.ReadAsync();
                    var inputBuffer = result.Buffer;

                    if (inputBuffer.IsEmpty)
                    {
                        if (result.IsCompleted)
                        {
                            break;
                        }

                        reader.Advance(inputBuffer.End);
                        continue;
                    }

                    var writerBuffer = writer;
                    var buffer = inputBuffer.First;

                    unsafe
                    {
                        var handle = buffer.Retain(pin: true);
                        handles.Add(handle);
                        _deflater.SetInput((IntPtr)handle.Pointer, buffer.Length);
                    }

                    while (!_deflater.NeedsInput())
                    {
                        unsafe
                        {
                            var wbuffer = writerBuffer.GetMemory();
                            var handle = wbuffer.Retain(pin: true);
                            handles.Add(handle);
                            int written = _deflater.ReadDeflateOutput((IntPtr)handle.Pointer, wbuffer.Length);
                            writerBuffer.Advance(written);
                        }
                    }

                    var consumed = buffer.Length - _deflater.AvailableInput;

                    inputBuffer = inputBuffer.Slice(0, consumed);

                    reader.Advance(inputBuffer.End);

                    await writerBuffer.FlushAsync();
                }

                bool flushed = false;
                do
                {
                    // Need to do more stuff here
                    var writerBuffer = writer;

                    unsafe
                    {
                        var memory = writerBuffer.GetMemory();
                        var handle = memory.Retain(pin: true);
                        handles.Add(handle);
                        flushed = _deflater.Flush((IntPtr)handle.Pointer, memory.Length, out int compressedBytes);
                        writerBuffer.Advance(compressedBytes);
                    }

                    await writerBuffer.FlushAsync();
                }
                while (flushed);

                bool finished = false;
                do
                {
                    // Need to do more stuff here
                    var writerBuffer = writer;

                    unsafe
                    {
                        var memory = writerBuffer.GetMemory();
                        var handle = memory.Retain(pin: true);
                        handles.Add(handle);
                        finished = _deflater.Finish((IntPtr)handle.Pointer, memory.Length, out int compressedBytes);
                        writerBuffer.Advance(compressedBytes);
                    }

                    await writerBuffer.FlushAsync();
                }
                while (!finished);

                reader.Complete();

                writer.Complete();

                _deflater.Dispose();

                foreach (var handle in handles)
                {
                    handle.Dispose();
                }
            }
        }

        private class ReadableDeflateTransform
        {
            private readonly Inflater _inflater;

            public ReadableDeflateTransform(int bits)
            {
                _inflater = new Inflater(bits);
            }

            public async Task Execute(IPipeReader reader, IPipeWriter writer)
            {
                List<MemoryHandle> handles = new List<MemoryHandle>();

                while (true)
                {
                    var result = await reader.ReadAsync();
                    var inputBuffer = result.Buffer;

                    if (inputBuffer.IsEmpty)
                    {
                        if (result.IsCompleted)
                        {
                            break;
                        }

                        reader.Advance(inputBuffer.End);
                        continue;
                    }

                    var writerBuffer = writer;
                    var buffer = inputBuffer.First;
                    if (buffer.Length > 0)
                    {
                        unsafe
                        {
                            var handle = buffer.Retain(pin: true);
                            handles.Add(handle);
                            _inflater.SetInput((IntPtr)handle.Pointer, buffer.Length);

                            var wbuffer = writerBuffer.GetMemory();
                            handle = wbuffer.Retain(pin: true);
                            handles.Add(handle);
                            int written = _inflater.Inflate((IntPtr)handle.Pointer, wbuffer.Length);
                            writerBuffer.Advance(written);

                            var consumed = buffer.Length - _inflater.AvailableInput;

                            inputBuffer = inputBuffer.Slice(0, consumed);
                        }
                    }

                    reader.Advance(inputBuffer.End);

                    await writerBuffer.FlushAsync();
                }

                reader.Complete();

                writer.Complete();

                _inflater.Dispose();

                foreach (var handle in handles)
                {
                    handle.Dispose();
                }
            }
        }
    }
}
