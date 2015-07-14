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
            DLLImports.gdImageCopyMerge(image.gdImageStructPtr, imageToDraw.gdImageStructPtr, xOffset, yOffset, 0, 0, imageToDraw.WidthInPixels, imageToDraw.HeightInPixels, 50);
        }

    }
}
