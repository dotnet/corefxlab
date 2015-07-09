// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
    //assuming only these three types for now (32 bit)
    public enum PixelFormat
    {
        ARGB,
        RGBA,
        CMYK
    }

    public class Image
    {
        /* Private Fields */
        ////do we even need any of this anymore??
        private byte[] _data = null;
        private int _width = 0;
        private int _height = 0;
        private PixelFormat _pixelFormat = PixelFormat.ARGB;
        private int _bytesPerPixel = 0;

        private DLLImports.gdImageStruct _gdImgStruct;

            /* Properties */
        public int WidthInPixels
        {
            get { return _width; }
        }
        public int HeightInPixels
        {
            get { return _height; }
        }
        public PixelFormat PixelFormat
        {
            get { return _pixelFormat; }
        }
        public byte[] Data
        {
            get { return _data; }
        }
        public int BytesPerPixel
        {
            get { return _bytesPerPixel; }
        }

            /* Factory Methods */
        public static Image Create(int width, int height)
        {
            return new Image(width, height);
        }
        public static Image Load(string filePath)
        {
            return new Image(filePath);
        }
        public static Image Load(Stream stream)
        {
            return new Image(stream);
        }

            /* Constructors */
        private Image(int width, int height)
        {
            IntPtr gdImagePtr = DLLImports.gdImageCreate(width, height);
            _gdImgStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImagePtr);
        }
        private Image(string filepath)
        {
            throw new NotImplementedException();
        }
        private Image(Stream stream)
        {
            throw new NotImplementedException();
        }


    }
}
