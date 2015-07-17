﻿// Copyright (c) Microsoft. All rights reserved. 
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
        public static Bitmap Resize(this Bitmap sourceBitmap, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                Bitmap destinationImage = Bitmap.Create(width, height);
                DLLImports.gdImageCopyResized(destinationImage.gdImageStructPtr, sourceBitmap.gdImageStructPtr, 0, 0, 0, 0,
                    destinationImage.WidthInPixels, destinationImage.HeightInPixels, sourceBitmap.WidthInPixels, sourceBitmap.HeightInPixels);
                return destinationImage;
            }
            else
            {
                throw new InvalidOperationException("Parameters for resizing an image must be positive integers.");
            }
        }

        //Transparency
        public static void SetAlphaPercentage(this Bitmap sourceBitmap, double percentOpacity)
        {
            if(percentOpacity > 100 || percentOpacity < 0)
            {
                throw new InvalidOperationException("Percent Transparency must be a value between 0 - 100.");
            }

            double alphaAdjustment = (100.0 - percentOpacity) / 100.0;
            for(int y = 0; y < sourceBitmap.HeightInPixels; y++)
            {
                for(int x = 0; x < sourceBitmap.WidthInPixels; x++)
                {
                    //get the current color of the pixel
                    int currentColor = DLLImports.gdImageGetPixel(sourceBitmap.gdImageStructPtr, x, y);
                    //mask to just get the alpha value (7 bits)
                    double currentAlpha = (currentColor >> 24) & 0xff;

                    
                    //if the current alpha is transparent
                    //dont bother/ skip over
                    if (currentAlpha == 127)
                        continue;
                    //calculate the new alpha value given the adjustment

                    currentAlpha += (127 - currentAlpha) * alphaAdjustment;
                    //if it is somehow transparent now
                    //dont bother setting pixel/skip over
                    if (currentAlpha >= 127)
                        continue;

                    //make a new color with the new alpha to set the pixel
                    currentColor = (currentColor & 0x00ffffff | ((int)currentAlpha << 24));
                    //turn alpha blending off so you don't draw over the same picture and get an opaque cat
                    DLLImports.gdImageAlphaBlending(sourceBitmap.gdImageStructPtr, 0);

                    DLLImports.gdImageSetPixel(sourceBitmap.gdImageStructPtr, x, y, currentColor);
                }
            }
        }

        //Stamping an Image onto another
        public static void Draw(this Bitmap destinationBitmap, Bitmap sourceBitmap, int xOffset, int yOffset)
        {
            //turn alpha blending on for drawing
            DLLImports.gdImageAlphaBlending(destinationBitmap.gdImageStructPtr, 1);
            //DLLImports.gdImageAlphaBlending(imageToDraw.gdImageStructPtr, 1);

            //loop through the source image
            for (int y = 0; y < sourceBitmap.HeightInPixels; y++)
            {
                for(int x = 0; x < sourceBitmap.WidthInPixels; x++)
                {
                    int color = DLLImports.gdImageGetPixel(sourceBitmap.gdImageStructPtr, x, y);

                    int alpha = (color >> 24) & 0xff;
                    if (alpha == 127)
                    {
                        continue;
                    }

                    DLLImports.gdImageSetPixel(destinationBitmap.gdImageStructPtr, x + xOffset, y + yOffset, color);
                }
            }
        }
    }
}
