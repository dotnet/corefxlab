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
                throw new FileNotFoundException(SR.Format(SR.MalformedFilePath, filePath));
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
                throw new FileLoadException(SR.Format(SR.FileTypeNotSupported, filePath));
            }
        }

        //add png specific method later
        public static void WriteToFile(Image img, string filePath)
        {

            DLLImports.gdImageSaveAlpha(img.gdImageStructPtr, 1);

            if (!DLLImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException(SR.Format(SR.FileTypeNotSupported, filePath));
            }
            else
            {

                if (!DLLImports.gdImageFile(img.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException(SR.Format(SR.WriteToFileFailed, filePath));
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
                    pNativeImage = DLLImports.gdImageCreateFromPngCtx(ref wrapper.IOCallbacks);
                    DLLImports.gdImageStruct* pStruct = (DLLImports.gdImageStruct*)pNativeImage;
                    Image toRet = Image.Create(pStruct->sx, pStruct->sx);
                    DLLImports.gdImageDestroy(toRet.gdImageStructPtr);
                    toRet.gdImageStructPtr = pNativeImage;
                    return toRet;
                }
            }
            else
            {
                throw new InvalidOperationException(SR.NullStreamReferenced);
            }
            
        }

        public static void WriteToStream(Image bmp, Stream stream)
        {
            DLLImports.gdImageSaveAlpha(bmp.gdImageStructPtr, 1);

            //MARSHALLING?
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream);
            DLLImports.gdImagePngCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }
    }
}


