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
    public static void Test2()
    {
        FileStream filestream = new FileStream(@"C:\Users\t-dahid\Pictures\BlackCat.png", FileMode.Open);

        Image catStreamTest = Png.Load((Stream)filestream);
        Png.WriteToFile(catStreamTest, @"C:\Users\t-dahid\Pictures\TestWrite1.png");


        using (var stream = new MemoryStream())
        {
            //load an image to file & change transparency
            Image soccerCat = Jpg.Load(@"C:\users\t-dahid\Pictures\SoccerCat.jpg");
            soccerCat.SetAlphaPercentage(10);

            Jpg.WriteToStream(soccerCat, stream);
            stream.Position = 0;
            Image soccerCatTest = Jpg.Load((Stream)stream);

            Jpg.WriteToFile(soccerCatTest, @"C:\Users\t-dahid\Pictures\StreamTest2SoccercAT.jpg");
        }


        Image cat1 = Png.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
        Image cat2 = Jpg.Load(@"C:\Users\t-dahid\Pictures\SquareCat.jpg");

        cat1.SetAlphaPercentage(40);
        Png.WriteToFile(cat1, @"C:\Users\t-dahid\Pictures\TransparentBlackCat.png");

        Image transparentCat1 = Png.Load(@"C:\Users\t-dahid\Pictures\TransparentBlackCat.png");
        cat2.Draw(transparentCat1, 0, 1);
        Png.WriteToFile(cat2, @"C:\Users\t-dahid\Pictures\TestWrite2.png");
    }


    private static void ValidateImage(Image img, int widthToCompare, int heightToCompare)
    {
        Assert.Equal(img.WidthInPixels, widthToCompare);
        Assert.Equal(img.HeightInPixels, heightToCompare);
    }

    ///* Tests Create Method */

    [Fact]
    public static void WhenCreatingAnEmptyImageThenValidateAnImage()
    {
        ////create an empty 10x10 image
        Image emptyTenSquare = Image.Create(10, 10);
        ValidateImage(emptyTenSquare, 10, 10);
    }
    //[Fact]
    //public void WhenCreatingABlankImageWithNegativeHeightThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(1, -1));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenCreatingABlankImageWithNegativeWidthThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(-1, 1));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenCreatingABlankImageWithNegativeSizesThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(-1, -1));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenCreatingABlankImageWithZeroHeightThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(1, 0));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenCreatingABlankImageWithZeroWidthThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(0, 1));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenCreatingABlankImageWithZeroParametersThenThrowException()
    //{
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Create(0, 0));
    //    Assert.Equal("Parameters for creating an image must be positive integers.", exception.Message);
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAValidFileGiveAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
    //    Image fromFile = Image.Load(filepath);
    //    ValidateImage(fromFile, 600, 701);
    //}

    ////File I/O should be returning exceptions --> HOW TO DO THIS?!!?
    ////Invalid file path
    ////Path not found
    ////Path not an image

    ///* Tests Load(filepath) method */
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAMalformedPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
    //    Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
    //    Assert.Equal("Malformed file path given.", exception.Message);
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAnUnfoundPathThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareDog.jpg";
    //    Exception exception = Assert.Throws<FileNotFoundException>(() => Image.Load(invalidFilepath));
    //    Assert.Equal("Malformed file path given.", exception.Message);
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAFileTypeThatIsNotAnImageThenThrowException()
    //{
    //    //place holder string to demonstrate what would be the error case
    //    string invalidFilepath = @"C:\Users\t-dahid\Documents\GitHub\corefxlab\src\System.Drawing.Graphics\text.txt";
    //    Exception exception = Assert.Throws<FileLoadException>(() => Image.Load(invalidFilepath));
    //    Assert.Equal("File type not supported.", exception.Message);
    //}

    ///* Tests Load(stream) mehtod*/
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAValidStreamThenGiveValidImage()
    //{
    //    //placeholder stream
    //    Stream stream = null;
    //    Image fromStream = Image.Load(stream);
    //    //arbitraily passing in pixelformat.argb now and 0, 0
    //    ValidateImage(fromStream, 0, 0);
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenCreatingAnImageFromAnInvalidStreamThenThrowException()
    //{
    //    Stream stream = null;
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => Image.Load(stream));
    //    Assert.Equal("Stream given is not valid", exception.Message);
    //}

    ///* Test Resize */
    //[Fact]
    //public void WhenResizingEmptyImageDownThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(10, 10);
    //    //arbitraily passing in pixelformat.argb now 
    //    ValidateImage(emptyResizeSquare, 10, 10);
    //}
    //[Fact]
    //public void WhenResizingEmptyImageUpThenGiveAValidatedResizedImage()
    //{
    //    Image emptyResizeSquare = Image.Create(100, 100);
    //    emptyResizeSquare = emptyResizeSquare.Resize(200, 200);
    //    //arbitraily passing in pixelformat.argb now 
    //    ValidateImage(emptyResizeSquare, 200, 200);
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenResizingImageLoadedFromFileThenGiveAValidatedResizedImage()
    //{
    //    string filepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
    //    Image fromFileResizeSquare = Image.Load(filepath);
    //    fromFileResizeSquare = fromFileResizeSquare.Resize(100, 100);
    //    //arbitraily passing in pixelformat.argb now 
    //    ValidateImage(fromFileResizeSquare, 100, 100);
    //    fromFileResizeSquare.WriteToFile(@"C:\Users\t-dahid\Pictures\TESTCATResized.jpg");
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenResizingImageLoadedFromStreamThenGiveAValidatedResizedImage()
    //{
    //    Stream stream = null;
    //    Image fromStreamResizeSquare = Image.Load(stream);
    //    fromStreamResizeSquare.Resize(10, 10);
    //    //arbitraily passing in pixelformat.argb now 
    //    ValidateImage(fromStreamResizeSquare, 10, 10);
    //}

    ///* Testing Resize parameters */
    //[Fact]
    //public void WhenResizingImageGivenNegativeHeightThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(-1, 1));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenResizingImageGivenNegativeWidthThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(1, -1));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenResizingImageGivenNegativeSizesThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(-1, -1));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenResizingImageGivenZeroHeightThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(0, 1));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenResizingImageGivenZeroWidthThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(1, 0));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}
    //[Fact]
    //public void WhenResizingImageGivenZeroSizesThenThrowException()
    //{
    //    Image img = Image.Create(1, 1);
    //    //Not sure if this is how to do this
    //    Exception exception = Assert.Throws<InvalidOperationException>(() => img.Resize(0, 0));
    //    Assert.Equal("Parameters for resizing an image must be positive integers.", exception.Message);
    //}

    ///* Tests Writing to a file*/
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenWritingABlankCreatedImageToAValidFileWriteToAValidFile()
    //{
    //    Image emptyImage = Image.Create(10, 10);
    //    ValidateImage(emptyImage, 10, 10);
    //    emptyImage.WriteToFile(@"C:\Users\t-dahid\Pictures\TESTBlankWrite.jpg");
    //}
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenWritingAnImageCreatedFromFileToAValidFileWriteAValidImage()
    //{
    //    //checking with cat image
    //    string filepath = @"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg";
    //    Image fromFile = Image.Load(filepath);
    //    ValidateImage(fromFile, 600, 701);
    //    fromFile.WriteToFile(@"C:\Users\t-dahid\Pictures\TTestCatWrite.jpg");
    //}

    ///* Test Draw */
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenDrawingTwoImagesWriteACorrectResult()
    //{
    //    //open yellow cat image
    //    Image yellowCat = Image.Load(@"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    //open black cat image
    //    Image blackCat = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    //draw & Write
    //    yellowCat.Draw(blackCat, 0, 0);
    //    yellowCat.WriteToFile(@"C:\Users\t-dahid\Pictures\DrawTest.png");
    //}
    ///* Test SetTransparency */
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenSettingTheTransparencyOfAnImageWriteAnImageWithChangedTransparency()
    //{
    //    //open black cat image
    //    Image blackCat0 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat0, 220, 220);
    //    blackCat0.SetAlphaPercentage(0);
    //    ValidateImage(blackCat0, 220, 220);
    //    blackCat0.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest0.png");

    //    Image blackCat1 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat1, 220, 220);
    //    blackCat1.SetAlphaPercentage(10);
    //    ValidateImage(blackCat1, 220, 220);
    //    blackCat1.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest1.png");

    //    Image blackCat2 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat2, 220, 220);
    //    blackCat2.SetAlphaPercentage(20);
    //    ValidateImage(blackCat2, 220, 220);
    //    blackCat2.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest2.png");

    //    Image blackCat3 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat3, 220, 220);
    //    blackCat3.SetAlphaPercentage(30);
    //    ValidateImage(blackCat3, 220, 220);
    //    blackCat3.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest3.png");

    //    Image blackCat4 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat4, 220, 220);
    //    blackCat4.SetAlphaPercentage(40);
    //    ValidateImage(blackCat4, 220, 220);
    //    blackCat4.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest4.png");

    //    Image blackCat5 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat5, 220, 220);
    //    blackCat5.SetAlphaPercentage(50);
    //    ValidateImage(blackCat5, 220, 220);
    //    blackCat5.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest5.png");

    //    Image blackCat6 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat6, 220, 220);
    //    blackCat6.SetAlphaPercentage(60);
    //    ValidateImage(blackCat6, 220, 220);
    //    blackCat6.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest6.png");

    //    Image blackCat7 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat7, 220, 220);
    //    blackCat7.SetAlphaPercentage(70);
    //    ValidateImage(blackCat7, 220, 220);
    //    blackCat7.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest7.png");

    //    Image blackCat8 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat8, 220, 220);
    //    blackCat8.SetAlphaPercentage(80);
    //    ValidateImage(blackCat8, 220, 220);
    //    blackCat8.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest8.png");

    //    Image blackCat9 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat9, 220, 220);
    //    blackCat9.SetAlphaPercentage(90);
    //    ValidateImage(blackCat9, 220, 220);
    //    blackCat9.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest9.png");

    //    Image blackCat10 = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat10, 220, 220);
    //    blackCat10.SetAlphaPercentage(100);
    //    ValidateImage(blackCat2, 220, 220);
    //    blackCat10.WriteToFile(@"C:\Users\t-dahid\\Pictures\SetTransparencyTest10.png");
    //}
    ///* Test Draw and Set Transparency */
    //[Fact(Skip = "Not Implemented yet...")]
    //public void WhenDrawingAnImageWithTransparencyChangedGiveACorrectWrittenFile()
    //{
    //    //black cat load
    //    Image blackCat = Image.Load(@"C:\Users\t-dahid\Pictures\BlackCat.png");
    //    ValidateImage(blackCat, 220, 220);
    //    blackCat.SetAlphaPercentage(20);
    //    //yellow cat load
    //    Image yellowCat = Image.Load(@"C:\Users\t-dahid\Pictures\DemoPictures\1-ImageEx\SquareCat.jpg");
    //    ValidateImage(yellowCat, 600, 701);
    //    yellowCat.Draw(blackCat, 0, 0);
    //    ValidateImage(yellowCat, 600, 701);
    //    //write
    //    yellowCat.WriteToFile(@"C:\Users\t-dahid\Pictures\DrawAndTransparencyTest.png");

    //}

}




