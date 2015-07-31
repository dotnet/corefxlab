// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
//#define WINDOWS

using System.IO;
using System.Runtime.InteropServices;


namespace System.Drawing.Graphics
{
#if WINDOWS
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
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(img.gdImageStructPtr);

                if (!img.TrueColor)
                {
                    DLLImports.gdImagePaletteToTrueColor(img.gdImageStructPtr);
                    gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(img.gdImageStructPtr);
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
                IntPtr pNativeImage = IntPtr.Zero;
                var wrapper = new gdStreamWrapper(stream);
                pNativeImage = DLLImports.gdImageCreateFromPngCtx(ref wrapper.IOCallbacks);

                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(pNativeImage);
                Image toRet = Image.Create(gdImageStruct.sx, gdImageStruct.sy);
                toRet.gdImageStructPtr = pNativeImage;

                return toRet;
            }
            else
            {
                throw new InvalidOperationException(SR.NullStreamReferenced);
            }
            
        }

        public static void WriteToStream(Image bmp, Stream stream)
        {
            DLLImports.gdImageSaveAlpha(bmp.gdImageStructPtr, 1);

            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream);
            DLLImports.gdImagePngCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }




    }
}

#else

    public static class Png
    {
        //add png specific method later
        public static Image Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(SR.Format(SR.MalformedFilePath, filePath));
            }
            else if (LibGDLinuxImports.gdSupportsFileType(filePath, false))
            {
                Image img = new Image(LibGDLinuxImports.gdImageCreateFromFile(filePath));
                LibGDLinuxImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(img.gdImageStructPtr);

                if (!img.TrueColor)
                {
                    LibGDLinuxImports.gdImagePaletteToTrueColor(img.gdImageStructPtr);
                    gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(img.gdImageStructPtr);
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
            LibGDLinuxImports.gdImageSaveAlpha(img.gdImageStructPtr, 1);

            if (!LibGDLinuxImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException(SR.Format(SR.FileTypeNotSupported, filePath));
            }
            else
            {
                if (!LibGDLinuxImports.gdImageFile(img.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException(SR.Format(SR.WriteToFileFailed, filePath));
                }
            }
        }


        public static Image Load(Stream stream)
        {
            if(stream != null)
            {
                IntPtr pNativeImage = IntPtr.Zero;
                var wrapper = new gdStreamWrapper(stream);
                pNativeImage = LibGDLinuxImports.gdImageCreateFromPngCtx(ref wrapper.IOCallbacks);

                LibGDLinuxImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(pNativeImage);
                Image toRet = Image.Create(gdImageStruct.sx, gdImageStruct.sy);
                toRet.gdImageStructPtr = pNativeImage;
                return toRet;
            }
            else
            {
                throw new InvalidOperationException(SR.NullStreamReferenced);
            }

        }

        public static void WriteToStream(Image bmp, Stream stream)
        {
            LibGDLinuxImports.gdImageSaveAlpha(bmp.gdImageStructPtr, 1);

            LibGDLinuxImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(bmp.gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream);
            LibGDLinuxImports.gdImagePngCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }




    }
}


#endif
