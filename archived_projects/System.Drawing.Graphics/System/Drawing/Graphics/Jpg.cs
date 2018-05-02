// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.IO;
using System.Runtime.InteropServices;
using System.Text.Formatting;

namespace System.Drawing.Graphics
{
    public static class Jpg
    {
        //add jpg specific method later
        public static Image Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format(Strings.MalformedFilePath, filePath));
            }
            else if (DLLImports.gdSupportsFileType(filePath, false))
            {
                Image img = new Image(DLLImports.gdImageCreateFromFile(filePath));

                if (!img.TrueColor)
                {
                    DLLImports.gdImagePaletteToTrueColor(img.gdImageStructPtr);
                }
                return img;
            }
            else
            {
                throw new FileLoadException(string.Format(Strings.FileTypeNotSupported, filePath));
            }
        }

        //add jpg specific method later
        public static void Write(Image img, string filePath)
        {
            DLLImports.gdImageSaveAlpha(img.gdImageStructPtr, 1);

            if (!DLLImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException(string.Format(Strings.FileTypeNotSupported, filePath));
            }
            else
            {
                if (!DLLImports.gdImageFile(img.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException(string.Format(Strings.WriteToFileFailed, filePath));
                }
            }
        }


        public static Image Load(Stream stream)
        {
            if (stream != null)
            {
                unsafe
                {
                    IntPtr pNativeImage = IntPtr.Zero;
                    var wrapper = new gdStreamWrapper(stream);
                    pNativeImage = DLLImports.gdImageCreateFromJpegCtx(ref wrapper.IOCallbacks);
                    DLLImports.gdImageStruct* pStruct = (DLLImports.gdImageStruct*)pNativeImage;
                    Image toRet = Image.Create(pStruct->sx, pStruct->sx);
                    DLLImports.gdImageDestroy(toRet.gdImageStructPtr);
                    toRet.gdImageStructPtr = pNativeImage;                 
                    return toRet;
                }
            }
            else
            {
                throw new InvalidOperationException(Strings.NullStreamReferenced);
            }

        }

        public static void Write(Image bmp, Stream stream)
        {
            DLLImports.gdImageSaveAlpha(bmp.gdImageStructPtr, 1);
            
            //MARSHALLING?
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream);
            DLLImports.gdImageJpegCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }
    }
}



