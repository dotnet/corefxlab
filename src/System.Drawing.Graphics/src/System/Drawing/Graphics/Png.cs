// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
    public static class Png
    {
        //add png specific method later
        public static Image Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Malformed file path given.");
            }
            else if (DLLImports.gdSupportsFileType(filePath, false))
            {
                Image img = new Image(DLLImports.gdImageCreateFromFile(filePath));
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(img.gdImageStructPtr);

                if (gdImageStruct.trueColor == 0)
                {
                    int a = DLLImports.gdImagePaletteToTrueColor(img.gdImageStructPtr);
                    gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(img.gdImageStructPtr);
                }
                return img;
            }
            else
            {
                throw new FileLoadException("File type not supported.");
            }
        }

        //add png specific method later
        public static void WriteToFile(Image img, string filePath)
        {
            DLLImports.gdImageSaveAlpha(img.gdImageStructPtr, 1);

            if (!DLLImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException("File type not supported or not found.");
            }
            else
            {
                if (!DLLImports.gdImageFile(img.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException("Failed to write to file.");
                }
            }
        }
    }
}
