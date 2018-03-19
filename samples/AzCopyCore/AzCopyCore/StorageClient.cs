// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Azure.Authentication;
using System.Azure.Storage.Requests;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Buffers.Text;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Experimental;
using System.Text;
using System.Text.Http.Parser;
using System.Threading.Tasks;

namespace System.Azure.Storage
{
    public class StorageClient : IDisposable
    {
        PipeHttpClient _client;
        SocketPipe _pipe;
        Sha256 _hash;
        string _host;
        int _port;
        string _accountName;

        public StorageClient(ReadOnlySpan<char> masterKey, ReadOnlySpan<char> accountName, ReadOnlySpan<char> host, int port = 80, TraceSource log = null)
        {
            _host = new string(host);
            _accountName = new string(accountName);
            _port = port;
            byte[] keyBytes = Key.ComputeKeyBytes(masterKey);
            _hash = Sha256.Create(keyBytes);
            Log = log;
        }

        public StorageClient(byte[] keyBytes, ReadOnlySpan<char> accountName, ReadOnlySpan<char> host, int port = 80, TraceSource log = null)
        {
            _host = new string(host);
            _accountName = new string(accountName);
            _port = port;
            _hash = Sha256.Create(keyBytes);
            Log = log;
        }

        public TraceSource Log { get; }

        public string Host => _host;

        public string AccountName => _accountName;

        internal Sha256 Hash => _hash;

        public async ValueTask<StorageResponse> SendRequest<TRequest>(TRequest request)
            where TRequest : IStorageRequest
        {
            if (!_client.IsConnected)
            {
                _pipe = await SocketPipe.ConnectAsync(_host, _port).ConfigureAwait(false);
                _pipe.Log = Log;
                _client = new PipeHttpClient(_pipe);
            }
            request.Client = this;

            StorageResponse response = await _client.SendRequest<TRequest, StorageResponse>(request).ConfigureAwait(false);
            if (request.ConsumeBody) await ConsumeResponseBody(response.Body);
            return response;
        }

        // for some reason some responses contain a body, despite the fact that the MSDN docs say there is no body, so
        // I need to skip the body without understanding what it is (it's "0\n\r\n\r", BTW)
        static async Task ConsumeResponseBody(PipeReader reader)
        {
            ReadResult body = await reader.ReadAsync();
            ReadOnlySequence<byte> bodyBuffer = body.Buffer;
            reader.AdvanceTo(bodyBuffer.End);
        }

        public void Dispose()
        {
            _pipe.Dispose();
            _hash.Dispose();
        }
    }

    public struct StorageResponse : IHttpResponseHandler
    {
        static byte[] s_contentLength = Encoding.UTF8.GetBytes("Content-Length");

        ulong _contentLength;
        public ulong ContentLength => _contentLength;
        public ushort StatusCode { get; private set; }
        public PipeReader Body { get; private set; }

        public void OnStatusLine(Http.Version version, ushort statusCode, ReadOnlySpan<byte> status)
        {
            StatusCode = statusCode;
        }

        public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
        {
            if (name.SequenceEqual(s_contentLength))
            {
                if (!Utf8Parser.TryParse(value, out _contentLength, out _))
                {
                    throw new Exception("invalid header");
                }
            }
        }

        public ValueTask OnBody(PipeReader body)
        {
            Body = body;
            return default;
        }
    }
}

