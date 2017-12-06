using System.IO.Pipelines.Testing;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace System.IO.Pipelines.Performance.Tests
{
    [Config(typeof(CoreConfig))]
    public class Enumerators
    {
        private const int InnerLoopCount = 512;

        private ReadableBuffer _readableBuffer;

        [GlobalSetup]
        public void Setup()
        {
            _readableBuffer = BufferUtilities.CreateBuffer(Enumerable.Range(100, 200).ToArray());
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void MemoryEnumerator()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                var enumerator = new BufferEnumerator(_readableBuffer.Start, _readableBuffer.End);
                while (enumerator.MoveNext())
                {
                    var memory = enumerator.Current;
                }
            }
        }
    }
}
