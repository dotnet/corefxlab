using System;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public class UvRequest : UvMemory
    {
        private GCHandle _pin;

        protected UvRequest() : base ()
        {
        }

        protected override bool ReleaseHandle()
        {
            DestroyMemory(handle);
            handle = IntPtr.Zero;
            return true;
        }

        public virtual void Pin()
        {
            _pin = GCHandle.Alloc(this, GCHandleType.Normal);
        }

        public virtual void Unpin()
        {
            _pin.Free();
        }
    }
}

