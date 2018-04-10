using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines.Testing
{
    internal class BufferSegment : ReadOnlySequenceSegment<byte>
    {
        private BufferSegment _next;
        
        public int Start { get; private set; }

        public int End
        {
            get => _end;
            set
            {
                Debug.Assert(Start - value <= AvailableMemory.Length);

                _end = value;
                Memory = AvailableMemory.Slice(Start, _end - Start);
            }
        }

        /// <summary>
        /// Reference to the next block of data when the overall "active" bytes spans multiple blocks. At the point when the block is
        /// leased Next is guaranteed to be null. Start, End, and Next are used together in order to create a linked-list of discontiguous
        /// working memory. The "active" memory is grown when bytes are copied in, End is increased, and Next is assigned. The "active"
        /// memory is shrunk when bytes are consumed, Start is increased, and blocks are returned to the pool.
        /// </summary>
        public BufferSegment NextSegment
        {
            get => _next;
            set
            {
                _next = value;
                Next = value;
            }
        }

        /// <summary>
        /// The buffer being tracked if segment owns the memory
        /// </summary>
        private IMemoryOwner<byte> _ownedMemory;

        private int _end;

        public void SetMemory(IMemoryOwner<byte> buffer)
        {
            SetMemory(buffer, 0, 0);
        }

        public void SetMemory(IMemoryOwner<byte> ownedMemory, int start, int end, bool readOnly = false)
        {
            _ownedMemory = ownedMemory;

            AvailableMemory = _ownedMemory.Memory;

            ReadOnly = readOnly;
            RunningIndex = 0;
            Start = start;
            End = end;
            NextSegment = null;
        }

        public void ResetMemory()
        {
            _ownedMemory.Dispose();
            _ownedMemory = null;
            AvailableMemory = default;
        }

        public Memory<byte> AvailableMemory { get; private set; }

        public int Length => End - Start;

        /// <summary>
        /// If true, data should not be written into the backing block after the End offset. Data between start and end should never be modified
        /// since this would break cloning.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// The amount of writable bytes in this segment. It is the amount of bytes between Length and End
        /// </summary>
        public int WritableBytes => AvailableMemory.Length - End;

        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Memory.IsEmpty)
            {
                return "<EMPTY>";
            }

            var builder = new StringBuilder();
            SpanLiteralExtensions.AppendAsLiteral(Memory.Span, builder);
            return builder.ToString();
        }

        public void SetNext(BufferSegment segment)
        {
            Debug.Assert(segment != null);
            Debug.Assert(Next == null);

            NextSegment = segment;

            segment = this;

            while (segment.Next != null)
            {
                segment.NextSegment.RunningIndex = segment.RunningIndex + segment.Length;
                segment = segment.NextSegment;
            }
        }
    }
}
