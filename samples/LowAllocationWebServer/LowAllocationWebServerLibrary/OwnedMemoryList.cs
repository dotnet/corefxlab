using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Text;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;

namespace Microsoft.Net.Http
{
    class OwnedLinkableBuffer : OwnedMemory<byte>, IMemoryList<byte>
    {
        internal OwnedLinkableBuffer _next;
        int _written;

        public OwnedLinkableBuffer(int desiredSize = 1024) : base(ArrayPool<byte>.Shared.Rent(desiredSize))
        { }

        public Memory<byte> First => Memory;

        public Memory<byte> Free => Memory.Slice(_written);

        public IMemoryList<byte> Rest => _next;

        int? ISequence<Memory<byte>>.Length => null;

        public int CopyTo(Span<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            throw new NotImplementedException();
        }

        public void Enlarge(int desiredSize)
        {
            _next = new OwnedLinkableBuffer(desiredSize);
        }

        protected override void Dispose(bool disposing)
        {
            var array = Array;
            base.Dispose(disposing);
            ArrayPool<byte>.Shared.Return(array);
            _next.Dispose();
        }

        internal void Advance(int bytes)
        {
            _written += bytes;
        }
    }
}
