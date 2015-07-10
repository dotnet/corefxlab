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
        Argb,
        Rgba,
        Cmyk
    }

    public class Image
    {
        /* Private Fields */
        ////do we even need any of this anymore??
        private byte[][] _data = null;
        private int _width = 0;
        private int _height = 0;
        private PixelFormat _pixelFormat = PixelFormat.Argb;
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
        public byte[][] Data
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
            if(width > 0 && height > 0)
            {
                _width = width;
                _height = height;
                _data = new byte[height][];
                for (int i = 0; i < height; i++)
                {
                    _data[i] = new byte[width];
                }
            }
            else
            {
                throw new InvalidOperationException("Parameters for creating an image must be positive integers.") ;
            }
        }
        private Image(string filepath)
        {
            unsafe
            {

            }

            throw new NotImplementedException();
        }
        private Image(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
