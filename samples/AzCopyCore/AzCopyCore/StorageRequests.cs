// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Azure.Authentication;
using System.Buffers.Writer;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Text.Http.Formatter;
using System.Text.Http.Parser;
using System.Threading.Tasks;

namespace System.Azure.Storage.Requests
{
    public interface IStorageRequest : IPipeWritable
    {
        StorageClient Client { get; set; }
        string RequestPath { get; }
        string CanonicalizedResource { get; }
        long ContentLength { get; }
        bool ConsumeBody { get; }

    }

    public abstract class RequestWriter<T> where T : IPipeWritable
    {
        public abstract Text.Http.Parser.Http.Method Verb { get; }

        public async Task WriteAsync(PipeWriter writer, T request)
        {
            WriteRequestLineAndHeaders(writer, ref request);
            await WriteBody(writer, request).ConfigureAwait(false);
            await writer.FlushAsync();
        }

        // TODO (pri 2): writing the request line should not be abstract; writing headers should.
        protected abstract void WriteRequestLineAndHeaders(PipeWriter writer, ref T request);
        protected virtual Task WriteBody(PipeWriter writer, T request) { return Task.CompletedTask; }
    }

    // This is a helper class for impementing writers for various Storage requests.
    // Subclasses of this type (see below) know how to write specific headers for each Storage Rest API.
    abstract class StorageRequestWriter<T> : RequestWriter<T> where T : IStorageRequest
    {
        // TODO (pri 3): this should be cached with some expiration policy
        protected DateTime Time => DateTime.UtcNow;

        // TODO (pri 2): it would be good if this could advance and flush instead demanding larger and larger buffers.
        protected override void WriteRequestLineAndHeaders(PipeWriter writer, ref T arguments)
        {
            Span<byte> memory = writer.GetSpan();
            BufferWriter bufferWriter = memory.AsHttpWriter();
            bufferWriter.Enlarge = (int desiredSize) =>
            {
                return writer.GetMemory(desiredSize);
            };

            bufferWriter.WriteRequestLine(Verb, Http.Version.Http11, arguments.RequestPath);

            int headersStart = bufferWriter.WrittenCount;
            WriteXmsHeaders(ref bufferWriter, ref arguments);
            Span<byte> headersBuffer = bufferWriter.Written.Slice(headersStart);

            var authenticationHeader = new StorageAuthorizationHeader()
            {
                // TODO (pri 1): the hash is not thread safe. is that OK?
                Hash = arguments.Client.Hash,
                HttpVerb = AsString(Verb),
                AccountName = arguments.Client.AccountName,
                CanonicalizedResource = arguments.CanonicalizedResource,
                // TODO (pri 1): this allocation should be eliminated
                CanonicalizedHeaders = headersBuffer.ToArray(),
                ContentLength = arguments.ContentLength
            };
            // TODO (pri 3): the default should be defaulted
            bufferWriter.WriteHeader("Authorization", authenticationHeader, default);

            WriteOtherHeaders(ref bufferWriter, ref arguments);
            bufferWriter.WriteEoh();

            writer.Advance(bufferWriter.WrittenCount);
        }

        protected abstract void WriteXmsHeaders(ref BufferWriter writer, ref T arguments);

        protected virtual void WriteOtherHeaders(ref BufferWriter writer, ref T arguments)
        {
            writer.WriteHeader("Content-Length", arguments.ContentLength);
            writer.WriteHeader("Host", arguments.Client.Host);
        }

        static readonly byte[] s_GETu8 = Encoding.ASCII.GetBytes("GET");
        static readonly byte[] s_PUTu8 = Encoding.ASCII.GetBytes("PUT");
        public static ReadOnlyMemory<byte> AsString(Http.Method verb)
        {
            if (verb == Http.Method.Get) return s_GETu8;
            if (verb == Http.Method.Put) return s_PUTu8;
            throw new NotImplementedException();
        }
    }

    public struct PutRangeRequest : IStorageRequest
    {
        Stream _fileContent; // TODO (pri 3): should there be a way to write from file handle or PipeReader?
        string _filePath;
        long _offset;
        int _length;

