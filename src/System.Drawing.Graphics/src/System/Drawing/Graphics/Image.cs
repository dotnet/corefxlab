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
        private int _height;
        private int _width;
        private byte[] _pixelData = null;
        private PixelFormat _pixelFormat = PixelFormat.ARGB;
        private int _bytesPerPixel = 0;

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
        public byte[] PixelData
        {
            get { return _pixelData; }
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

        /* Write */
        public void Write(string filePath)
        {
            unsafe{
                fixed (byte* pPixelData = _pixelData)
                {
                    DLLImports.gdImageStruct s = new DLLImports.gdImageStruct();
                    s.pixels = pPixelData;
                    s.sx = _width;
                    s.sy = _height;
                    DLLImports.gdImageFile(s, filePath);
                }
            }
        }

        /* Constructors */
        private Image(int width, int height)
        {
            if(width > 0 && height > 0)
            {
                _width = width;
                _height = height;
                _pixelData = new byte[width * height];
            }
            else
            {
                throw new Exception();
            }
            
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
