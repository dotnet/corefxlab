using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Compression.Brotli
{
    internal sealed class Encoder
    {
        private const int MinWindowBits = 10;
        private const int MaxWindowBits = 24;
        private const int MinQuality = 0;
        private const int MaxQuality = 11;
        private bool _isDisposed=false;
        internal IntPtr State=IntPtr.Zero;

        internal Encoder()
        {
            _isDisposed = false;
            InitializeEncoder();
        }
        public void SetQuality(uint quality)
        {
            if (quality < MinQuality || quality > MaxQuality)
            {
                throw new ArgumentException();//TODO
            }
            BrotliNative.BrotliEncoderSetParameter(State, BrotliNative.BrotliEncoderParameter.Quality, quality);
        }
        public void SetQuality()
        {
            SetQuality(MaxQuality);
        }
        public void SetWindow(uint window)
        {
            if (window < MinWindowBits || window > MaxWindowBits)
            {
                throw new ArgumentException();//TODO
            }
            BrotliNative.BrotliEncoderSetParameter(State, BrotliNative.BrotliEncoderParameter.LGWin, window);
        }
        public void SetWindow()
        {
            SetWindow(MaxWindowBits);
        }
        private void InitializeEncoder()
        {
            State = BrotliNative.BrotliEncoderCreateInstance();
            if (State == IntPtr.Zero)
            {
                throw new Exception();//TODO Create exception
            }
        }
        internal void Dispose()
        {
            if (!_isDisposed && State != IntPtr.Zero)
            {
                BrotliNative.BrotliEncoderDestroyInstance(State);
            }
            _isDisposed = true;
        }
    }
}
