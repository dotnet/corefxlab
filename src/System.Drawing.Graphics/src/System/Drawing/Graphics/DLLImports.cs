// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 


using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
    internal class DLLImports
    {
        const int gdMaxColors = 256;

        enum gdInterpolationMethod
        {
            GD_DEFAULT = 0,
            GD_BELL,
            GD_BESSEL,
            GD_BILINEAR_FIXED,
            GD_BICUBIC,
            GD_BICUBIC_FIXED,
            GD_BLACKMAN,
            GD_BOX,
            GD_BSPLINE,
            GD_CATMULLROM,
            GD_GAUSSIAN,
            GD_GENERALIZED_CUBIC,
            GD_HERMITE,
            GD_HAMMING,
            GD_HANNING,
            GD_MITCHELL,
            GD_NEAREST_NEIGHBOUR,
            GD_POWER,
            GD_QUADRATIC,
            GD_SINC,
            GD_TRIANGLE,
            GD_WEIGHTED4,
            GD_METHOD_COUNT = 21
        };

        public delegate double interpolation_method(double param0);

        [StructLayout(LayoutKind.Sequential)]
        //[CLSCompliant(false)]
        unsafe internal struct gdImageStruct
        {
            public byte** pixels;
            //public IntPtr pixels;
            public int sx;
            public int sy;
            public int colorsTotal;
            public fixed int red[gdMaxColors];
            public fixed int green[gdMaxColors];
            public fixed int blue[gdMaxColors];
            public fixed int open[gdMaxColors];
            public int transparent;
            public IntPtr polyInts;
            public int polyAllocated;
            IntPtr brush;
            IntPtr tile;
            public fixed int brushColorMap[gdMaxColors];
            public fixed int tileColorMap[gdMaxColors];
            public int styleLength;
            public int stylePos;
            public IntPtr style;
            public int interlace;
            public int thick;
            public fixed int alpha[gdMaxColors];
            public int trueColor;
            public int** tpixels;
            public int alphaBlendingFlag;
            public int saveAlphaFlag;
            public int AA;
            public int AA_color;
            public int AA_dont_blend;
            public int cx1;
            public int cy1;
            public int cx2;
            public int cy2;
            public uint res_x;
            public uint res_y;
            public int paletteQuantizationMethod;
            public int paletteQuantizationSpeed;
            public int paletteQuantizationMinQuality;
            public int paletteQuantizationMaxQuality;
            //gdInterpolationMethod interpolation_id;
            //interpolation_method interpolation;
        }

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Ansi)]
        internal static extern bool gdSupportsFileType(string filename, bool writing);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern IntPtr gdImageCreate(int sx, int sy);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern IntPtr gdImageCreateTrueColor(int sx, int sy);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode, EntryPoint = Interop.LibGDColorAllocateEntryPoint)]
        internal static extern int gdImageColorAllocate(int r, int b, int g);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Ansi)]
        internal static extern IntPtr gdImageCreateFromFile(string filename);


        //had to use mangled name here
        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Ansi, EntryPoint = Interop.LibGDImageFileEntryPoint)]
        internal static extern bool gdImageFile(IntPtr image, string filename);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageCopyResized(IntPtr destination, IntPtr source, int destinationX, int destinationY,
                                        int sourceX, int sourceY, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageCopyMerge(IntPtr destination, IntPtr source, int dstX, int dstY, int srcX, int srcY, int w, int h, int pct);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageColorTransparent(IntPtr im, int color);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageSaveAlpha(IntPtr im, int flag);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageAlphaBlending(IntPtr im, int flag);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern int gdImageGetPixel(IntPtr im, int x, int y);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern int gdImageGetTrueColorPixel(IntPtr im, int x, int y);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern void gdImageSetPixel(IntPtr im, int x, int y, int color);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        internal static extern int gdImagePaletteToTrueColor(IntPtr src);

        [DllImport(Interop.LibGDBinary, EntryPoint = Interop.LibGDImageCreateFromPngCtxEntryPoint)] 
        public static extern IntPtr gdImageCreateFromPngCtx(ref gdIOCtx @in);

        [DllImport(Interop.LibGDBinary, EntryPoint = Interop.LibGDImagePngCtxEntryPoint)] 
        public static extern void gdImagePngCtx(ref gdImageStruct im, ref gdIOCtx @out);

        [DllImport(Interop.LibGDBinary, EntryPoint = Interop.LibGDImageCreateFromJpegCtxEntryPoint)]
        public static extern IntPtr gdImageCreateFromJpegCtx(ref gdIOCtx @in);

        [DllImport(Interop.LibGDBinary, EntryPoint = Interop.LibGDImageJpegCtxEntryPoint)]
        public static extern void gdImageJpegCtx(ref gdImageStruct im, ref gdIOCtx @out);


        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        public static extern void gdImageDestroy(IntPtr im);

        [DllImport(Interop.LibGDBinary, CharSet = CharSet.Unicode)]
        public static extern int gdAlphaBlend(int src, int dst);



        /// Return Type: int 
        ///param0: gdIOCtx* 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate int gdIOCtx_getC(IntPtr ctx); 
 
 
        /// Return Type: int 
        ///param0: gdIOCtx* 
        ///param1: void* 
        ///param2: int 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate int gdIOCtx_getBuf(IntPtr ctx, System.IntPtr buf, int wanted); 
 
        /// Return Type: void 
        ///param0: gdIOCtx* 
        ///param1: int 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate void gdIOCtx_putC(IntPtr ctx, int ch); 
 
        /// Return Type: int 
        ///param0: gdIOCtx* 
        ///param1: void* 
        ///param2: int 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate int gdIOCtx_putBuf(IntPtr ctx, System.IntPtr buf, int wanted); 

        /// Return Type: int 
        ///param0: gdIOCtx* 
        ///param1: int 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate int gdIOCtx_seek(IntPtr ctx, int pos); 
 
        /// Return Type: int 
        ///param0: gdIOCtx* 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate int gdIOCtx_tell(IntPtr ctx); 

        /// Return Type: void 
        ///param0: gdIOCtx* 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        public delegate void gdIOCtx_gd_free(IntPtr param0);


        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)] 
        public struct gdIOCtx
        { 
      
            /// gdIOCtx_getC 
            public gdIOCtx_getC getC; 
      
            /// gdIOCtx_getBuf 
            public gdIOCtx_getBuf getBuf; 
      
            /// gdIOCtx_putC 
            public gdIOCtx_putC putC; 
      
            /// gdIOCtx_putBuf 
            public gdIOCtx_putBuf putBuf; 
      
            /// gdIOCtx_seek 
            public gdIOCtx_seek seek; 
      
            /// gdIOCtx_tell 
            public gdIOCtx_tell tell; 
      
            /// gdIOCtx_gd_free 
            public gdIOCtx_gd_free gd_free; 
      
            /// void* 
            public System.IntPtr data; 
        }


    }
}