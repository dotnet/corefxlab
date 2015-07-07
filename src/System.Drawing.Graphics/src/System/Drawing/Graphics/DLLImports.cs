// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 


using System.Runtime.InteropServices;

namespace System.Drawing.Graphics {
    public class DLLImports
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
        unsafe internal struct gdImagePtr
        {
            public IntPtr pixels;
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
            public int styleLength;
            public int stylePos;
            public int style;
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

        [DllImport("libgdx86.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr gdImageCreate(int sx, int sy);
       

    }
}