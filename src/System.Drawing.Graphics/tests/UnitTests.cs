// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Xunit;
using System.Drawing.Graphics;
using System.IO;


public partial class GraphicsUnitTests
{
    [Fact]
    public static void TestForLibGDCalls()
    {
        System.Console.WriteLine("hi");
        IntPtr imgPtr = DLLImports.gdImageCreate(1, 1);
        System.Console.WriteLine(imgPtr.ToInt64());

    }

    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        ////create an empty 10x10 image
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateImage(emptyTenSquare, 10, 10, PixelFormat.ARGB);

    }

    private static void ValidateImage(Image image, int widthToCompare, int heightToCompare,
                               PixelFormat formatToCompare)
    {
        //ApprovalTests.Approvals.Verify(image.ToString());
        //image.HeightInPixels.ShouldBeEquivalentTo(heightToCompare, "the height of the image we want to check is this");
        //image.PixelFormat.Should().Be(formatToCompare, "this is the Pixel Format this image should be");
        //image.Data.Should().NotBeNull("an initialized image data should neve be null");
        Assert.Equal(image.WidthInPixels, widthToCompare);
        Assert.Equal(image.HeightInPixels, heightToCompare);
        Assert.NotNull(image.Data);


    }

    /* Tests Create Method */
    [Fact]
    public void WhenCreatingABlankImageWithNegativeHeightThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(1, -1));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(-1, 1));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(-1, -1));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(1, 0));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(0, 1));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    {
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(0, 0));
        Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenCreatingAnImageFromAValidFileGiveAValidImage()
    {
        string filepath = "";
        Image fromFile = Image.Load(filepath);
        //arbitraily passing in pixelformat.argb now and 0, 0
        ValidateImage(fromFile, 0, 0, PixelFormat.ARGB);
    }

    //File I/O should be returning exceptions --> HOW TO DO THIS?!!?
    //Invalid file path
    //Path not found
    //Path not an image
    /* Tests Load(filepath) method */
    [Fact]
    public void WhenCreatingAnImageFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C://";
        Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Malformed file path given", exception.Message);
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C:\\Documents\\Documents\\Documents\\documents.jpeg";
        Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Malformed file path given", exception.Message);
    }
    [Fact]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C:\\Documents\\doc.docx";
        Exception exception = Assert.Throws<FileLoadException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Path given is not an image", exception.Message);
    }

    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAnImageFromAValidStreamThenGiveValidImage()
    {
        //placeholder stream
        Stream stream = null;
        Image fromStream = Image.Load(stream);
        //arbitraily passing in pixelformat.argb now and 0, 0
        ValidateImage(fromStream, 0, 0, PixelFormat.ARGB);
    }
    [Fact]
    public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    {
        Stream stream = null;
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Load(stream));
        Assert.Equal("Stream given is not valid", exception.Message);
    }

    /* Test Resize */
    [Fact]
    public void WhenResizingEmptyImageThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(emptyResizeSquare, 10, 10, PixelFormat.ARGB);
    }
    [Fact]
    public void WhenResizingImageLoadedFromFileThenGiveAValidatedResizedImage()
    {
        string filepath = "";
        Image fromFileResizeSquare = Image.Load(filepath);
        fromFileResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(fromFileResizeSquare, 10, 10, PixelFormat.ARGB);
    }
    [Fact]
    public void WhenResizingImageLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        Stream stream = null;
        Image fromStreamResizeSquare = Image.Load(stream);
        fromStreamResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(fromStreamResizeSquare, 10, 10, PixelFormat.ARGB);
    }

    /* Testing Resize parameters */
    [Fact]
    public void WhenResizingImageGivenNegativeHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(-1, 1));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenResizingImageGivenNegativeWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(1, -1));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenResizingImageGivenNegativeSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(-1, -1));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenResizingImageGivenZeroHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(0, 1));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenResizingImageGivenZeroWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(1, 0));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }
    [Fact]
    public void WhenResizingImageGivenZeroSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        //Not sure if this is how to do this
        Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(0, 0));
        Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    }




    //[Fact]
    //public void WhenWritingAnImageToAValidFileWriteToAValidFile()
    //{
    //    string filepath = "";
    //    string newfilepath = "";
    //    int height = 0;
    //    int width = 0;

    //    PixelFormat format = PixelFormat.RGBA;

    //    Image fromFile = Image.Load(filepath);
    //    fromFile.Save(newfilepath, format);
    //    Assert.AreEqual(height, fromFile.HeightInPixels);
    //    Assert.AreEqual(width, fromFile.WidthInPixels);
    //    Assert.IsNotNull(fromFile.Data);
    //    Assert.AreEqual(format, fromFile.PixelFormat);


    //}

    //[Fact]
    //public void WhenWritingAByteArrayToFileToAValidFile(string _FileName, byte[] _ByteArray)
    //{
    //    System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
    //    _FileStream.Write(_ByteArray, 0, _ByteArray.Length);
    //    _FileStream.Close();

    //}
}
