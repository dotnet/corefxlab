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
        //Action act = () => Image.Create(1, -1);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    {
        //Action act = () => Image.Create(-1, 1);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    {
        //Action act = () => Image.Create(-1, -1);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    {
        //Action act = () => Image.Create(0, 1);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    {
        //Action act = () => Image.Create(1, 0);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    {
        //Action act = () => Image.Create(0, 0);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
    }
    [Fact]
    public void WhenCreatingAnImageFromAValidFileGiveAValidImage()
    {
        string filepath = "";
        Image fromFile = Image.Load(filepath);
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
        //string invalidFilepath = "C://";
        //Action act = () => Image.Load(invalidFilepath);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Malformed file path given",
        //    "because you can't read a wrongly formed path");
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //string invalidFilepath = "C:\\Documents\\Documents\\Documents\\documents.jpeg";
        //Action act = () => Image.Load(invalidFilepath);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Path not found",
        //    "because you can't read from an unexistent path");
    }
    [Fact]
    public void WhenCreatingAnImageFromAnThenThrowException()
    {
        //string invalidFilepath = "C:\\Documents\\doc.docx";
        //Action act = () => Image.Load(invalidFilepath);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("File path not an image",
        //    "because you can't read an image from a non-image file");
    }

    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAnImageFromAValidStreamThenGiveValidImage()
    {
        Stream stream = null;
        Image fromStream = Image.Load(stream);
        ValidateImage(fromStream, 0, 0, PixelFormat.ARGB);
    }
    [Fact]
    public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    {
        //Stream stream = new Stream();
        //Action act = () => Image.Load(stream);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image creation stream",
        //    "because you can't read an image from a non existent stream");
    }

    /* Test Resize */
    [Fact]
    public void WhenResizingEmptyImageThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        //emptyResizeSquare.Resize(10, 10);
        ValidateImage(emptyResizeSquare, 10, 10, emptyResizeSquare.PixelFormat);
    }
    [Fact]
    public void WhenResizingImageLoadedFromFileThenGiveAValidatedResizedImage()
    {
        string filepath = "";
        Image fromFileResizeSquare = Image.Load(filepath);
        //fromFileResizeSquare.Resize(10, 10);
        ValidateImage(fromFileResizeSquare, 10, 10, fromFileResizeSquare.PixelFormat);
    }
    [Fact]
    public void WhenResizingImageLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        Stream stream = null;
        Image fromStreamResizeSquare = Image.Load(stream);
        //fromStreamResizeSquare.Resize(10, 10);
        ValidateImage(fromStreamResizeSquare, 10, 10, fromStreamResizeSquare.PixelFormat);
    }

    /* Testing Resize parameters */
    [Fact]
    public void WhenResizingImageGivenZeroWidthThenThrowException()
    {
        //Not sure if this is how to do this
        //Action act = () => ImageExtensions.Resize(Image.Create(100, 100), 10, 10);
        //act.ShouldThrow<InvalidOperationException>().WithMessage("Invalid image size given for creation. Parameters must be positive numbers",
        //    "because an image has to have positive width/height");
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
