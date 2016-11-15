// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security;

namespace System.IO.Pipelines.Compression
{
    /// <summary>
    /// Provides a wrapper around the ZLib decompression API
    /// </summary>
    internal sealed class Inflater : IDisposable
    {
        private bool _finished;                             // Whether the end of the stream has been reached
        private bool _isDisposed;                           // Prevents multiple disposals
        private ZLibNative.ZLibStreamHandle _zlibStream;    // The handle to the primary underlying zlib stream
        private readonly object _syncLock = new object();   // Used to make writing to unmanaged structures atomic 
        private const int minWindowBits = -15;              // WindowBits must be between -8..-15 to ignore the header, 8..15 for
        private const int maxWindowBits = 47;               // zlib headers, 24..31 for GZip headers, or 40..47 for either Zlib or GZip

        #region Exposed Members

        /// <summary>
        /// Initialized the Inflater with the given windowBits size
        /// </summary>
        internal Inflater(int windowBits)
        {
            Debug.Assert(windowBits >= minWindowBits && windowBits <= maxWindowBits);
            _finished = false;
            _isDisposed = false;
            InflateInit(windowBits);
        }

        public int AvailableOutput => (int)_zlibStream.AvailOut;

        public int AvailableInput => (int)_zlibStream.AvailIn;

        /// <summary>
        /// Returns true if the end of the stream has been reached.
        /// </summary>
        public bool Finished()
        {
            return _finished && _zlibStream.AvailIn == 0 && _zlibStream.AvailOut == 0;
        }

        public unsafe int Inflate(IntPtr buffer, int count)
        {
            // If Inflate is called on an invalid or unready inflater, return 0 to indicate no bytes have been read.
            if (NeedsInput())
            {
                return 0;
            }

            try
            {
                int bytesRead;
                if (ReadInflateOutput(buffer, count, ZLibNative.FlushCode.NoFlush, out bytesRead) == ZLibNative.ErrorCode.StreamEnd)
                {
                    _finished = true;
                }
                return bytesRead;
            }
            finally
            {
                // Before returning, make sure to release input buffer if necessary:
                if (_zlibStream.AvailIn == 0)
                {
                    DeallocateInputBufferHandle();
                }
            }
        }

        public bool NeedsInput()
        {
            return _zlibStream.AvailIn == 0;
        }

        public void SetInput(IntPtr buffer, int count)
        {
            lock (_syncLock)
            {
                _zlibStream.NextIn = buffer;
                _zlibStream.AvailIn = (uint)count;
                _finished = false;
            }
        }

        [SecuritySafeCritical]
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _zlibStream.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Inflater()
        {
            Dispose(false);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates the ZStream that will handle inflation
        /// </summary>
        [SecuritySafeCritical]
        private void InflateInit(int windowBits)
        {
            ZLibNative.ErrorCode error;
            try
            {
                error = ZLibNative.CreateZLibStreamForInflate(out _zlibStream, windowBits);
            }
            catch (Exception exception) // could not load the ZLib dll
            {
                throw new ZLibException("SR.ZLibErrorDLLLoadError", exception);
            }

            switch (error)
            {
                case ZLibNative.ErrorCode.Ok:           // Successful initialization
                    return;

                case ZLibNative.ErrorCode.MemError:     // Not enough memory
                    throw new ZLibException("SR.ZLibErrorNotEnoughMemory", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage());

                case ZLibNative.ErrorCode.VersionError: //zlib library is incompatible with the version assumed
                    throw new ZLibException("SR.ZLibErrorVersionMismatch", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage());

                case ZLibNative.ErrorCode.StreamError:  // Parameters are invalid
                    throw new ZLibException("SR.ZLibErrorIncorrectInitParameters", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage());

                default:
                    throw new ZLibException("SR.ZLibErrorUnexpected", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage());
            }
        }

        /// <summary>
        /// Wrapper around the ZLib inflate function, configuring the stream appropriately.
        /// </summary>
        private unsafe ZLibNative.ErrorCode ReadInflateOutput(IntPtr bufPtr, int length, ZLibNative.FlushCode flushCode, out int bytesRead)
        {
            lock (_syncLock)
            {
                _zlibStream.NextOut = bufPtr;
                _zlibStream.AvailOut = (uint)length;

                ZLibNative.ErrorCode errC = Inflate(flushCode);
                bytesRead = length - (int)_zlibStream.AvailOut;

                return errC;
            }
        }

        /// <summary>
        /// Wrapper around the ZLib inflate function
        /// </summary>
        [SecuritySafeCritical]
        private ZLibNative.ErrorCode Inflate(ZLibNative.FlushCode flushCode)
        {
            ZLibNative.ErrorCode errC;
            try
            {
                errC = _zlibStream.Inflate(flushCode);
            }
            catch (Exception cause) // could not load the Zlib DLL correctly
            {
                throw new ZLibException("SR.ZLibErrorDLLLoadError", cause);
            }
            switch (errC)
            {
                case ZLibNative.ErrorCode.Ok:           // progress has been made inflating
                case ZLibNative.ErrorCode.StreamEnd:    // The end of the input stream has been reached
                    return errC;

                case ZLibNative.ErrorCode.BufError:     // No room in the output buffer - inflate() can be called again with more space to continue
                    return errC;

                case ZLibNative.ErrorCode.MemError:     // Not enough memory to complete the operation
                    throw new ZLibException("SR.ZLibErrorNotEnoughMemory", "inflate_", (int)errC, _zlibStream.GetErrorMessage());

                case ZLibNative.ErrorCode.DataError:    // The input data was corrupted (input stream not conforming to the zlib format or incorrect check value)
                    throw new InvalidDataException("SR.UnsupportedCompression");

                case ZLibNative.ErrorCode.StreamError:  //the stream structure was inconsistent (for example if next_in or next_out was NULL),
                    throw new ZLibException("SR.ZLibErrorInconsistentStream", "inflate_", (int)errC, _zlibStream.GetErrorMessage());

                default:
                    throw new ZLibException("SR.ZLibErrorUnexpected", "inflate_", (int)errC, _zlibStream.GetErrorMessage());
            }
        }

        /// <summary>
        /// Frees the GCHandle being used to store the input buffer
        /// </summary>
        private void DeallocateInputBufferHandle()
        {
            lock (_syncLock)
            {
                _zlibStream.AvailIn = 0;
                _zlibStream.NextIn = ZLibNative.ZNullPtr;
            }
        }

        #endregion
    }
}
