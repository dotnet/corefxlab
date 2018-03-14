// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Utf8;
using System.Threading;

public class BufferWriterBench
{
    private static AsciiString _crlf = "\r\n";
    private static AsciiString _eoh = "\r\n\r\n"; // End Of Headers
    private static AsciiString _http11OK = "HTTP/1.1 200 OK\r\n";
    private static AsciiString _headerServer = "Server: Custom";
    private static AsciiString _headerContentLength = "Content-Length: ";
    private static AsciiString _headerContentLengthZero = "Content-Length: 0\r\n";
    private static AsciiString _headerContentTypeText = "Content-Type: text/plain\r\n";
    private static AsciiString _plainTextBody = "Hello, World!";

    private static Utf8String _crlfU8 = (Utf8String)"\r\n";
    private static Utf8String _eohU8 = (Utf8String)"\r\n\r\n"; // End Of Headers
    private static Utf8String _http11OKU8 = (Utf8String)"HTTP/1.1 200 OK\r\n";
    private static Utf8String _headerServerU8 = (Utf8String)"Server: Custom";
    private static Utf8String _headerContentLengthU8 = (Utf8String)"Content-Length: ";
    private static Utf8String _headerContentLengthZeroU8 = (Utf8String)"Content-Length: 0\r\n";
    private static Utf8String _headerContentTypeTextU8 = (Utf8String)"Content-Type: text/plain\r\n";
    private static Utf8String _plainTextBodyU8 = (Utf8String)"Hello, World!";

    static Sink _sink = new Sink(4096);

    const int InnerItterations = 1000000;

    [Benchmark(InnerIterationCount = InnerItterations)]
    static void PlatfromBenchmarkPlaintext()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < InnerItterations; i++)
                {
                    _sink.Reset();
                    var writer = new PlatfromBenchmark.BufferWriter<Sink>(_sink);

                    // HTTP 1.1 OK
                    writer.Write(_http11OK);

                    // Server headers
                    writer.Write(_headerServer);

                    // Date header
                    writer.Write(DateHeader.HeaderBytes);

                    // Content-Type header
                    writer.Write(_headerContentTypeText);

                    // Content-Length header
                    writer.Write(_headerContentLength);
                    writer.Write((ulong)_plainTextBody.Length);

                    // End of headers
                    writer.Write(_eoh);

                    // Body
                    writer.Write(_plainTextBody);
                    writer.Commit();
                }
            }
        }
    }

    [Benchmark(InnerIterationCount = InnerItterations)]
    static void BufferWriterPlaintext()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < InnerItterations; i++)
                {
                    _sink.Reset();
                    var writer = BufferWriter.Create(_sink);

                    // HTTP 1.1 OK
                    writer.Write(_http11OK);

                    // Server headers
                    writer.Write(_headerServer);

                    // Date header
                    writer.Write(DateHeader.HeaderBytes);

                    // Content-Type header
                    writer.Write(_headerContentTypeText);

                    // Content-Length header
                    writer.Write(_headerContentLength);
                    writer.Write((ulong)_plainTextBody.Length);

                    // End of headers
                    writer.Write(_eoh);

                    // Body
                    writer.Write(_plainTextBody);
                    writer.Flush();
                }
            }
        }
    }

    [Benchmark(InnerIterationCount = InnerItterations)]
    static void BufferWriterPlaintextUtf8()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < InnerItterations; i++)
                {
                    _sink.Reset();
                    var writer = BufferWriter.Create(_sink);

                    // HTTP 1.1 OK
                    writer.Write(_http11OKU8);

                    // Server headers
                    writer.Write(_headerServerU8);

                    // Date header
                    writer.Write(DateHeader.HeaderBytes);

                    // Content-Type header
                    writer.Write(_headerContentTypeTextU8);

                    // Content-Length header
                    writer.Write(_headerContentLengthU8);
                    writer.Write((ulong)_plainTextBody.Length);

                    // End of headers
                    writer.Write(_eohU8);

                    // Body
                    writer.Write(_plainTextBodyU8);
                    writer.Flush();
                }
            }
        }
    }

    [Benchmark(InnerIterationCount = InnerItterations)]
    static void BufferWriterCopyPlaintext()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < InnerItterations; i++)
                {
                    _sink.Reset();
                    var writer = new SystemBuffers.BufferWriter<Sink>(_sink);

                    // HTTP 1.1 OK
                    writer.Write(_http11OK);

                    // Server headers
                    writer.Write(_headerServer);

                    // Date header
                    writer.Write(DateHeader.HeaderBytes);

                    // Content-Type header
                    writer.Write(_headerContentTypeText);

                    // Content-Length header
                    writer.Write(_headerContentLength);
                    writer.Write((ulong)_plainTextBody.Length);

                    // End of headers
                    writer.Write(_eoh);

                    // Body
                    writer.Write(_plainTextBody);
                    writer.Flush();
                }
            }
        }
    }
}

