// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 


using System.Runtime.InteropServices;

namespace System.Drawing.Graphics {
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
            gdInterpolationMethod interpolation_id;
            interpolation_method interpolation;
        }

        [DllImport("libgdx86.dll", CharSet = CharSet.Ansi)]
        internal static extern bool gdSupportsFileType(string filename, bool writing);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr gdImageCreate(int sx, int sy);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr gdImageCreateTrueColor(int sx, int sy);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode, EntryPoint = "_gdImageColorAllocate@16")]
        internal static extern int gdImageColorAllocate(int r, int b, int g);

        [DllImport("libgdx86.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr gdImageCreateFromFile(string filename);


        [DllImport("libgdx86.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr gdIOCtx();



        //had to use mangled name here
        [DllImport("libgdx86.dll", CharSet = CharSet.Ansi, EntryPoint = "_gdImageFile@8")]
        internal static extern bool gdImageFile(IntPtr image, string filename);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageCopyResized(IntPtr destination, IntPtr source, int destinationX, int destinationY,
                                        int sourceX, int sourceY, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageCopyMerge(IntPtr destination, IntPtr source, int dstX, int dstY, int srcX, int srcY, int w, int h, int pct);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageColorTransparent(IntPtr im, int color);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageSaveAlpha(IntPtr im, int flag);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageAlphaBlending(IntPtr im, int flag);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]

        internal static extern int gdImageGetPixel(IntPtr im, int x, int y);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern void gdImageSetPixel(IntPtr im, int x, int y, int color);

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        internal static extern int gdImagePaletteToTrueColor(IntPtr src);
    }
}