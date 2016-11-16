using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.OpenSsl
{
    internal unsafe static class LockStore
    {
        private static SemaphoreSlim[] _locks;
        public static readonly InteropCrypto.locking_function Callback;

        static LockStore()
        {
            var numberOfLocks = InteropCrypto.CRYPTO_num_locks();
            _locks = new SemaphoreSlim[numberOfLocks];
            for (int i = 0; i < _locks.Length; i++)
            {
                _locks[i] = new SemaphoreSlim(1);
            }
            Callback = HandleLock;
        }

        [Diagnostics.DebuggerHidden]
        private static unsafe void HandleLock(InteropCrypto.LockState lockState, int lockId, byte* file, int lineNumber)
        {
            if ((lockState & InteropCrypto.LockState.CRYPTO_UNLOCK) > 0)
            {
                _locks[lockId].Release();
            }
            else if ((lockState & InteropCrypto.LockState.CRYPTO_LOCK) > 0)
            {
                _locks[lockId].Wait();
            }
        }
    }
}
