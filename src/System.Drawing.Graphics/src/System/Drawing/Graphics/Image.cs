using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace System.Drawing.Graphics
{
    public enum PixelFormat
    {
        ARGB,
        RGBA,
        CMYK
    }

    public class Image
    {
            /* Private Fields */
        private byte[] _data;
        private int _width;
        private int _height;

            /* Properties */
        public int WidthInPixels
        {
            get { return _width; }
        }
        public int HeightInPixels
        {
            get { return _height; }
        }
        public PixelFormat PixelFormat
        {
            get { return PixelFormat.RGBA; }
        }
        public byte[] Data
        {
            get { return _data; }
        }
        public int BytesPerPixel
        {
            get { return 0; }
        }

            /* Factory Methods */
        public static Image Create(int width, int height)
        {
            return new Image(width, height);
        }
        public static Image Load(string filePath)
        {
            return new Image(filePath);
        }
        public static Image Load(object stream)
        {
            return new Image(stream);
        }

            /* Constructors */
        private Image(int width, int height)
        {
            throw new NotImplementedException();
        }
        private Image(string filepath)
        {
            throw new NotImplementedException();
        }
        private Image(object stream)
        {
            throw new NotImplementedException();
        }


    }
}
