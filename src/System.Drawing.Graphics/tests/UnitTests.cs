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
        //TODO: make this way better!!
    }

    /* Tests Create Method */
    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
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


    /* Tests Load(filepath) method */
    [Fact]
    public void WhenCreatingAJpegFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFile = Jpg.Load(filepath);
        ValidateImage(fromFile, 600, 701);
    }
    [Fact]
    public void WhenCreatingAPngFromAValidFileGiveAValidImage()
    {
        //checking with cat image
        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
    }
    [Fact]
    public void WhenCreatingAJpegFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/\SquareCat.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAPngFromAMalformedPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/\BlackCat.png";
        Assert.Throws<FileNotFoundException>(() => Png.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/SquareDog.jpg";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }
    [Fact]
    public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    {
        //place holder string to demonstrate what would be the error case
        string invalidFilepath = @"/home/ddcloud/Documents/text.txt";
        Assert.Throws<FileNotFoundException>(() => Jpg.Load(invalidFilepath));
    }


    /* Tests Load(stream) mehtod*/
    [Fact]
    public void WhenCreatingAJpegFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"/home/ddcloud/Documents/SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 400, 249);
            //must be commented out later (1 job rule)
            Jpg.WriteToFile(fromStream, @"/home/ddcloud/Documents/TestFromStreamWrite.jpg");
        }

    }
    [Fact]
    public void WhenCreatingAPngFromAValidStreamThenWriteAValidImageToFile()
    {
        using (FileStream filestream = new FileStream(@"/home/ddcloud/Documents/CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 360, 362);
            //must be commented out later (1 job rule)
            Png.WriteToFile(fromStream, @"/home/ddcloud/Documents/TestFromStreamWrite.png");
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

        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFileResizeSquare = Jpg.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(200, 200);
        ValidateImage(fromFileResizeSquare, 200, 200);
        //must be commented out later (1 job rule)
        Jpg.WriteToFile(fromFileResizeSquare, @"/home/ddcloud/Documents/TestFromFileResizedWrite.jpg");
    }
    [Fact]
    public void WhenResizingPngLoadedFromFileThenGiveAValidatedResizedImage()
    {

        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFileResizeSquare = Png.Load(filepath);
        fromFileResizeSquare = fromFileResizeSquare.Resize(400, 400);
        ValidateImage(fromFileResizeSquare, 400, 400);
        //must be commented out later (1 job rule)
        Png.WriteToFile(fromFileResizeSquare, @"/home/ddcloud/Documents/TestFromFileResizedWrite.png");
    }
    [Fact]
    public void WhenResizingJpegLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"/home/ddcloud/Documents/SoccerCat.jpg", FileMode.Open))
        {
            Image fromStream = Jpg.Load(filestream);
            ValidateImage(fromStream, 400, 249);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            //must be commented out later (1 job rule)
            Jpg.WriteToFile(fromStream, @"/home/ddcloud/Documents/TestFromStreamResizedWrite.jpg");
        }
    }

    [Fact]
    public void WhenResizingPngLoadedFromStreamThenGiveAValidatedResizedImage()
    {
        using (FileStream filestream = new FileStream(@"/home/ddcloud/Documents/CuteCat.png", FileMode.Open))
        {
            Image fromStream = Png.Load(filestream);
            ValidateImage(fromStream, 360, 362);
            fromStream = fromStream.Resize(400, 400);
            ValidateImage(fromStream, 400, 400);
            //must be commented out later (1 job rule)
            Png.WriteToFile(fromStream, @"/home/ddcloud/Documents/TestFromStreamResizedWrite.png");
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


    /* Test WriteToFile */
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Jpg.WriteToFile(emptyImage, @"/home/ddcloud/Documents/TestBlankWriteFile.jpg");
    }
    [Fact]
    public void WhenWritingABlankCreatedPngToAValidFileWriteToAValidFile()
    {
        Image emptyImage = Image.Create(10, 10);
        ValidateImage(emptyImage, 10, 10);
        Png.WriteToFile(emptyImage, @"/home/ddcloud/Documents/TestBlankWriteFile.png");
    }
    [Fact]
    public void WhenWritingAJpegCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = @"/home/ddcloud/Documents/SquareCat.jpg";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 600, 701);
        Png.WriteToFile(fromFile, @"/home/ddcloud/Documents/TestFromFileWriteFile.jpg");
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidFileWriteAValidImage()
    {
        string filepath = @"/home/ddcloud/Documents/BlackCat.png";
        Image fromFile = Png.Load(filepath);
        ValidateImage(fromFile, 220, 220);
        Png.WriteToFile(fromFile, @"/home/ddcloud/Documents/TestFromFileWriteFile.png");
    }
    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileTransparentWriteFile.png");
    }
    [Fact]
    public void WhenWritingATransparentResizedPngToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(img, 220, 220);
        img.SetAlphaPercentage(.2);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileTransparentResizeWriteFile.png");
    }
    [Fact]
    public void WhenWritingAResizedTransparentPngToAValidFileWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(img, 220, 220);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileResizeTransparentWriteFile.png");
    }



    /* Tests Writing to a Stream*/
    [Fact]
    public void WhenWritingABlankCreatedJpegToAValidStreamWriteToAValidStream()
    {
        Image img = Image.Create(100, 100);
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"/home/ddcloud/Documents/TestBlankWriteStream.jpg");
        }
    }
    public void WhenWritingABlankCreatedPngToAValidStreamWriteToAValidStream()
    {
        Image img = Image.Create(100, 100);
        using (MemoryStream stream = new MemoryStream())
        {
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"/home/ddcloud/Documents/TestBlankWriteStream.png");
        }
    }
    [Fact]
    public void WhenWritingAJpegFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"/home/ddcloud/Documents/SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileWriteStream.jpg");
        }
    }
    [Fact]
    public void WhenWritingAPngCreatedFromFileToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileWriteStream.png");
        }
    }

    [Fact]
    public void WhenWritingAResizedJpegToAValidStreamWriteAValidImage()
    {
        Image img = Jpg.Load(@"/home/ddcloud/Documents/SoccerCat.jpg");
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            ValidateImage(img, 40, 40);
            Jpg.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Jpg.Load(stream);
            Jpg.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileResizeWriteStream.jpg");
        }
    }
    [Fact]
    public void WhenWritingAResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            img = img.Resize(40, 40);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            Png.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileResizeWriteStream.png");
        }
    }

    [Fact]
    public void WhenWritingAPngMadeTransparentToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/CuteCat.png");
        using (MemoryStream stream = new MemoryStream())
        {
            ValidateImage(img, 360, 362);
            img.SetAlphaPercentage(.2);
            Png.WriteToStream(img, stream);
            stream.Position = 0;
            Image img2 = Png.Load(stream);
            ValidateImage(img2, 360, 362);
            Png.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileTransparentWriteStream.png");
        }
    }
    [Fact]
    public void WhenWritingATransparentResizedPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/CuteCat.png");
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
            Png.WriteToFile(img2, @"/home/ddcloud/Documents/TestFromFileTransparentResizeWriteStream.png");
        }
    }
    [Fact]
    public void WhenWritingAResizedTransparentPngToAValidStreamWriteAValidImage()
    {
        Image img = Png.Load(@"/home/ddcloud/Documents/CuteCat.png");
        ValidateImage(img, 360, 362);
        img = img.Resize(400, 400);
        ValidateImage(img, 400, 400);
        img.SetAlphaPercentage(.2);
        Png.WriteToFile(img, @"/home/ddcloud/Documents/TestFromFileResizeTransparentWriteStream.png");
    }

    /* Test Draw */
    [Fact]
    public void WhenDrawingTwoImagesWriteACorrectResult()
    {
        //open yellow cat image
        Image yellowCat = Jpg.Load(@"/home/ddcloud/Documents/SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        //open black cat image
        Image blackCat = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        //draw & Write
        yellowCat.Draw(blackCat, 0, 0);
        Png.WriteToFile(yellowCat, @"/home/ddcloud/Documents/DrawTest.png");
    }
    /* Test SetTransparency */
    [Fact]
    public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    {
        //open black cat image
        Image blackCat0 = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(blackCat0, 220, 220);
        blackCat0.SetAlphaPercentage(0);
        ValidateImage(blackCat0, 220, 220);
        Png.WriteToFile(blackCat0, @"/home/ddcloud/Documents/SetTransparencyTest0.png");

        Image blackCat1 = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(blackCat1, 220, 220);
        blackCat1.SetAlphaPercentage(.7);
        ValidateImage(blackCat1, 220, 220);
        Png.WriteToFile(blackCat1, @"/home/ddcloud/Documents/SetTransparencyTest1.png");

        Image blackCat2 = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(blackCat2, 220, 220);
        blackCat2.SetAlphaPercentage(1);
        ValidateImage(blackCat2, 220, 220);
        Png.WriteToFile(blackCat2, @"/home/ddcloud/Documents/SetTransparencyTest2.png");
    }
    /* Test Draw and Set Transparency */
    [Fact]
    public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    {
        //black cat load
        Image blackCat = Png.Load(@"/home/ddcloud/Documents/BlackCat.png");
        ValidateImage(blackCat, 220, 220);
        blackCat.SetAlphaPercentage(0.5);
        //yellow cat load
        Image yellowCat = Jpg.Load(@"/home/ddcloud/Documents/SquareCat.jpg");
        ValidateImage(yellowCat, 600, 701);
        yellowCat.Draw(blackCat, 0, 0);
        ValidateImage(yellowCat, 600, 701);
        //write
        Png.WriteToFile(yellowCat, @"/home/ddcloud/Documents/DrawAndTransparencyTest.png");
    }

}




