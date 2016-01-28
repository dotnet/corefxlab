// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.InteropServices;
using System.Text.Formatting;

namespace System.Drawing.Graphics
{
    public static class ImageExtensions
    {

        public static readonly double[,] GreyScaleMatrix = { { 1, 0, 0, 0 },
                                                      { 0, 0.3, 0.3, 0.3},
                                                      { 0, 0.59, 0.59, 0.59},
                                                      { 0, 0.11, 0.11, 0.11 } };

        public static readonly double[,] SepiaMatrix = { {1, 0, 0, 0},
                                                 {0, 0.393, 0.349, 0.272 },
                                                 {0, 0.769, 0.686,0.534},
                                                 {0 ,0.189, 0.168 , 0.131}
                                                 };

        public static readonly double[,] NegativeMatrix = { {1, 0, 0, 0},
                                                {0, -1, 0, 0 },
                                                {0, 0, -1, 0},
                                                { 0 , 0, 0, -1}
                                                 };

        ////test...
        //public static readonly double[,] AlphaMatrix040 = { {0.4, 0, 0, 0},
        //                                           {0, 1, 0, 0 },
        //                                           {0, 0, 1, 0},
        //                                           {0, 0, 0, 1}
        //                                         };

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
                throw new InvalidOperationException(string.Format(Strings.ResizeInvalidParameters, width, height));
            }
        }

        //Transparency
        public static void SetAlphaPercentage(this Image sourceImage, double opacityMultiplier)
        {
            if (opacityMultiplier > 1 || opacityMultiplier < 0)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidTransparencyPercent, opacityMultiplier));
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

        //helper method for each pixel
        private static int ApplyFilter(int currentColor, double[,] matrixMultiplier)
        {
   
            //isolate each color
            double currentAlpha = (currentColor >> 24) & 0xff;
            double currentRed = (currentColor >> 16) & 0xff;
            double currentBlue = (currentColor >> 8) & 0xff;
            double currentGreen = currentColor & 0xff;

            double[] pixel = { currentAlpha, currentRed, currentBlue, currentGreen };
            double[] pixelFinals = new double[4];
            for (int i = 0; i < pixel.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < pixel.Length; j++)
                {
                    //should alpha be dealt with seperately???????????? :(
                    //which direction should it go...
                    if(i == 0 && j == 0)
                    {
                        sum += pixel[j] + ((127 - pixel[j]) * (1 - matrixMultiplier[j, i]));

                    }
                    else
                    {
                        sum += pixel[j] * matrixMultiplier[j, i];
                    }
                }
                if(sum > 255)
                {
                    sum = 255;
                }

                //deal with overflow that is not intentional negation
                pixelFinals[i] = (int)sum & 0xff;
            }
            //shift back into position
            currentColor = ((int)pixelFinals[0] << 24) | ((int)pixelFinals[1] << 16) | ((int)pixelFinals[2] << 8) | ((int)pixelFinals[3]);
            return currentColor;

        }
        public static void ApplyMatrixMultiplier(this Image sourceImage, double[,] matrixMultiplier)
        {
            unsafe
            {
                DLLImports.gdImageStruct* pStruct = (DLLImports.gdImageStruct*)sourceImage.gdImageStructPtr;

                for (int y = 0; y < sourceImage.HeightInPixels; y++)
                {
                    for (int x = 0; x < sourceImage.WidthInPixels; x++)
                    {
                        int currentColor = pStruct->tpixels[y][x];
                        pStruct->tpixels[y][x] = ApplyFilter(currentColor, matrixMultiplier);
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

        //Creating an avatar of the image
        public static Image CircleCrop(this Image sourceImage, int xOffset, int yOffset)
        {
            int radius, diameter;
            if (sourceImage.HeightInPixels < sourceImage.WidthInPixels)
            {
                radius = sourceImage.HeightInPixels / 2;
            }

            else
            {
                radius = sourceImage.WidthInPixels / 2;
            }

            diameter = 2 * radius;

            Image destinationImage = Image.Create(diameter, diameter);

            unsafe
            {
                DLLImports.gdImageStruct* pStruct = (DLLImports.gdImageStruct*)sourceImage.gdImageStructPtr;
                DLLImports.gdImageStruct* fStruct = (DLLImports.gdImageStruct*)destinationImage.gdImageStructPtr;

                for (int y = 0; y < diameter; y++)
                {
                    for (int x = 0; x < diameter; x++)
                    {
                        int currentColor = pStruct->tpixels[y][x];

                        if ((x - radius - xOffset) * (x - radius - xOffset) + (y - radius - yOffset) * (y - radius - yOffset) > (radius * radius))
                        {
                            //mask to just get the alpha value (7 bits)
                            double currentAlpha = (currentColor >> 24) & 0xff;
                            currentAlpha = 127;
                            //make a new color with the new alpha to set the pixel
                            currentColor = (currentColor & 0x00ffffff | ((int)currentAlpha << 24));
                        }
                        fStruct->tpixels[y][x] = currentColor;
                    }
                }
            }
            return destinationImage;
        }
    }
}
