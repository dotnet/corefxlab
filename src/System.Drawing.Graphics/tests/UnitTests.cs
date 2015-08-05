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
        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg";
        Image fromFile = Jpg.Load(filepath);
        ValidateImage(fromFile, 600, 701);
    }
    [Fact]
    public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
    }


    /* Tests Load(filepath) method */
    [Fact]
    public void WhenCreatingAJpegFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png";
        Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareDog.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/text.txt";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }

    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAJpegFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 500, 311);
            Jpg.WriteToFile(fromStream, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromStreamWrite.jpg");
        }

    }
    [Fact]
    public void WhenCreatingAPngFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 425, 383);
            Png.WriteToFile(fromStream, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromStreamWrite.png");
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

        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        Png.WriteToFile(fromFileResizeSquare, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileResizedWrite.jpg");
    }
    [Fact]
    public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        Png.WriteToFile(fromFileResizeSquare, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileResizedWrite.png");
    }
    [Fact]
    public void WhenResizingJpegLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 500, 311);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            Jpg.WriteToFile(fromStream, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromStreamResizedWrite.jpg");
        }
    }

    [Fact]
    public void WhenResizingPngLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 425, 383);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            Png.WriteToFile(fromStream, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromStreamResizedWrite.png");
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
        Jpg.WriteToFile(emptyImage, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TESTBlankWrite.jpg");
    }
    [Fact]
    public void WhenWritingABlankCreatedPngToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Png.WriteToFile(emptyImage, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TESTBlankWrite.png");
    }
    [Fact]
    public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    {
        //checking with cat image
        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 600, 701);
        Png.WriteToFile(fromFile, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFileWrite.jpg");
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    {
        //checking with cat image
        string filepath = @"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
        Png.WriteToFile(fromFile, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFileWrite.png");
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileFileTransparentWrite.png");

    }

    [Fact]
    public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        Png.WriteToFile(img, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileFileTransparentResizeWrite.png");
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
            Jpg.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestBlankStreamWrite.jpg");
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
            Jpg.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestBlankStreamWrite.png");
        }
    }
    [Fact]
    public void WhenWritingAJpegFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamWrite.jpg");
        }
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamWrite.png");
        }
    }

    [Fact]
    public void WhenWritingAResizedJpegToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            ValidateImage(img, 40, 40);
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamResizeWrite.jpg");
        }
    }
    [Fact]
    public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            //comment
            img = img.Resize(40, 40);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamResizeWrite.png");
        }
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            ValidateImage(img, 425, 383);
            img.SetAlphaPercentage(.2);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImage(img2, 425, 383);
            Png.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamTransparentWrite.png");
        }

    }

    [Fact]
    public void WhenWritingATransparentResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            ValidateImage(img, 400, 383);
            img.SetAlphaPercentage(.2);
            img = img.Resize(400, 400);
            ValidateImage(img, 400, 400);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImage(img2, 400, 400);
            Png.WriteToFile(img2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/TestFromFileStreamTransparentResizeWrite.png");
        }
    }

    /* Test Draw */
    [Fact]
    public void WhenDrawingTwoImagesWriteACorrectResult()
    {
        //open yellow cat image
        Image yellowCat = Jpg.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        //open black cat image
        Image blackCat = Jpg.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        //draw & Write
        yellowCat.Draw(blackCat, 0, 0);
        Jpg.WriteToFile(yellowCat, @"/Users/matell/Documents/OSX-S.D.G./TestImages/DrawTest.png");
    }
    /* Test SetTransparency */
    [Fact]
    public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    {
        //open black cat image
        Image blackCat0 = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(blackCat0, 220, 220);
        blackCat0.SetAlphaPercentage(0);
        ValidateImage(blackCat0, 220, 220);
        Png.WriteToFile(blackCat0, @"/Users/matell/Documents/OSX-S.D.G./TestImages/SetTransparencyTest0.png");

        Image blackCat1 = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(blackCat1, 220, 220);
        blackCat1.SetAlphaPercentage(.7);
        ValidateImage(blackCat1, 220, 220);
        Png.WriteToFile(blackCat1, @"/Users/matell/Documents/OSX-S.D.G./TestImages/SetTransparencyTest1.png");

        Image blackCat2 = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(blackCat2, 220, 220);
        blackCat2.SetAlphaPercentage(1);
        ValidateImage(blackCat2, 220, 220);
        Png.WriteToFile(blackCat2, @"/Users/matell/Documents/OSX-S.D.G./TestImages/SetTransparencyTest2.png");
    }
    /* Test Draw and Set Transparency */
    [Fact]
    public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    {
        //black cat load
        Image blackCat = Png.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        blackCat.SetAlphaPercentage(0.5);
        //yellow cat load
        Image yellowCat = Jpg.Load(@"/Users/matell/Documents/OSX-S.D.G./TestImages/SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        yellowCat.Draw(blackCat, 0, 0);
        ValidateImage(yellowCat, 600, 701);
        //write
        Png.WriteToFile(yellowCat, @"/Users/matell/Documents/OSX-S.D.G./TestImages/DrawAndTransparencyTest.png");
    }

}




