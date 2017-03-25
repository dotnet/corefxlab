// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    internal class TaskUtilities
    {
        public static readonly Task<int> ZeroTask = Task.FromResult(0);
#if NETSTANDARD1_3
        public static readonly Task CompletedTask = Task.CompletedTask;
        public static Task FromException(Exception exception)
                        => Task.FromException(exception);
#else
        public static readonly Task CompletedTask = ZeroTask;
        public static Task FromException(Exception exception)
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(exception);
            return tcs.Task;
        }
#endif
    }
}
