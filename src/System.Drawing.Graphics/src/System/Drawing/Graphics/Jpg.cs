// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

//#define WINDOWS


using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
#if WINDOWS
    public static class Jpg
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

                if (!img.TrueColor)
                {
                    DLLImports.gdImagePaletteToTrueColor(img.gdImageStructPtr);
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


        public static Image Load(Stream stream)
        {
            IntPtr pNativeImage = IntPtr.Zero;
            var wrapper = new gdStreamWrapper(stream);
            pNativeImage = DLLImports.gdImageCreateFromJpegCtx(ref wrapper.IOCallbacks);

            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(pNativeImage);
            Image toRet = Image.Create(gdImageStruct.sx, gdImageStruct.sy);
            toRet.gdImageStructPtr = pNativeImage;
            return toRet;

        }

        public static void WriteToStream(Image img, Stream stream)
        {
            IntPtr point = img.gdImageStructPtr;
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(point);
            var wrapper = new gdStreamWrapper(stream);
            DLLImports.gdImageJpegCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }


    }
}

#else

    public static class Jpg
    {
        //add png specific method later
        public static Image Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Malformed file path given.");
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
                throw new FileLoadException("File type not supported.");
            }
        }

        //add png specific method later
        public static void WriteToFile(Image img, string filePath)
        {
            LibGDLinuxImports.gdImageSaveAlpha(img.gdImageStructPtr, 1);

            if (!LibGDLinuxImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException("File type not supported or not found.");
            }
            else
            {
                if (!LibGDLinuxImports.gdImageFile(img.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException("Failed to write to file.");
                }
            }
        }


        public static Image Load(Stream stream)
        {
            IntPtr pNativeImage = IntPtr.Zero;
            var wrapper = new gdStreamWrapper(stream);
            pNativeImage = LibGDLinuxImports.gdImageCreateFromJpegCtx(ref wrapper.IOCallbacks);

            LibGDLinuxImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(pNativeImage);
            Image toRet = Image.Create(gdImageStruct.sx, gdImageStruct.sy);
            toRet.gdImageStructPtr = pNativeImage;
            return toRet;

        }

        public static void WriteToStream(Image img, Stream stream)
        {
            IntPtr point = img.gdImageStructPtr;
            LibGDLinuxImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<LibGDLinuxImports.gdImageStruct>(point);
            var wrapper = new gdStreamWrapper(stream);
            LibGDLinuxImports.gdImageJpegCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }


    }
}

#endif
