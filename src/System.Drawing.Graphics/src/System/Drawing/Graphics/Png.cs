using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing.Graphics
{
    public static class Png
    {
        public static Bitmap Create(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                return new Bitmap(DLLImports.gdImageCreateTrueColor(width, height));
            }
            else
            {
                throw new InvalidOperationException("Parameters for creating an image must be positive integers.");
            }
        }

        //add png specific method later
        public static Bitmap Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Malformed file path given.");
            }
            else if (DLLImports.gdSupportsFileType(filePath, false))
            {
                Bitmap bmp = new Bitmap(DLLImports.gdImageCreateFromFile(filePath));
                DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);

                if (!bmp.TrueColor)
                {
                    DLLImports.gdImagePaletteToTrueColor(bmp.gdImageStructPtr);
                    gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);
                }
                return bmp;
            }
            else
            {
                throw new FileLoadException("File type not supported.");
            }
        }

        //add png specific method later
        public static void WriteToFile(Bitmap bmp, string filePath)
        {
            DLLImports.gdImageSaveAlpha(bmp.gdImageStructPtr, 1);

            if (!DLLImports.gdSupportsFileType(filePath, true))
            {
                throw new InvalidOperationException("File type not supported or not found.");
            }
            else
            {
                if (!DLLImports.gdImageFile(bmp.gdImageStructPtr, filePath))
                {
                    throw new FileLoadException("Failed to write to file.");
                }
            }
        }


        public static Bitmap Load(Stream stream)
        {

            IntPtr pNativeImage = IntPtr.Zero;
            var wrapper = new gdStreamWrapper(stream);
            pNativeImage = DLLImports.gdImageCreateFromPngCtx(ref wrapper.IOCallbacks);

            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(pNativeImage);
            Bitmap toRet = Bitmap.Create(gdImageStruct.sx, gdImageStruct.sy);
            toRet.gdImageStructPtr = pNativeImage;
            return toRet;
        }

        public static void WriteToStream(Bitmap bmp, Stream stream)
        {
            DLLImports.gdImageStruct gdImageStruct = Marshal.PtrToStructure<DLLImports.gdImageStruct>(bmp.gdImageStructPtr);
            var wrapper = new gdStreamWrapper(stream);
            DLLImports.gdImagePngCtx(ref gdImageStruct, ref wrapper.IOCallbacks);
        }




    }
}
