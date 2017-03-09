// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Compression
{
    public static class CompressionPipelineExtensions
    {
        public static IPipeReader DeflateDecompress(this IPipeReader reader, PipeFactory factory)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader DeflateCompress(this IPipeReader reader, PipeFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader GZipDecompress(this IPipeReader reader, PipeFactory factory)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeWriter GZipCompress(this IPipeWriter writer, PipeFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = deflater.Execute(pipe.Reader, writer);
            return pipe.Writer;
        }

        public static IPipeReader GZipCompress(this IPipeReader reader, PipeFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateDeflateDecompressReader(this PipeFactory factory, IPipeReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateDeflateCompressReader(this PipeFactory factory, IPipeReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = deflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeReader CreateGZipDecompressReader(this PipeFactory factory, IPipeReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = inflater.Execute(reader, pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeWriter CreateGZipCompressWriter(this PipeFactory factory, IPipeWriter writer, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
            var ignore = deflater.Execute(pipe.Reader, writer);
            return pipe.Writer;
        }

        public static IPipeReader CreateGZipCompressReader(this PipeFactory factory, IPipeReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            var pipe = factory.Create();
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

                    var writerBuffer = writer.Alloc();
                    var memory = inputBuffer.First;

                    unsafe
                    {
                        var handle = memory.Pin();
                        handles.Add(handle);
                        _deflater.SetInput((IntPtr)handle.PinnedPointer, memory.Length);
                    }

                    while (!_deflater.NeedsInput())
                    {
                        unsafe
                        {
                            writerBuffer.Ensure();
                            var handle = writerBuffer.Memory.Pin();
                            handles.Add(handle);
                            int written = _deflater.ReadDeflateOutput((IntPtr)handle.PinnedPointer, writerBuffer.Memory.Length);
                            writerBuffer.Advance(written);
                        }
                    }

                    var consumed = memory.Length - _deflater.AvailableInput;

                    inputBuffer = inputBuffer.Slice(0, consumed);

                    reader.Advance(inputBuffer.End);

                    await writerBuffer.FlushAsync();
                }

                bool flushed = false;
                do
                {
                    // Need to do more stuff here
                    var writerBuffer = writer.Alloc();

                    unsafe
                    {
                        writerBuffer.Ensure();
                        var memory = writerBuffer.Memory;
                        var handle = memory.Pin();
                        handles.Add(handle);
                        int compressedBytes;
                        flushed = _deflater.Flush((IntPtr)handle.PinnedPointer, memory.Length, out compressedBytes);
                        writerBuffer.Advance(compressedBytes);
                    }

                    await writerBuffer.FlushAsync();
                }
                while (flushed);

                bool finished = false;
                do
                {
                    // Need to do more stuff here
                    var writerBuffer = writer.Alloc();

                    unsafe
                    {
                        writerBuffer.Ensure();
                        var memory = writerBuffer.Memory;
                        var handle = memory.Pin();
                        handles.Add(handle);
                        int compressedBytes;
                        finished = _deflater.Finish((IntPtr)handle.PinnedPointer, memory.Length, out compressedBytes);
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
                    handle.Free();
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

                    var writerBuffer = writer.Alloc();
                    var memory = inputBuffer.First;
                    if (memory.Length > 0)
                    {
                        unsafe
                        {
                            var handle = memory.Pin();
                            handles.Add(handle);
                            _inflater.SetInput((IntPtr)handle.PinnedPointer, memory.Length);

                            writerBuffer.Ensure();
                            handle = writerBuffer.Memory.Pin();
                            handles.Add(handle);
                            int written = _inflater.Inflate((IntPtr)handle.PinnedPointer, writerBuffer.Memory.Length);
                            writerBuffer.Advance(written);

                            var consumed = memory.Length - _inflater.AvailableInput;

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
                    handle.Free();
                }
            }
        }
    }
}
