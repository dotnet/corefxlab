// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

//#define WINDOWS
//#define LINUX


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

#if (WINDOWS && !LINUX)
    public class Image
    {
        /* Fields */ 
        private PixelFormat _pixelFormat = PixelFormat.Argb;
        private int _bytesPerPixel = 0;
        internal IntPtr gdImageStructPtr;

        public Image(IntPtr gdImageStructPtr)
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
        public int BytesPerPixel
        {
            get { return _bytesPerPixel; }
        }

        public bool TrueColor
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

        /* constructors */
        private Image(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                gdImageStructPtr = DLLImports.gdImageCreateTrueColor(width, height);

            }
            else
            {
                string rsc = SR.Format(SR.CreateInvalidParameters, width, height);
                throw new InvalidOperationException(rsc);
            }
        }
    }

#if (LINUX && !WINDOWS)

    public class Image
    {
        /* Fields */ 
        private PixelFormat _pixelFormat = PixelFormat.Argb;
        private int _bytesPerPixel = 0;
        internal IntPtr gdImageStructPtr;

        public Image(IntPtr gdImageStructPtr)
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
        public int BytesPerPixel
        {
            get { return _bytesPerPixel; }
        }

        public bool TrueColor
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

        /* constructors */
        private Image(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                gdImageStructPtr = DLLImports.gdImageCreateTrueColor(width, height);

            }
            else
            {
                string rsc = SR.Format(SR.CreateInvalidParameters, width, height);
                throw new InvalidOperationException(rsc);
            }
        }
    }
#endif

#else

    public class Image
    {
        /* Fields */ 
        private PixelFormat _pixelFormat = PixelFormat.Argb;
        private int _bytesPerPixel = 0;
        internal IntPtr gdImageStructPtr;

        public Image(IntPtr gdImageStructPtr)
        {
            this.gdImageStructPtr = gdImageStructPtr;
        }


        /* Properties*/
        public int WidthInPixels
        {
            get
            {
                LibGDOSXImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDOSXImports.gdImageStruct>(gdImageStructPtr);
                return gdImageStruct.sx;
            }
        }

        public int HeightInPixels
        {
            get
            {
                LibGDOSXImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDOSXImports.gdImageStruct>(gdImageStructPtr);
                return gdImageStruct.sy;
            }
        }
        public PixelFormat PixelFormat
        {
            get { return _pixelFormat; }
        }
        public int BytesPerPixel
        {
            get { return _bytesPerPixel; }
        }

        public bool TrueColor
        {
            get
            {
                LibGDOSXImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDOSXImports.gdImageStruct>(gdImageStructPtr);
                return (gdImageStruct.trueColor == 1);
            }
        }
        
        /* Factory Methods */
        public static Image Create(int width, int height)
        {
            return new Image(width, height);
        }

        /* constructors */
        private Image(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                gdImageStructPtr = LibGDOSXImports.gdImageCreateTrueColor(width, height);
            }
            else
            {
                throw new InvalidOperationException(SR.Format(SR.CreateInvalidParameters, width, height));
            }
        }
    }
}
#endif



