// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
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

    public class Bitmap
    {
        /* Fields */ 
        private PixelFormat _pixelFormat = PixelFormat.Argb;
        private int _bytesPerPixel = 0;
        internal IntPtr gdImageStructPtr;

        public Bitmap(IntPtr gdImageStructPtr)
        {
            this.gdImageStructPtr = gdImageStructPtr;
        }

        /* Properties*/ 
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

        //public byte[][] PixelData
        //{
        //    get { return _pixelData; }
        //}
       
        public int BytesPerPixel
        {
            get { return _bytesPerPixel; }
        }

        private bool TrueColor
        {
            get
            {
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
                return (gdImageStruct.trueColor == 1);
            }
        }



        /* Factory Methods */
        public static Bitmap Create(int width, int height)
        {
            return new Bitmap(width, height);
        }
        //public static Image Load(string filePath)
        //{
        //    return new Image(filePath);
        //}
        //public static Image Load(Stream stream)
        //{
        //    return new Image(stream);
        //}

        ///* Write */
        //public void WriteToFile(string filePath)
        //{
        //    DLLImports.gdImageSaveAlpha(gdImageStructPtr, 1);

        //    if (!DLLImports.gdSupportsFileType(filePath, true))
        //    {
        //        throw new InvalidOperationException("File type not supported or not found.");
        //    }
        //    else
        //    {
        //        if (!DLLImports.gdImageFile(gdImageStructPtr, filePath))
        //        {
        //            throw new FileLoadException("Failed to write to file.");
        //        }
        //    }
        //}

        /* constructors */
        private Bitmap(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                gdImageStructPtr = DLLImports.gdImageCreateTrueColor(width, height);
            }
            else
            {
                throw new InvalidOperationException("parameters for creating an image must be positive integers.");
            }
        }

        //private Image(string filePath)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        throw new FileNotFoundException("Malformed file path given.");
        //    }
        //    else if (DLLImports.gdSupportsFileType(filePath, false))
        //    {
        //        gdImageStructPtr = DLLImports.gdImageCreateFromFile(filePath);
        //        DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
        //        if(gdImageStruct.trueColor == 0)
        //        {
        //            int a = DLLImports.gdImagePaletteToTrueColor(gdImageStructPtr);
        //            gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
        //        }
        //    }
        //    else
        //    {
        //        throw new FileLoadException("File type not supported.");
        //    }
        //}

        //private Image(Stream stream)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
