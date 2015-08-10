// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
    public class Image
    {
        /* Fields */ 
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
                unsafe
                {
                    return ((DLLImports.gdImageStruct*)gdImageStructPtr)->sx;
                }
            }
        }

        public int HeightInPixels
        {
            get
            {
                unsafe
                {
                    return ((DLLImports.gdImageStruct*)gdImageStructPtr)->sy;
                }
            }
        }

        public bool TrueColor
        {
            get
            {
                unsafe
                {
                    return ((DLLImports.gdImageStruct*)gdImageStructPtr)->trueColor == 1;
                }
            }
        }

        /* Release */

        public void ReleaseStruct()
        {
            DLLImports.gdImageDestroy(gdImageStructPtr);
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
}




