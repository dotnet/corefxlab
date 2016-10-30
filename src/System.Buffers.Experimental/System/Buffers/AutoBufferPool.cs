using System.Collections.Concurrent;

namespace System.Buffers
{
    public class AutoBufferPool
    {
        private readonly static AutoBufferPool s_shared = new AutoBufferPool();
        public static AutoBufferPool Shared => s_shared;

        private ConcurrentQueue<AutoPooledMemory> _pool = new ConcurrentQueue<AutoPooledMemory>();

        public OwnedMemory<byte> Rent(int minimumBufferSize)
        {
            var array = ArrayPool<byte>.Shared.Rent(minimumBufferSize);

            AutoPooledMemory memory;
            if (!_pool.TryDequeue(out memory))
            {
                memory = new AutoPooledMemory(array, this);
            }
            else
            {
                memory.Initalize(array);
            }

            return memory;
        }

        private void Return(AutoPooledMemory memory)
        {
            _pool.Enqueue(memory);
        }

        private class AutoPooledMemory : OwnedMemory<byte>
        {
            private AutoBufferPool _pool;

            public AutoPooledMemory(byte[] array, AutoBufferPool pool) : base(array, 0, array.Length)
            {
                _pool = pool;
            }

            public void Initalize(byte[] array)
            {
                base.Initialize(array, 0, array.Length);
            }

            protected override void Dispose(bool disposing)
            {
                // Return Array to pool before clear
                ArrayPool<byte>.Shared.Return(Array);
                base.Dispose(disposing);
            }

            protected override void DisposeComplete()
            {
                // Disposal complete, no init/clear race - return AutoPooledMemory to pool
                _pool.Return(this);
            }
        }
    }
}
