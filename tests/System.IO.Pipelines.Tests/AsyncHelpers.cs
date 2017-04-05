using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Tests
{
    static class AwaiterExtensions
    {
        public static FlushResult GetResultSync(this WritableBufferAwaitable awaitable)
        {
            if (!awaitable.IsCompleted)
            {
                ManualResetEvent complete = new ManualResetEvent(false);
                awaitable.OnCompleted(() => complete.Set());
                Task.Run(() => complete.WaitOne()).Wait();
            }

            return awaitable.GetResult();
        }

        public static T GetResultSync<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
