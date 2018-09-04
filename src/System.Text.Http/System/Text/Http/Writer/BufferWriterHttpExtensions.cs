// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Writer;

namespace System.Text.Http.Formatter
{
    public static class BufferWriterHttpExtensions
    {
        public static readonly byte[] s_httpNewline = new byte[] { 13, 10 };
        public static readonly byte[] s_eoh = new byte[] { 13, 10, 13, 10 };
        static byte[] s_Get = new byte[] { (byte)'G', (byte)'E', (byte)'T' };
        static byte[] s_Put = new byte[] { (byte)'P', (byte)'U', (byte)'T' };

        public static BufferWriter AsHttpWriter(this Span<byte> buffer)
        {
            var writer = BufferWriter.Create(buffer);
            writer.NewLine = s_httpNewline;
            return writer;
        }

        public static void WriteRequestLine(ref this BufferWriter writer, Parser.Http.Method verb, Parser.Http.Version version, string path)
        {
            writer.WriteBytes(verb.AsBytes());
            writer.Write(" /");

            writer.Write(path);
            if (version == Parser.Http.Version.Http11)
            {
                writer.WriteLine(" HTTP/1.1");
            }
            else if (version == Parser.Http.Version.Http10)
            {
                writer.WriteLine(" HTTP/1.0");
            }
            else throw new NotSupportedException("version not supported");
        }

        public static void WriteHeader(ref this BufferWriter writer, string headerName, string headerValue)
        {
            writer.Write(headerName);
            writer.Write(":");
            writer.WriteLine(headerValue);
        }

        public static void WriteHeader(ref this BufferWriter writer, string headerName, int headerValue)
        {
            writer.Write(headerName);
            writer.Write(":");
            // TODO (Pri 0): this allocation needs to be eliminated
            writer.WriteLine(headerValue);
        }

        public static void WriteHeader(ref this BufferWriter writer, string headerName, long headerValue)
        {
            writer.Write(headerName);
            writer.Write(":");

            writer.WriteLine(headerValue);
        }

        public static void WriteHeader(ref this BufferWriter writer, string headerName, DateTime headerValue, StandardFormat format)
        {
            writer.Write(headerName);
            writer.Write(":");
            writer.WriteLine(headerValue, format);
        }

        public static void WriteHeader<T>(ref this BufferWriter writer, string headerName, T headerValue, StandardFormat format)
            where T : IWritable
        {
            writer.Write(headerName);
            writer.Write(":");
            writer.WriteBytes(headerValue, format);
            writer.WriteLine("");
        }

        public static void WriteEoh(ref this BufferWriter writer)
        {
            writer.WriteLine("");
        }

        static ReadOnlySpan<byte> AsBytes(this Parser.Http.Method verb)
        {
            if (verb == Parser.Http.Method.Get) return s_Get;
            if (verb == Parser.Http.Method.Put) return s_Put;
            throw new NotImplementedException();
        }
    }
}
