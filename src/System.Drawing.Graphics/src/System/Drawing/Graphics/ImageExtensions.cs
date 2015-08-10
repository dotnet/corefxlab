// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.InteropServices;

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
                //turn off alpha blending to overwrite it right
                DLLImports.gdImageAlphaBlending(destinationImage.gdImageStructPtr, 0);
                DLLImports.gdImageCopyResized(destinationImage.gdImageStructPtr, sourceImage.gdImageStructPtr, 0, 0, 0, 0,
                    destinationImage.WidthInPixels, destinationImage.HeightInPixels, sourceImage.WidthInPixels, sourceImage.HeightInPixels);

                return destinationImage;
            }
            else
            {
                throw new InvalidOperationException(SR.Format(SR.ResizeInvalidParameters, width, height));
            }
        }

        ////Transparency
        public static void SetAlphaPercentage(this Image sourceImage, double opacityMultiplier)
        {
            if (opacityMultiplier > 1 || opacityMultiplier < 0)
            {
                throw new InvalidOperationException(SR.Format(SR.InvalidTransparencyPercent, opacityMultiplier));
            }

            double alphaAdjustment = 1 - opacityMultiplier;
            unsafe
            {
                DLLImports.gdImageStruct* pStruct = (DLLImports.gdImageStruct*)sourceImage.gdImageStructPtr;

                for (int y = 0; y < sourceImage.HeightInPixels; y++)
                {
                    for (int x = 0; x < sourceImage.WidthInPixels; x++)
                    {
                        int currentColor = pStruct->tpixels[y][x];
                        //mask to just get the alpha value (7 bits)
                        double currentAlpha = (currentColor >> 24) & 0xff;
                        //if the current alpha is transparent
                        //dont bother/ skip over
                        if (currentAlpha == 127)
                            continue;
                        //calculate the new alpha value given the adjustment
                        currentAlpha += (127 - currentAlpha) * alphaAdjustment;

                        //make a new color with the new alpha to set the pixel
                        currentColor = (currentColor & 0x00ffffff | ((int)currentAlpha << 24));
                        pStruct->tpixels[y][x] = currentColor;
                    }
                }
            }
        }

        //Stamping an Image onto another
        public static void Draw(this Image destinationImage, Image sourceImage, int xOffset, int yOffset)
        {
            //turn alpha blending on for drawing
            DLLImports.gdImageAlphaBlending(destinationImage.gdImageStructPtr, 1);

            unsafe
            {
                DLLImports.gdImageStruct* pStructSource = (DLLImports.gdImageStruct*)sourceImage.gdImageStructPtr;
                DLLImports.gdImageStruct* pStructDest = (DLLImports.gdImageStruct*)destinationImage.gdImageStructPtr;

                //loop through the source image
                for (int y = 0; y < sourceImage.HeightInPixels; y++)
                {
                    for (int x = 0; x < sourceImage.WidthInPixels; x++)
                    {
                        //ignores what falls outside the bounds of dsetination image
                        if ((y + yOffset) >= destinationImage.HeightInPixels || (x + xOffset) >= destinationImage.WidthInPixels)
                        {
                            continue;
                        }
                        
                        int sourceColor = pStructSource->tpixels[y][x];
                        int alpha = (sourceColor >> 24) & 0xff;
                        //should not have 127 as magic
                        if (alpha == 127)
                        {
                            continue;
                        }
                        int destColor = pStructDest->tpixels[y + yOffset][x + xOffset];
                        int blendedColor = DLLImports.gdAlphaBlend(destColor, sourceColor);

                        pStructDest->tpixels[y + yOffset][x + xOffset] = blendedColor;
                    }
                }
            }  
        }
    }
}
