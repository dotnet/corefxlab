// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO.Compression;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Compression
{
    public static class CompressionPipelineExtensions
    {
        public static IPipelineReader DeflateDecompress(this IPipelineReader reader, PipelineFactory factory)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            return factory.CreateReader(reader, inflater.Execute);
        }

        public static IPipelineReader DeflateCompress(this IPipelineReader reader, PipelineFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            return factory.CreateReader(reader, deflater.Execute);
        }

        public static IPipelineReader GZipDecompress(this IPipelineReader reader, PipelineFactory factory)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateReader(reader, inflater.Execute);
        }

        public static IPipelineWriter GZipCompress(this IPipelineWriter writer, PipelineFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateWriter(writer, deflater.Execute);
        }

        public static IPipelineReader GZipCompress(this IPipelineReader reader, PipelineFactory factory, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateReader(reader, deflater.Execute);
        }

        public static IPipelineReader CreateDeflateDecompressReader(this PipelineFactory factory, IPipelineReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.Deflate_DefaultWindowBits);
            return factory.CreateReader(reader, inflater.Execute);
        }

        public static IPipelineReader CreateDeflateCompressReader(this PipelineFactory factory, IPipelineReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.Deflate_DefaultWindowBits);
            return factory.CreateReader(reader, deflater.Execute);
        }

        public static IPipelineReader CreateGZipDecompressReader(this PipelineFactory factory, IPipelineReader reader)
        {
            var inflater = new ReadableDeflateTransform(ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateReader(reader, inflater.Execute);
        }

        public static IPipelineWriter CreateGZipCompressWriter(this PipelineFactory factory, IPipelineWriter writer, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateWriter(writer, deflater.Execute);
        }

        public static IPipelineReader CreateGZipCompressReader(this PipelineFactory factory, IPipelineReader reader, CompressionLevel compressionLevel)
        {
            var deflater = new WritableDeflateTransform(compressionLevel, ZLibNative.GZip_DefaultWindowBits);
            return factory.CreateReader(reader, deflater.Execute);
        }

        private class WritableDeflateTransform
        {
            private readonly Deflater _deflater;

            public WritableDeflateTransform(CompressionLevel compressionLevel, int bits)
            {
                _deflater = new Deflater(compressionLevel, bits);
            }

            public async Task Execute(IPipelineReader reader, IPipelineWriter writer)
            {
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
                        // TODO: Pin pointer if not pinned
                        void* inPointer;
                        if (memory.TryGetPointer(out inPointer))
                        {
                            _deflater.SetInput((IntPtr)inPointer, memory.Length);
                        }
                        else
                        {
                            throw new InvalidOperationException("Pointer needs to be pinned");
                        }
                    }

                    while (!_deflater.NeedsInput())
                    {
                        unsafe
                        {
                            void* outPointer;
                            writerBuffer.Ensure();
                            if (writerBuffer.Memory.TryGetPointer(out outPointer))
                            {
                                int written = _deflater.ReadDeflateOutput((IntPtr)outPointer, writerBuffer.Memory.Length);
                                writerBuffer.Advance(written);
                            }
                            else
                            {
                                throw new InvalidOperationException("Pointer needs to be pinned");
                            }
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
                        void* pointer;
                        writerBuffer.Ensure();
                        var memory = writerBuffer.Memory;
                        if (memory.TryGetPointer(out pointer))
                        {
                            int compressedBytes;
                            flushed = _deflater.Flush((IntPtr)pointer, memory.Length, out compressedBytes);
                            writerBuffer.Advance(compressedBytes);
                        }
                        else
                        {
                            throw new InvalidOperationException("Pointer needs to be pinned");
                        }
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
                        void* pointer;
                        writerBuffer.Ensure();
                        var memory = writerBuffer.Memory;
                        if (memory.TryGetPointer(out pointer))
                        {
                            int compressedBytes;
                            finished = _deflater.Finish((IntPtr)pointer, memory.Length, out compressedBytes);
                            writerBuffer.Advance(compressedBytes);
                        }
                    }

                    await writerBuffer.FlushAsync();
                }
                while (!finished);

                reader.Complete();

                writer.Complete();

                _deflater.Dispose();
            }
        }

        private class ReadableDeflateTransform
        {
            private readonly Inflater _inflater;

            public ReadableDeflateTransform(int bits)
            {
                _inflater = new Inflater(bits);
            }

            public async Task Execute(IPipelineReader reader, IPipelineWriter writer)
            {
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
                            void* pointer;
                            if (memory.TryGetPointer(out pointer))
                            {
                                _inflater.SetInput((IntPtr)pointer, memory.Length);

                                void* writePointer;
                                writerBuffer.Ensure();
                                if (writerBuffer.Memory.TryGetPointer(out writePointer))
                                {
                                    int written = _inflater.Inflate((IntPtr)writePointer, writerBuffer.Memory.Length);
                                    writerBuffer.Advance(written);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Pointer needs to be pinned");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Pointer needs to be pinned");
                            }

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
            }
        }
    }
}
