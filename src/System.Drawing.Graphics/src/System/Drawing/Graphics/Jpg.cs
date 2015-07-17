using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing.Graphics
{
    public static class Jpg
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

                if (gdImageStruct.trueColor == 0)
                {
                    int a = DLLImports.gdImagePaletteToTrueColor(bmp.gdImageStructPtr);
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
    }
}
