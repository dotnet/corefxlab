// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Xunit;
using System.Drawing.Graphics;
using System.IO;


public partial class GraphicsUnitTests
{

    //[Fact]
    //public static void Test2()
    //{
    //    System.Console.WriteLine("Test 2");
    //    Image image = Image.Load("C:\\Users\\t-xix\\Pictures\\Test\\desk.JPG");
    //    System.Console.WriteLine(image.WidthInPixels);
    //    System.Console.WriteLine(image.HeightInPixels);
    //}
    [Fact]
    public static void Test()
    {

        Image catTest = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
        Image pikaTest = Image.Load(@"C:\Users\t-dahid\Pictures\PikachuSprite.png");

        catTest.SetTransparency(10);
        pikaTest.SetTransparency(30);

        catTest.WriteToFile(@"C:\Users\t-dahid\Pictures\BlackCat12312123.png");
        pikaTest.WriteToFile(@"C:\Users\t-dahid\Pictures\PikaTest.png");

        Image catSquare = Image.Load(@"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg");
        catSquare.Draw(catTest, 0, 0);

        //Image blank = Image.Load(@"C:\Users\t-dahid\Pictures\BlankSlide.jpg");

        catSquare.WriteToFile(@"C:\Users\t-dahid\Pictures\blankCatTest.png");


    }

    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        ////create an empty 10x10 image
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateImage(emptyTenSquare, 10, 10);
    }
    private static void ValidateImage(Image image, int widthToCompare, int heightToCompare)
    {
        Assert.Equal(image.WidthInPixels, widthToCompare);
        Assert.Equal(image.HeightInPixels, heightToCompare);
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
    [Fact(Skip = "for now")]
    public void WhenCreatingAnImageFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
        Image fromFile = Image.Load(filepath);
        System.Console.WriteLine(fromFile.WidthInPixels);
        System.Console.WriteLine(fromFile.HeightInPixels);
        ValidateImage(fromFile, 600, 701);
        fromFile.WriteToFile(@"C:\Users\t-dahid\Pictures\TESTCAT.jpg");

    }

    //File I/O should be returning exceptions --> HOW TO DO THIS?!!?
    //Invalid file path
    //Path not found
    //Path not an image
    /* Tests Load(filepath) method */
    [Fact(Skip = "not IMPLEMENTED YET ABOUYT TO DO SO>>>!!!")]
    public void WhenCreatingAnImageFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C:/sadfg/";
        Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Malformed file path given", exception.Message);
    }
    [Fact(Skip = "Not Implemented yet...")]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C:\\Documents\\Documents\\Documents\\documents.jpeg";
        Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Malformed file path given", exception.Message);
    }
    [Fact(Skip = "Not Implemented yet...")]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = "C:\\Documents\\doc.docx";
        Exception exception = Assert.Throws<FileLoadException>(() => Image.Load(invalidFilepath));
        Assert.Equal("Path given is not an image", exception.Message);
    }

    /* Tests Load(stream) mehtod*/
    [Fact(Skip = "Not Implemented yet...")]
    public void WhenCreatingAnImageFromAValidStreamThenGiveValidImage()
    {
        //placeholder stream
        Stream stream = null;
        Image fromStream = Image.Load(stream);
        //arbitraily passing in pixelformat.argb now and 0, 0
        ValidateImage(fromStream, 0, 0);
    }
    [Fact(Skip = "Not Implemented yet...")]
    public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    {
        Stream stream = null;
        Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Load(stream));
        Assert.Equal("Stream given is not valid", exception.Message);
    }

    /* Test Resize */
    [Fact]
    public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(emptyResizeSquare, 10, 10);
    }
    [Fact]
    public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(emptyResizeSquare, 200, 200);
    }
    [Fact(Skip = "for now...")]
    public void WhenResizingImageLoadedFromFileThenGiveAValidatedResizedImage()
    {
        string filepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
        Image fromFileResizeSquare = Image.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(fromFileResizeSquare, 10, 10);
        fromFileResizeSquare.WriteToFile(@"C:\Users\t-dahid\Pictures\TESTCAT22.jpg");
    }
    [Fact(Skip = "Not Implemented yet...")]
    public void WhenResizingImageLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        Stream stream = null;
        Image fromStreamResizeSquare = Image.Load(stream);
        fromStreamResizeSquare.Resize(10, 10);
        //arbitraily passing in pixelformat.argb now 
        ValidateImage(fromStreamResizeSquare, 10, 10);
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


    [Fact(Skip = "for now..")]
    public void WhenWritingAnImageToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        emptyImage.WriteToFile(@"C:\Users\t-dahid\Pictures\TEST.jpg");
    }

}