        // TODO (pri 3): I dont like how the client property is a public API
        public StorageClient Client { get; set; }

        public PutRangeRequest(string filePath, Stream fileContent, long offset, int length)
        {
            if (offset < 0 || offset > fileContent.Length - length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length)); 

            _filePath = filePath;
            _fileContent = fileContent;
            Client = null;
            _offset = offset;
            _length = length;
        }

        public long ContentLength => _length;
        // TODO (pri 2): would be nice to elimnate these allocations
        public string RequestPath => _filePath + "?comp=range";
        public string CanonicalizedResource => _filePath + "\ncomp:range";
        public bool ConsumeBody => true;

        // TODO (pri 3): can this be an extension method? All implementations are the same.
        public async Task WriteAsync(PipeWriter writer)
            => await requestWriter.WriteAsync(writer, this).ConfigureAwait(false);

        static readonly Writer requestWriter = new Writer();
        class Writer : StorageRequestWriter<PutRangeRequest>
        {
            public override Http.Method Verb => Http.Method.Put;

            protected override async Task WriteBody(PipeWriter writer, PutRangeRequest arguments)
            {
                Stream stream = arguments._fileContent;
                stream.Seek(arguments._offset, SeekOrigin.Begin);
                await writer.WriteAsync(stream, arguments._length);
            }

            protected override void WriteXmsHeaders(ref BufferWriter writer, ref PutRangeRequest arguments)
            {
                long size = arguments._fileContent.Length;
                writer.WriteHeader("x-ms-date", Time, 'R');
                // TODO (pri 3): this allocation should be eliminated

                long start = arguments._offset;
                long end = start + arguments._length - 1;
                writer.WriteHeader("x-ms-range", $"bytes={start}-{end}");
                writer.WriteHeader("x-ms-version", "2017-04-17");
                writer.WriteHeader("x-ms-write", "update");
            }
        }
    }

    public struct CreateFileRequest : IStorageRequest
    {
        public long FileSize { get; set; }
        public string RequestPath { get; set; }
        public StorageClient Client { get; set; }

        public CreateFileRequest(string requestPath, long fileSize)
        {
            RequestPath = requestPath;
            FileSize = fileSize;
            Client = null;
        }

        public bool ConsumeBody => true;
        public long ContentLength => 0;
        public string CanonicalizedResource => RequestPath;

        public async Task WriteAsync(PipeWriter writer)
            => await requestWriter.WriteAsync(writer, this).ConfigureAwait(false);

        static readonly Writer requestWriter = new Writer();
        class Writer : StorageRequestWriter<CreateFileRequest>
        {
            public override Http.Method Verb => Http.Method.Put;

            protected override void WriteXmsHeaders(ref BufferWriter writer, ref CreateFileRequest arguments)
            {
                writer.WriteHeader("x-ms-content-length", arguments.FileSize);
                writer.WriteHeader("x-ms-date", Time, 'R');
                writer.WriteHeader("x-ms-type", "file");
                writer.WriteHeader("x-ms-version", "2017-04-17");
            }
        }
    }

    public struct GetFileRequest : IStorageRequest
    {
        public StorageClient Client { get; set; }
        public string FilePath { get; set; }

        public GetFileRequest(string filePath)
        {
            FilePath = filePath;
            Client = null;
        }

        public long ContentLength => 0;
        public string RequestPath => FilePath;
        public string CanonicalizedResource => FilePath;
        public bool ConsumeBody => false;

        public async Task WriteAsync(PipeWriter writer)
            => await requestWriter.WriteAsync(writer, this).ConfigureAwait(false);

        static readonly Writer requestWriter = new Writer();

        class Writer : StorageRequestWriter<GetFileRequest>
        {
            public override Http.Method Verb => Http.Method.Get;

            protected override void WriteXmsHeaders(ref BufferWriter writer, ref GetFileRequest arguments)
            {
                writer.WriteHeader("x-ms-date", Time, 'R');
                writer.WriteHeader("x-ms-version", "2017-04-17");
            }
        }
    }
}

