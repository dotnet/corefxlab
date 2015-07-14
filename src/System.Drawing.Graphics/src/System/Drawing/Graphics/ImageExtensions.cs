// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing.Graphics
{
    public static class ImageExtensions
    {
        
        //Resizing
        public static Image Resize(this Image sourceImage, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                Image destinationImage = Image.Create(width, height);
                DLLImports.gdImageCopyResized(destinationImage.gdImageStructPtr, sourceImage.gdImageStructPtr, 0, 0, 0, 0,
                    destinationImage.WidthInPixels, destinationImage.HeightInPixels, sourceImage.WidthInPixels, sourceImage.HeightInPixels);
                return destinationImage;
            }
            else
            {
                throw new InvalidOperationException("Parameters for resizing an image must be positive integers.");
            }
        }

            //Transparency
        public static void SetTransparency(this Image image, double percentTransparency)
        {
            throw new NotImplementedException();

        }

        //Stamping an Image onto another
        public static void Draw(this Image image, Image imageToDraw, int xOffset, int yOffset)
        {
            //loop through the source image
            for(int y = 0; y < imageToDraw.HeightInPixels; y++)
            {
                for(int x = 0; x < imageToDraw.WidthInPixels; x++)
                {
                    int color = DLLImports.gdImageGetPixel(imageToDraw.gdImageStructPtr, x, y);

                    int alpha = (color >> 24) & 0xff;
                    if (alpha == 127)
                    {
                        continue;
                    }

                    DLLImports.gdImageSetPixel(image.gdImageStructPtr, x + xOffset, y + yOffset, color);

                }
            }
        }

    }
}
