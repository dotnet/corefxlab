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
        ///* Private Fields */
        //private int _height;
        //private int _width;
        private byte[][] _pixelData = null;
        private PixelFormat _pixelFormat = PixelFormat.Argb;
        private int _bytesPerPixel = 0;

        internal IntPtr gdImageStructPtr;

        public int WidthInPixels
        {
            get
            {
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
                return gdImageStruct.sx;
            }
        }
        public int HeightInPixels
        {
            get
            {
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
                return gdImageStruct.sy;
            }
        }
        public PixelFormat PixelFormat
        {
            get { return _pixelFormat; }
        }
        public byte[][] PixelData
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
        public void WriteToFile(string filePath)
        {
            //check valid file path
                
             if(DLLImports.gdImageFile(gdImageStructPtr, filePath) == 0)
            {
                throw new Exception("Failed to write to File.");
            }
        }

        /* Constructors */
        private Image(int width, int height)
        {
            if(width > 0 && height > 0)
            {
                gdImageStructPtr = DLLImports.gdImageCreate(width, height);
            }
            else
            {
                throw new InvalidOperationException("Parameters for creating an image must be positive integers.") ;
            }
        }
        private Image(string filepath)
        {
            gdImageStructPtr = DLLImports.gdImageCreateFromFile(filepath);
        }
        private Image(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
