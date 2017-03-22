﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipelines.File
{
    public class FileReader
    {
        private readonly IPipeWriter _writer;

        public FileReader(IPipeWriter writer)
        {
            _writer = writer;
        }

        // Win32 file impl
        // TODO: Other platforms
        public unsafe void OpenReadFile(string path)
        {
            var fileHandle = CreateFile(path, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, EFileAttributes.Overlapped, IntPtr.Zero);

            var handle = ThreadPoolBoundHandle.BindHandle(fileHandle);

            var readOperation = new ReadOperation
            {
                Writer = _writer,
                FileHandle = fileHandle,
                ThreadPoolBoundHandle = handle,
                IOCallback = IOCallback
            };

            var overlapped = new PreAllocatedOverlapped(IOCallback, readOperation, null);
            readOperation.PreAllocatedOverlapped = overlapped;

            Task.Factory.StartNew(state =>
            {
                ((ReadOperation)state).Read();
            },
            readOperation);
        }

        public unsafe static void IOCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
        {
            var state = ThreadPoolBoundHandle.GetNativeOverlappedState(pOverlapped);
            var operation = (ReadOperation)state;

            operation.ThreadPoolBoundHandle.FreeNativeOverlapped(operation.Overlapped);

            operation.Offset += (int)numBytes;

            var buffer = operation.BoxedBuffer.Value;

            buffer.Advance((int)numBytes);
            var awaitable = buffer.FlushAsync();

            if (numBytes == 0)
            {
                operation.Writer.Complete();

                // The operation can be disposed when there's nothing more to produce
                operation.Dispose();
            }
            else if (awaitable.IsCompleted)
            {
                // No back pressure being applied to continue reading
                operation.Read();
            }
            else
            {
                var ignore = Continue(awaitable, operation);
            }
        }

        private static async Task Continue(WritableBufferAwaitable awaitable, ReadOperation operation)
        {
            // Keep reading once we get the completion
            var flushResult = await awaitable;
            if (!flushResult.IsCompleted)
            {
                operation.Read();
            }
            else
            {
                operation.Writer.Complete();

                operation.Dispose();
            }
        }

        private class ReadOperation
        {
            public IOCompletionCallback IOCallback { get; set; }
            public SafeFileHandle FileHandle { get; set; }

            public PreAllocatedOverlapped PreAllocatedOverlapped { get; set; }

            public ThreadPoolBoundHandle ThreadPoolBoundHandle { get; set; }

            public unsafe NativeOverlapped* Overlapped { get; set; }

            public IPipeWriter Writer { get; set; }

            public StrongBox<WritableBuffer> BoxedBuffer { get; set; }

            public int Offset { get; set; }

            public unsafe void Read()
            {
                var buffer = Writer.Alloc(2048);
                fixed (byte* source = &buffer.Buffer.Span.DangerousGetPinnableReference())
                {
                    var count = buffer.Buffer.Length;

                    var overlapped = ThreadPoolBoundHandle.AllocateNativeOverlapped(PreAllocatedOverlapped);
                    overlapped->OffsetLow = Offset;

                    Overlapped = overlapped;

                    BoxedBuffer = new StrongBox<WritableBuffer>(buffer);

                    int r = ReadFile(FileHandle, (IntPtr)source, count, IntPtr.Zero, overlapped);

                    // TODO: Error handling

                    // 997
                    int hr = Marshal.GetLastWin32Error();
                    if (hr != 997)
                    {
                        Writer.Complete(Marshal.GetExceptionForHR(hr));
                    }
                }
            }

            public void Dispose()
            {
                FileHandle.Dispose();

                ThreadPoolBoundHandle.Dispose();

                PreAllocatedOverlapped.Dispose();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern unsafe int ReadFile(
            SafeFileHandle hFile,      // handle to file
            IntPtr pBuffer,        // data buffer, should be fixed
            int NumberOfBytesToRead,  // number of bytes to read
            IntPtr pNumberOfBytesRead,  // number of bytes read, provide IntPtr.Zero here
            NativeOverlapped* lpOverlapped // should be fixed, if not IntPtr.Zero
        );

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
            IntPtr template
        );


        [Flags]
        private enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
    }
}
