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

        private bool TrueColor
        {
            get
            {
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
                return (gdImageStruct.trueColor == 1);
            }
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

            if (!DLLImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException("File type not supported or not found.");
            }
            else
            {
                DLLImports.gdImageAlphaBlending(gdImageStructPtr, 1);

                if (!DLLImports.gdImageFile(gdImageStructPtr, filePath))
                {
                    throw new FileLoadException("Failed to write to file.");
                }
            }
        }

        /* Constructors */
        private Image(int width, int height)
        {
            if(width > 0 && height > 0)
            {
                gdImageStructPtr = DLLImports.gdImageCreateTrueColor(width, height);
            }
            else
            {
                throw new InvalidOperationException("Parameters for creating an image must be positive integers.") ;
            }
        }
        private Image(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Malformed file path given.");
            }
            if (DLLImports.gdSupportsFileType(filePath, false))
            {
                gdImageStructPtr = DLLImports.gdImageCreateFromFile(filePath);
                if(!TrueColor)
                {
                    DLLImports.gdImagePaletteToTrueColor(gdImageStructPtr);
                }
                DLLImports.gdImageAlphaBlending(gdImageStructPtr, 0);
                DLLImports.gdImageSaveAlpha(gdImageStructPtr, 1);
            }
            else
            {
                throw new FileLoadException("File type not supported.");
            }
        }

        private Image (Stream stream)
        {
            FromPng(stream);
        }

        private static Image FromPng(Stream stream)
		{ 
		    IntPtr pNativeImage = IntPtr.Zero; 
			//try 
			//{ 
			    var wrapper = new gdStreamWrapper(stream); 
				pNativeImage = DLLImports.gdImageCreateFromPngCtx(ref wrapper.IOCallbacks); 
    			var managedImage = Image.CopyFrom(pNativeImage); 
				//var ret = new Image(managedImage); 
				return managedImage; 
            //} 
			//finally 
			//{ 
			//    if (pNativeImage != IntPtr.Zero) 
			//	{ 
			//	    DLLImports.gdImageDestroy(pNativeImage); 
			//	} 
			// } 
		 } 
 
		public void WritePng(Stream stream)
	    {
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream); 
			DLLImports.gdImagePngCtx(ref gdImageStruct, ref wrapper.IOCallbacks); 
	   }




        internal static Image CopyFrom(IntPtr pNativeImage)
         {
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(pNativeImage); 
            Image toRet = Image.Create(gdImageStruct.sx, gdImageStruct.sy);
            toRet.gdImageStructPtr = pNativeImage;
            return toRet; 
         } 
  }
}
