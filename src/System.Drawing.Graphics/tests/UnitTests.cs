// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Xunit;
using System.Drawing.Graphics;
using System.IO;


public partial class GraphicsUnitTests
{

    private static void ValidateImage(Image img, int widthToCompare, int heightToCompare)
    {
        Assert.Equal(widthToCompare, img.WidthInPixels);
        Assert.Equal(heightToCompare, img.HeightInPixels);
    }

    /* Tests Create Method */
    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        //create an empty 10x10 image
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateImage(emptyTenSquare, 10, 10);
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeHeightThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(1, -1));
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(-1, 1));
    }
    [Fact]
    public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(-1, -1));
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(1, 0));
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(0, 1));
    }
    [Fact]
    public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    {
        Assert.Throws<InvalidOperationException>(() => Image.Create(0, 0));
    }
    [Fact]
    public void WhenCreatingAJpegFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
        Image fromFile = Jpg.Load(filepath);
        ValidateImage(fromFile, 600, 701);
    }
    [Fact]
    public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
    }


    /* Tests Load(filepath) method */
    [Fact]
    public void WhenCreatingAJpegFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"C:Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"C:Users\t-dahid\Pictures\TestPics\BlackCat.png";
        Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareDog.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"C:\Users\t-dahid\Documents\GitHub\corefxlab\src\System.Drawing.Graphics\text.txt";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }

    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAJpegFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 400, 249);
            Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamWrite.jpg");
        }

    }
    [Fact]
    public void WhenCreatingAPngFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 360, 362);
            Png.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamWrite.png");
        }

    }
    [Fact]
    public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    {
        Stream stream = null;
        Assert.Throws<InvalidOperationException>(() => Png.Load(stream));
    }

    /* Test Resize */
    [Fact]
    public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
        ValidateImage(emptyResizeSquare, 10, 10);
    }
    [Fact]
    public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    {
        Image emptyResizeSquare = Image.Create(100, 100);
        emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
        ValidateImage(emptyResizeSquare, 200, 200);
    }
    [Fact]
    public void WhenResizingJpegLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        Png.WriteToFile(fromFileResizeSquare, @"C:\Users\t-dahid\Pictures\TestFromFileResizedWrite.jpg");
    }
    [Fact]
    public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        Png.WriteToFile(fromFileResizeSquare, @"C:\Users\t-dahid\Pictures\TestFromFileResizedWrite.png");
    }
    [Fact]
    public void WhenResizingJpegLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 400, 249);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            Jpg.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamResizedWrite.jpg");
        }
    }

    [Fact]
    public void WhenResizingPngLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 360, 362);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            Png.WriteToFile(fromStream, @"C:\Users\t-dahid\Pictures\TestFromStreamResizedWrite.png");
        }
    }

    /* Testing Resize parameters */
    [Fact]
    public void WhenResizingImageGivenNegativeHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(-1, 1));
    }
    [Fact]
    public void WhenResizingImageGivenNegativeWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(1, -1));
    }
    [Fact]
    public void WhenResizingImageGivenNegativeSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(-1, -1));
    }
    [Fact]
    public void WhenResizingImageGivenZeroHeightThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(0, 1));
    }
    [Fact]
    public void WhenResizingImageGivenZeroWidthThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(1, 0));
    }
    [Fact]
    public void WhenResizingImageGivenZeroSizesThenThrowException()
    {
        Image img = Image.Create(1, 1);
        Assert.Throws<InvalidOperationException>(() => img.Resize(0, 0));
    }

    /* Tests Writing to a file*/
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Jpg.WriteToFile(emptyImage, @"C:\Users\t-dahid\Pictures\TESTBlankWrite.jpg");
    }
    [Fact]
    public void WhenWritingABlankCreatedPngToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Png.WriteToFile(emptyImage, @"C:\Users\t-dahid\Pictures\TESTBlankWrite.png");
    }
    [Fact]
    public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    {
        //checking with cat image
        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 600, 701);
        Png.WriteToFile(fromFile, @"C:\Users\t-dahid\Pictures\TestFileWrite.jpg");
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    {
        //checking with cat image
        string filepath = @"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
        Png.WriteToFile(fromFile, @"C:\Users\t-dahid\Pictures\TestFileWrite.png");
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"C:\Users\t-dahid\Pictures\TestFromFileFileTransparentWrite.png");

    }

    [Fact]
    public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        Png.WriteToFile(img, @"C:\Users\t-dahid\Pictures\TestFromFileFileTransparentResizeWrite.png");
    }

    /* Tests Writing to a Stream*/
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidStreamWriteToAValidStream()
    {
        Image img = Image.Create(100, 100);
        using(MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestBlankStreamWrite.jpg");
        }
    }
    public void WhenWritingABlankCreatedPngToAValidStreamWriteToAValidStream()
    {
        Image img = Image.Create(100, 100);
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestBlankStreamWrite.png");
        }
    }
    [Fact]
    public void WhenWritingAJpegFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamWrite.jpg");
        }
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamWrite.png");
        }
    }

    [Fact]
    public void WhenWritingAResizedJpegToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            ValidateImage(img, 40, 40);
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.jpg");
        }
    }
    [Fact]
    public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            //comment
            img = img.Resize(40, 40);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamResizeWrite.png");
        }
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            ValidateImage(img, 360, 362);
            img.SetAlphaPercentage(.2);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImage(img2, 360, 362);
            Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamTransparentWrite.png");
        }

    }

    [Fact]
    public void WhenWritingATransparentResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            ValidateImage(img, 360, 362);
            img.SetAlphaPercentage(.2);
            img = img.Resize(400, 400);
            ValidateImage(img, 400, 400);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImage(img2, 400, 400);
            Png.WriteToFile(img2, @"C:\Users\t-dahid\Pictures\TestFromFileStreamTransparentResizeWrite.png");
        }
    }

    /* Test Draw */
    [Fact]
    public void WhenDrawingTwoImagesWriteACorrectResult()
    {
        //open yellow cat image
        Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        //open black cat image
        Image blackCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        //draw & Write
        yellowCat.Draw(blackCat, 0, 0);
        Jpg.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawTest.png");
    }
    /* Test SetTransparency */
    [Fact]
    public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    {
        //open black cat image
        Image blackCat0 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(blackCat0, 220, 220);
        blackCat0.SetAlphaPercentage(0);
        ValidateImage(blackCat0, 220, 220);
        Png.WriteToFile(blackCat0, @"C:\Users\t-dahid\Pictures\SetTransparencyTest0.png");

        Image blackCat1 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(blackCat1, 220, 220);
        blackCat1.SetAlphaPercentage(.7);
        ValidateImage(blackCat1, 220, 220);
        Png.WriteToFile(blackCat1, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest1.png");

        Image blackCat2 = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(blackCat2, 220, 220);
        blackCat2.SetAlphaPercentage(1);
        ValidateImage(blackCat2, 220, 220);
        Png.WriteToFile(blackCat2, @"C:\Users\t-dahid\\Pictures\SetTransparencyTest2.png");
    }
    /* Test Draw and Set Transparency */
    [Fact]
    public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    {
        //black cat load
        Image blackCat = Png.Load(@"C:\Users\t-dahid\Pictures\TestPics\BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        blackCat.SetAlphaPercentage(0.5);
        //yellow cat load
        Image yellowCat = Jpg.Load(@"C:\Users\t-dahid\Pictures\TestPics\SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        yellowCat.Draw(blackCat, 0, 0);
        ValidateImage(yellowCat, 600, 701);
        //write
        Png.WriteToFile(yellowCat, @"C:\Users\t-dahid\Pictures\DrawAndTransparencyTest.png");
    }

}