class Sink : IBufferWriter<byte>
{
    byte[] _buffer;
    int _written;

    public Sink(int size)
    {
        _buffer = new byte[4096];
        _written = 0;
    }
    public void Reset() => _written = 0;

    public void Advance(int count)
    {
        _written += count;
        if (_written > _buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
        => _buffer.AsMemory(_written, _buffer.Length - _written);

    public Span<byte> GetSpan(int sizeHint = 0)
        => _buffer.AsSpan(_written, _buffer.Length - _written);
}

readonly struct AsciiString : IEquatable<AsciiString>
{
    private readonly byte[] _data;

    public AsciiString(string s) => _data = Encoding.ASCII.GetBytes(s);

    public int Length => _data.Length;

    public ReadOnlySpan<byte> AsSpan() => _data;

    public static implicit operator ReadOnlySpan<byte>(AsciiString str) => str._data;
    public static implicit operator byte[] (AsciiString str) => str._data;

    public static implicit operator AsciiString(string str) => new AsciiString(str);

    public override string ToString() => Encoding.ASCII.GetString(_data);

    public static explicit operator string(AsciiString str) => str.ToString();

    public bool Equals(AsciiString other) => ReferenceEquals(_data, other._data) || SequenceEqual(_data, other._data);
    private bool SequenceEqual(byte[] data1, byte[] data2) => new Span<byte>(data1).SequenceEqual(data2);

    public static bool operator ==(AsciiString a, AsciiString b) => a.Equals(b);
    public static bool operator !=(AsciiString a, AsciiString b) => !a.Equals(b);
    public override bool Equals(object other) => (other is AsciiString) && Equals((AsciiString)other);

    public override int GetHashCode()
    {
        // Copied from x64 version of string.GetLegacyNonRandomizedHashCode()
        // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/String.Comparison.cs
        var data = _data;
        int hash1 = 5381;
        int hash2 = hash1;
        foreach (int b in data)
        {
            hash1 = ((hash1 << 5) + hash1) ^ b;
        }
        return hash1 + (hash2 * 1566083941);
    }
}

static class DateHeader
{
    const int prefixLength = 8; // "\r\nDate: ".Length
    const int dateTimeRLength = 29; // Wed, 14 Mar 2018 14:20:00 GMT
    const int suffixLength = 2; // crlf
    const int suffixIndex = dateTimeRLength + prefixLength;

    private static byte[] s_headerBytesMaster = new byte[prefixLength + dateTimeRLength + suffixLength];
    private static byte[] s_headerBytesScratch = new byte[prefixLength + dateTimeRLength + suffixLength];

    static DateHeader()
    {
        var utf8 = Encoding.ASCII.GetBytes("\r\nDate: ").AsSpan();
        utf8.CopyTo(s_headerBytesMaster);
        utf8.CopyTo(s_headerBytesScratch);
        s_headerBytesMaster[suffixIndex] = (byte)'\r';
        s_headerBytesMaster[suffixIndex + 1] = (byte)'\n';
        s_headerBytesScratch[suffixIndex] = (byte)'\r';
        s_headerBytesScratch[suffixIndex + 1] = (byte)'\n';
        SetDateValues(DateTimeOffset.UtcNow);
    }

    public static ReadOnlySpan<byte> HeaderBytes => s_headerBytesMaster;

    public static void SetDateValues(DateTimeOffset value)
    {
        lock (s_headerBytesScratch)
        {
            if (!Utf8Formatter.TryFormat(value, s_headerBytesScratch.AsSpan().Slice(prefixLength), out int written, 'R'))
            {
                throw new Exception("date time format failed");
            }
            Debug.Assert(written == dateTimeRLength);
            var temp = s_headerBytesMaster;
            s_headerBytesMaster = s_headerBytesScratch;
            s_headerBytesScratch = temp;
        }
    }
}

// copy from https://github.com/aspnet/benchmarks/tree/dev/src/PlatformBenchmarks
namespace PlatfromBenchmark
{
    internal ref struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        private T _output;
        private Span<byte> _span;
        private int _buffered;

        public BufferWriter(T output)
        {
            _buffered = 0;
            _output = output;
            _span = output.GetSpan();
        }

        public Span<byte> Span => _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Commit()
        {
            var buffered = _buffered;
            if (buffered > 0)
            {
                _buffered = 0;
                _output.Advance(buffered);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _buffered += count;
            _span = _span.Slice(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ReadOnlySpan<byte> source)
        {
            if (_span.Length >= source.Length)
            {
                source.CopyTo(_span);
                Advance(source.Length);
            }
            else
            {
                WriteMultiBuffer(source);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ensure(int count = 1)
        {
            if (_span.Length < count)
            {
                EnsureMore(count);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EnsureMore(int count = 0)
        {
            if (_buffered > 0)
            {
                Commit();
            }

            _output.GetMemory(count);
            _span = _output.GetSpan();
        }

        private void WriteMultiBuffer(ReadOnlySpan<byte> source)
        {
            while (source.Length > 0)
            {
                if (_span.Length == 0)
                {
                    EnsureMore();
                }

                var writable = Math.Min(source.Length, _span.Length);
                source.Slice(0, writable).CopyTo(_span);
                source = source.Slice(writable);
                Advance(writable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong number)
        {
            // Try to format directly
            if (Utf8Formatter.TryFormat(number, Span, out int bytesWritten))
            {
                Advance(bytesWritten);
            }
            else
            {
                // Ask for at least 20 bytes
                Ensure(20);

                Debug.Assert(Span.Length >= 20, "Buffer is < 20 bytes");

                // Try again
                if (Utf8Formatter.TryFormat(number, Span, out bytesWritten))
                {
                    Advance(bytesWritten);
                }
            }
        }
    }
}

// copy from System.Buffers.ReaderWriter to isolate cross-dll calls.
namespace SystemBuffers
{
    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        private T _output;
        private Span<byte> _span;
        private int _buffered;

        public BufferWriter(T output)
        {
            _buffered = 0;
            _output = output;
            _span = output.GetSpan();
        }

        public Span<byte> Buffer => _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            var buffered = _buffered;
            if (buffered > 0)
            {
                _buffered = 0;
                _output.Advance(buffered);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _buffered += count;
            _span = _span.Slice(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ensure(int count = 1)
        {
            if (_span.Length < count)
            {
                EnsureMore(count);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EnsureMore(int count = 0)
        {
            var buffered = _buffered;
            if (buffered > 0)
            {
                _buffered = 0;
                _output.Advance(buffered);
            }
            _span = _output.GetSpan(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Enlarge()
        {
            EnsureMore(_span.Length + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ReadOnlySpan<byte> source)
        {
            if (_span.Length >= source.Length)
            {
                source.CopyTo(_span);
                Advance(source.Length);
            }
            else
            {
                WriteMultiBuffer(source);
            }
        }

        private void WriteMultiBuffer(ReadOnlySpan<byte> source)
        {
            while (source.Length > 0)
            {
                if (_span.Length == 0)
                {
                    EnsureMore();
                }

                var writable = Math.Min(source.Length, _span.Length);
                source.Slice(0, writable).CopyTo(_span);
                source = source.Slice(writable);
                Advance(writable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value, StandardFormat format = default)
        {
            int written;
            if (Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Advance(written);
            }
            else
            {
                Enlarge();
                while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
                {
                    Enlarge();
                }
                Advance(written);
            }
        }
    }
}
