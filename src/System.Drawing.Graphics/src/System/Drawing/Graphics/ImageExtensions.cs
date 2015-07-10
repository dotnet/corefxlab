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
            //throw new NotImplementedException();
            if (width > 0 && height > 0)
            {
                Image imageToRet = Image.Create(width, height);

                unsafe
                {
                    //fix the pixelData[] for both the new blank image and 
                    //source original image
                    fixed (byte* imageToRetPixelDataPtr = imageToRet.Data)
                    fixed (byte* sourceImagePixelDataPtr = sourceImage.Data)
                    {
                        //make a new temporary structure for the new blank image
                        DLLImports.gdImageStruct imageToRetStructure = new DLLImports.gdImageStruct();
                        //pin pixel data of new image to struct
                        imageToRetStructure.pixels = imageToRetPixelDataPtr;

                        //make a new temporary structure for the source image
                        DLLImports.gdImageStruct sourceImageStructure = new DLLImports.gdImageStruct();
                        //pin pixel data of  source image to struct
                        sourceImageStructure.pixels = sourceImagePixelDataPtr;

                        //call native library to resize
                        DLLImports.gdImageCopyResized(ref imageToRetStructure, ref sourceImageStructure, 0, 0,
                                            0, 0, imageToRet.WidthInPixels, imageToRet.HeightInPixels, sourceImage.WidthInPixels, sourceImage.HeightInPixels);

                    }
                }
                //return image
                //._pixelData[] should be changed 
                return imageToRet;
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
            throw new NotImplementedException();
        }

    }
}
