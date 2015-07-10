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
            throw new NotImplementedException();
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
