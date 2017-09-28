﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading;
using System.IO.Pipelines.Networking.Libuv.Interop;
using System.IO.Pipelines.Networking.Libuv.Internal;

namespace System.IO.Pipelines.Networking.Libuv
{
    // This class needs a bunch of work to make sure it's thread safe
    public class UvThread : IDisposable, IScheduler
    {
        private readonly Thread _thread = new Thread(OnStart)
        {
            Name = "Libuv event loop"
        };
        private readonly ManualResetEventSlim _running = new ManualResetEventSlim();
        private readonly WorkQueue<Work> _workQueue = new WorkQueue<Work>();

        private bool _stopping;
        private UvAsyncHandle _postHandle;

        public UvThread()
        {
            WriteReqPool = new WriteReqPool(this);
        }

        public Uv Uv { get; private set; }

        public UvLoopHandle Loop { get; private set; }

        public MemoryPool Pool { get; } = new MemoryPool();

        public WriteReqPool WriteReqPool { get; }

        public void Post(Action<object> callback, object state)
        {
            if (_stopping)
            {
                return;
            }

            EnsureStarted();

            var work = new Work
            {
                Callback = callback,
                State = state
            };

            _workQueue.Add(work);

            _postHandle.Send();
        }

        public void GetResult()
        {

        }

        private static void OnStart(object state)
        {
            ((UvThread)state).RunLoop();
        }

        private void RunLoop()
        {
            Uv = new Uv();

            Loop = new UvLoopHandle();
            Loop.Init(Uv);

            _postHandle = new UvAsyncHandle();
            _postHandle.Init(Loop, OnPost, null);

            _running.Set();

            Uv.run(Loop, 0);

            _postHandle.Reference();
            _postHandle.Dispose();

            Uv.run(Loop, 0);

            Loop.Dispose();
        }

        private void OnPost()
        {
            foreach (var work in _workQueue.DequeAll())
            {
                work.Callback(work.State);
            }

            if (_stopping)
            {
                WriteReqPool.Dispose();

                _postHandle.Unreference();
            }
        }

        private void EnsureStarted()
        {
            if (!_running.IsSet)
            {
                _thread.Start(this);

                _running.Wait();
            }
        }

        private void Stop()
        {
            if (!_stopping)
            {
                _stopping = true;

                _postHandle.Send();

                // REVIEW: Make it configurable
                if (!_thread.Join(5000))
                {
                    if (Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId)
                    {
                        throw new InvalidOperationException("Can't unwind on the loop thread");
                    }
                }

                // REVIEW: Can you restart the thread?
            }
        }

        public void Dispose()
        {
            Stop();

            Pool.Dispose();
        }

        public void Schedule(Action<object> action, object state)
        {
            // REVIEW: Should we inline actions if we're already on the libuv thread?
            Post(action, state);
        }

        private struct Work
        {
            public object State;
            public Action<object> Callback;
        }
    }
}
